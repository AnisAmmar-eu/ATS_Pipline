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
	private readonly uint _newMsg;
	private readonly uint _oldEntry;
	private readonly ILogger _logger = null!;
	protected bool ToDequeue = true;

	public BaseNotification(
		uint newMsg,
		uint oldEntry,
		ILogger logger)
	{
		_newMsg = newMsg;
		_oldEntry = oldEntry;
		_logger = logger;
	}

	protected BaseNotification()
	{
	}

	public async Task GetElement(AdsClient tcClient, IServiceProvider appServices)
	{
		// Get element of FIFO
		TStruct adsStruct = tcClient.ReadAny<TStruct>(_oldEntry);
		T entity = adsStruct.ToModel();

		await using AsyncServiceScope scope = appServices.CreateAsyncScope();
		IServiceProvider services = scope.ServiceProvider;
		try
		{
			await AddElement(services, entity);
		}
		catch (Exception e)
		{
			_logger.LogError($"Error while dequeuing a {typeof(T).Name}: {e}");
		}

		tcClient.WriteAny(_newMsg, false);
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