using System.Buffers.Binary;
using Core.Entities.Alarms;
using Core.Entities.Alarms.AlarmsPLC.Models.DB;
using Core.Shared.Data;
using testCVB;
using TwinCAT.Ads;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AlarmCTX>(options =>
   options.UseSqlServer(connectionString));

builder.Services.AddScoped<AdsService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



// Notification handle
ResultHandle[] hConnect = new ResultHandle[7];

async void AdsNotificationGetElementSub(dynamic dynamicObject)
{
    AdsClient? tcClient = (dynamicObject!.tcClient as AdsClient);
    tcClient!.WriteAny((uint)dynamicObject.msgAcquitHandle, Utils.IsReading);
    tcClient.WriteAny((uint)dynamicObject.msgNewHandle, Utils.NoMessage);
    // Get element of FIFO
    Alarm alarm = (Alarm)tcClient.ReadAny((uint)dynamicObject.oldEntryHandle, typeof(Alarm));
    AlarmPLC alarmPLC = new(alarm);
    await using AsyncServiceScope scope = ((IServiceProvider)dynamicObject.appServices).CreateAsyncScope();
    IServiceProvider services = scope.ServiceProvider;

    try
    {
        var serviceAds = services.GetRequiredService<AdsService>();
        await serviceAds.InsertAlarmPLC(alarmPLC);
        tcClient.WriteAny((uint)dynamicObject.msgAcquitHandle, Utils.FinishedReading);
    }
    catch
    {
        // Retry
        tcClient.WriteAny((uint)dynamicObject.msgAcquitHandle, Utils.ErrorWhileReading);
    }
}

async void AdsNotificationGetElement(object? sender, AdsNotificationEventArgs e)
{
    uint newMsg = BinaryPrimitives.ReadUInt32LittleEndian(e.Data.Span);
    if (e.Handle == hConnect[0].Handle && newMsg == Utils.HasNewMsg)
    {
        Console.WriteLine("Notif msgNew");
        // UserData is our data passed in parameter
        AdsNotificationGetElementSub(e.UserData as dynamic);
    }
}

app.MapGet("/alarm-plc", async () =>
{
    while (true)
    {

        CancellationToken cancel = CancellationToken.None;

        AdsClient tcClient = new AdsClient();

        while (true)
        {
            // Connection
            tcClient.Connect(851);
            if (tcClient.IsConnected)
            {
                break;
            }
            Console.WriteLine("Unable to connect to the automaton. Retrying in 1 second");
            Thread.Sleep(1000);
        }


        // Create dynamic Object to use in function notification
        dynamic myDynamic = new System.Dynamic.ExpandoObject();
        myDynamic.tcClient = tcClient;
        myDynamic.appServices = app.Services;

        // Use variable msgAcquit
        myDynamic.msgAcquitHandle = tcClient.CreateVariableHandle(Utils.AcquitMsg);
        myDynamic.msgNewHandle = tcClient.CreateVariableHandle(Utils.NewMsg);
        myDynamic.oldEntryHandle = tcClient.CreateVariableHandle(Utils.ToRead);

        int size = sizeof(UInt32);
        hConnect[0] = await tcClient.AddDeviceNotificationAsync(Utils.NewMsg, size, new NotificationSettings(AdsTransMode.OnChange, 0, 0), myDynamic, cancel);
        tcClient.AdsNotification += AdsNotificationGetElement;

        ResultValue<uint> newMsgValue = await tcClient.ReadAnyAsync<uint>(myDynamic.msgNewHandle, cancel);
        if (newMsgValue.ErrorCode != AdsErrorCode.NoError)
            throw new Exception(newMsgValue.ErrorCode.ToString());
        if (newMsgValue.Value == Utils.HasNewMsg)
            AdsNotificationGetElementSub(myDynamic);

        try
        {

            while ((await tcClient.ReadAnyAsync<uint>(myDynamic.msgNewHandle, cancel)).ErrorCode == AdsErrorCode.NoError)
            {
                // To avoid spamming the TwinCat
                Thread.Sleep(1000);
            }

            // Unregister the Event / Handle
            await tcClient.DeleteDeviceNotificationAsync(hConnect[0].Handle, cancel);
            tcClient.AdsNotification -= AdsNotificationGetElement;
            if (!(hConnect[0].Succeeded))
            {
                throw new Exception("Notification failed");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
})
.WithName("AlarmPLC")
.WithOpenApi();

app.Run();