using Core.Shared.Models.DB.Kernel.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TwinCAT.Ads;

namespace Core.Shared.Services.Notifications;

/// <summary>
/// Generic service responsible for dequeuing data from the automaton and add it to the SQL Database.
/// Its <see cref="AddElement"/> function should be override to allow for custom side effect and choosing in which table to add it.
/// </summary>
/// <typeparam name="T">Type to which to struct is converted before being given to the <see cref="AddElement"/> function.</typeparam>
/// <typeparam name="TStruct">Struct which corresponds to the one in the automaton</typeparam>
public class BaseNotification<T, TStruct>
	where TStruct : struct, IBaseADS<T>
{
	private uint _newMsg;
	private uint _acquitMsg;
	private uint _oldEntry;
	private uint _remove;
	private ResultHandle _resultHandle = null!;
	private ILogger _logger = null!;
	protected bool ToDequeue = true;

	public BaseNotification(
		ResultHandle resultHandle,
		uint remove,
		uint newMsg,
		uint oldEntry,
		uint acquitMsg,
		ILogger logger)
	{
		_resultHandle = resultHandle;
		_remove = remove;
		_newMsg = newMsg;
		_acquitMsg = acquitMsg;
		_oldEntry = oldEntry;
		_logger = logger;
	}

	protected BaseNotification()
	{
	}

	/// <summary>
	/// Creates the Notification which automatically starts.
	/// </summary>
	/// <param name="ads"></param>
	/// <param name="removeSymbol"></param>
	/// <param name="newMsgSymbol"></param>
	/// <param name="acquitMsgSymbol"></param>
	/// <param name="oldEntrySymbol"></param>
	/// <param name="logger"></param>
	/// <typeparam name="TNotification"></typeparam>
	/// <returns></returns>
	protected static async Task<BaseNotification<T, TStruct>> CreateSub<TNotification>(
		dynamic ads,
		string removeSymbol,
		string newMsgSymbol,
		string acquitMsgSymbol,
		string oldEntrySymbol,
		ILogger logger)
		where TNotification : BaseNotification<T, TStruct>, new()
	{
		AdsClient tcClient = (AdsClient)ads.tcClient;

		// on définit le canal pour s'abonner à des evt ADS
		uint remove = tcClient.CreateVariableHandle(removeSymbol);
		uint newMsg = tcClient.CreateVariableHandle(newMsgSymbol);
		uint acquitMsg = tcClient.CreateVariableHandle(acquitMsgSymbol);
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
				_acquitMsg = acquitMsg,
				_oldEntry = oldEntry,
				_resultHandle = resultHandle,
				_logger = logger,
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
		// on vérifie qu'on nous concerne et qu'il s'agit bien d'un nouveau msg
		if (e.Handle != _resultHandle.Handle || !newMsg)
			return;

		Console.WriteLine("Notif msgNew");
		// UserData is our data passed in parameter
		GetElementSub(e.UserData as dynamic);
	}

	private async void GetElementSub(dynamic dynamicObject)
	{
		AdsClient tcClient = dynamicObject.tcClient as AdsClient
			?? throw new InvalidOperationException("tcClient in ads dynamicObject is null");
		tcClient.WriteAny(_acquitMsg, true);
		tcClient.WriteAny(_newMsg, false);

		// Get element of FIFO
		TStruct adsStruct = tcClient.ReadAny<TStruct>(_oldEntry);
		T entity = adsStruct.ToModel();
		await using AsyncServiceScope scope = ((IServiceProvider)dynamicObject.appServices).CreateAsyncScope();
		IServiceProvider services = scope.ServiceProvider;

		try
		{
			await AddElement(services, entity);
		}
		catch (Exception e)
		{
			_logger.LogError($"Error while dequeuing a {typeof(T).Name}: {e}");
		}

		tcClient.WriteAny(_remove, true);
		tcClient.WriteAny(_acquitMsg, false);
	}

	/// <summary>
	/// Should be override to save the retrieved data.
	/// </summary>
	/// <param name="services"></param>
	/// <param name="entity"></param>
	/// <returns></returns>
	protected virtual Task AddElement(IServiceProvider services, T entity)
	{
		return Task.CompletedTask;
	}
}