using System.Text;
using Core.Entities.Vision.ToDos.Models.DB.ToNotifys;
using Core.Shared.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Client;

namespace Core.Shared.Services.Background;

public class NotifyService : BackgroundService
{
	private readonly IServiceScopeFactory _factory;
	private readonly ILogger<NotifyService> _logger;

	private readonly Dictionary<int, string> _lastMsgNew = new()
	{
		{ 3, string.Empty },
		{ 4, string.Empty },
		{ 5, string.Empty },
	};

	private readonly Dictionary<int, string> _stationTopics = new()
	{
		{ 3, "topicS3" },
		{ 4, "topicS4" },
		{ 5, "topicS5" },
	};

	private readonly Dictionary<int, string> _msgNewTopics = new()
	{
		{ 3, "TopicMsgNewS3" },
		{ 4, "TopicMsgNewS4" },
		{ 5, "TopicMsgNewS5" },
	};

	public NotifyService(
		ILogger<NotifyService> logger,
		IServiceScopeFactory factory)
	{
		_logger = logger;
		_factory = factory;
	}

	protected override async Task ExecuteAsync(CancellationToken stoppingToken)
	{
		GetLastMessageFromMsgNewTopic(3);
		GetLastMessageFromMsgNewTopic(4);
		GetLastMessageFromMsgNewTopic(5);

		while (!stoppingToken.IsCancellationRequested)
		{
			await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
			IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();

			try
			{
				foreach (KeyValuePair<int, string> entry in _lastMsgNew)
				{
					if (entry.Value != "2")
					{
						ToNotify? notification = await anodeUOW.ToNotify.GetBy([notify => notify.Station == entry.Key]);
						if (notification is null)
							continue;

						string msgNewTopic = _msgNewTopics.GetValueOrDefault(notification.Station, string.Empty);
						await SendNotificationInfo(notification);
						await SendMessageToEGABrokerAsync("2", msgNewTopic);

						await anodeUOW.ToNotify.Remove(notification.ID);
						anodeUOW.Commit();
						await anodeUOW.CommitTransaction();
					}
				}
			}
			catch (Exception ex)
			{
				_logger.LogError(
					"Failed to execute NotifyService with exception message {message}",
					ex.Message);
			}

			await Task.Delay(2000, stoppingToken);
		}
	}

	private async void GetLastMessageFromMsgNewTopic(int stationID)
	{
		MqttFactory mqttFactory = new();
		IMqttClient client = mqttFactory.CreateMqttClient();

		MqttClientOptions options = new MqttClientOptionsBuilder()
			.WithClientId(Guid.NewGuid().ToString())
			.WithTcpServer("localhost", 1883)
			.WithCleanSession()
			.Build();

		try
		{
			await client.ConnectAsync(options);
			string topic = _msgNewTopics.GetValueOrDefault(stationID, string.Empty);
			client.ApplicationMessageReceivedAsync += e => {
				if (e.ApplicationMessage.Topic == topic)
					_lastMsgNew[stationID] = Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment);

				return Task.CompletedTask;
			};

			await client.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic(topic).Build());
		}
		catch (Exception ex)
		{
			_logger.LogError("Failed to wait for response from MQTT topic with exception message {message}.", ex.Message);
		}
	}

	private async Task SendNotificationInfo(ToNotify notification)
	{
		string messageBody = $"{notification.SynchronisationKey};"
			+ $"{notification.SerialNumber};"
			+ $"{notification.Timestamp.ToString()};"
			+ $"{notification.Path}";
		string topic = _stationTopics.GetValueOrDefault(notification.Station, string.Empty);
		await SendMessageToEGABrokerAsync(messageBody, topic);
	}

	private async Task SendMessageToEGABrokerAsync(string messageBody, string topic)
	{
		MqttFactory mqttFactory = new();
		IMqttClient client = mqttFactory.CreateMqttClient();

		MqttClientOptions options = new MqttClientOptionsBuilder()
			.WithClientId(Guid.NewGuid().ToString())
			.WithTcpServer("localhost", 1883)
			.WithCleanSession()
			.Build();

		try
		{
			await client.ConnectAsync(options);

			Console.WriteLine("Connected to the broker");

			MqttApplicationMessage message = new MqttApplicationMessageBuilder()
				.WithTopic(topic)
				.WithPayload(messageBody)
				.Build();

			await client.PublishAsync(message);
		}
		catch (Exception ex)
		{
			_logger.LogError("Failed to send MQTT message with exception message {message}.", ex.Message);
		}
		finally
		{
			await client.DisconnectAsync();
		}
	}
}