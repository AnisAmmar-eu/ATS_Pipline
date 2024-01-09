using System.Runtime.CompilerServices;
using Core.Shared.Models.DB.Kernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TwinCAT.Ads;

namespace Core.Shared.Services.Notifications;

public class
	BaseNotification<T, TStruct>
	where TStruct : struct, IBaseADS<T>
{
	private uint _newMsg;
	private uint _oldEntry;
	private uint _remove;
	private ResultHandle _resultHandle = null!;
	protected bool ToDequeue = true;

	public BaseNotification(ResultHandle resultHandle, uint remove, uint newMsg, uint oldEntry)
	{
		_resultHandle = resultHandle;
		_remove = remove;
		_newMsg = newMsg;
		_oldEntry = oldEntry;
	}

	protected BaseNotification()
	{
	}

	protected static async Task<BaseNotification<T, TStruct>> CreateSub<TNotification>(
		dynamic ads,
		string removeSymbol,
		string newMsgSymbol,
		string oldEntrySymbol)
		where TNotification : BaseNotification<T, TStruct>, new()
	{
		AdsClient tcClient = (AdsClient)ads.tcClient;

		// on définit le canal pour s'abonner à des evt ADS
		uint remove = tcClient.CreateVariableHandle(removeSymbol);
		uint newMsg = tcClient.CreateVariableHandle(newMsgSymbol);
		uint oldEntry = tcClient.CreateVariableHandle(oldEntrySymbol);

		const int size = sizeof(bool);
		// abonnement à l'événement ADS
		// on n' absoin de s'abonner qu'à NewMSG
		ResultHandle resultHandle = await tcClient.AddDeviceNotificationAsync(
			newMsgSymbol,
			size,
			new NotificationSettings(AdsTransMode.OnChange, 0, 0),
			ads,
			ads.cancel);
		TNotification notification
			= new() {
				_remove = remove,
				_newMsg = newMsg,
				_oldEntry = oldEntry,
				_resultHandle = resultHandle,
			};
		tcClient.AdsNotification += notification.GetElement;
		// A bit tricky but rewriting the newMsg will trigger an event for the notification to see.
		if (tcClient.ReadAny<bool>(newMsg))
			tcClient.WriteAny(newMsg, true);

		return notification;
	}

	private void GetElement(object? sender, AdsNotificationEventArgs e)
	{
		// premiière action lorsqu'on reçoit une notification
		bool newMsg = BitConverter.ToBoolean(e.Data.Span);
		// on vérifie qu'on nous concern et qu'il s'agit bien d'un nouveau msg
		if (e.Handle != _resultHandle.Handle || !newMsg)
			return;

		Console.WriteLine("Notif msgNew");
		// UserData is our data passed in parameter
		GetElementSub(e.UserData as dynamic);
	}

	private async void GetElementSub(dynamic dynamicObject)
	{
		AdsClient? tcClient = dynamicObject.tcClient as AdsClient;
		//tcClient!.WriteAny(_acquitMsg, Utils.IsReading);
		tcClient!.WriteAny(_newMsg, false);
		// Get element of FIFO
		TStruct adsStruct = tcClient.ReadAny<TStruct>(_oldEntry);
		T entity = adsStruct.ToModel();
		await using AsyncServiceScope scope = ((IServiceProvider)dynamicObject.appServices).CreateAsyncScope();
		IServiceProvider services = scope.ServiceProvider;

		try
		{
			await AddElement(services, entity);
			if (ToDequeue)
				tcClient.WriteAny(_remove, true);
		}
		catch
		{
			// Retry
			//tcClient.WriteAny(_acquitMsg, Utils.ErrorWhileReading);
		}
	}

	protected virtual Task AddElement(IServiceProvider services, T entity)
	{
		return Task.CompletedTask;
	}
}