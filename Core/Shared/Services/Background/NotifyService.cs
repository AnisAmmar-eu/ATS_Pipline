using Core.Entities.IOT.IOTDevices.Models.DB.BackgroundServices;
using Core.Entities.Vision.ToDos.Models.DB.ToNotifys;
using Core.Entities.Vision.ToDos.Repositories.ToNotifys;
using Core.Shared.Configuration;
using Core.Shared.Dictionaries;
using Core.Shared.Models.TwinCat;
using Core.Shared.UnitOfWork.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Shared.Services.Background
{
	public class NotifyService : BackgroundService
	{
		private readonly IServiceScopeFactory _factory;
		private readonly ILogger<NotifyService> _logger;
		private readonly IConfiguration _configuration;

		public NotifyService(
			ILogger<NotifyService> logger,
			IServiceScopeFactory factory,
			IConfiguration configuration
			)
		{
			_logger = logger;
			_factory = factory;
			_configuration = configuration;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await using AsyncServiceScope asyncScope = _factory.CreateAsyncScope();
				IToNotifyRepository toNotifyRepository = asyncScope.ServiceProvider.GetRequiredService<IToNotifyRepository>();
				IAnodeUOW anodeUOW = asyncScope.ServiceProvider.GetRequiredService<IAnodeUOW>();

				try
				{
					// Vérifier si la table ToNotify est vide ou non
					bool hasNotifications = await anodeUOW.ToNotify.AnyAsync();

					if (hasNotifications)
					{
						// Si msgNew est différent de  2
						// récupérer une notification et envoyer les infos
						if (msgNew != 2)
						{
							// Récup  notif
							var notification = await anodeUOW.ToNotify.FirstOrDefaultAsync();

							SendNotificationInfo(notification);

							msgNew = 2;
						}
					}
					anodeUOW.Commit();
					await anodeUOW.CommitTransaction();
				}
				catch (Exception ex)
				{
					_logger.LogError(
						"Failed to execute NotifyService with exception message {message}. Good luck next round!",
						ex.Message);
				}

				// Attente apres la prochaine itération
				await Task.Delay(2000, stoppingToken);
			
			}
		}


		private void SendNotificationInfo(ToNotify notification)
		{
			// chaine d'info
			string messageBody = $"{notification.SynchronisationKey};{notification.Timestamp};{notification.Path}";

			// Choix du topic  en fct de la station
			string topic;
			switch (notification.Station)
			{
				case "3":
					topic = "topicS3";
					break;
				case "4":
					topic = "topicS4";
					break;
				case "5":
					topic = "topicS5";
					break;
			}
			SendMessageToEGABrokerAsync(messageBody, topic).Wait(); 

		}

		private async Task SendMessageToEGABrokerAsync(string messageBody, string topic)
		{
			// Configurer les options de connexion au broker MQTT
			var mqttFactory = new MqttFactory();
			var mqttClient = mqttFactory.CreateMqttClient();

			var options = new MqttClientOptionsBuilder()
				.WithTcpServer("localhost", 1883) 
				.Build();

			// Connecter le client MQTT au broker
			await mqttClient.ConnectAsync(options);

			// Publier le message sur le topic spécifique
			var message = new MqttApplicationMessageBuilder()
				.WithTopic(topic) // Utilisation du topic spécifié
				.WithPayload(messageBody)
				.WithExactlyOnceQoS()
				.Build();

			await mqttClient.PublishAsync(message);

			// Déconnecter le client MQTT du broker
			await mqttClient.DisconnectAsync();
		}
	}
}
