using Core.Entities.Journals.Services;
using Core.Shared.Data;
using Core.Shared.signalR;
using Core.Shared.UnitOfWork;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<AlarmCTX>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IJournalServices, JournalServices>();

builder.Services.AddScoped<AlarmUOW, AlarmUow>();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:4200")
            .WithMethods("GET", "POST", "HEAD", "PUT", "DELETE", "OPTIONS")
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddSignalR();


var app = builder.Build();

app.MapHub<AlarmsHub>("/alarmsHub");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseCors("AllowOrigin");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();


app.Run();