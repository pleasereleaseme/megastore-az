using MegaStore.Helper;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.WorkerService;
using Microsoft.Extensions.DependencyInjection;
using NATS.Client;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MegaStore.SaveSaleHandler
{
    class Program
    {
        private static ManualResetEvent _ResetEvent = new ManualResetEvent(false);
        private static TelemetryClient _telemetryClient;

        private const string QUEUE_GROUP = "save-sale-handler";

        static void Main()
        {
            IServiceCollection services = new ServiceCollection();
/*            var aiOpts = new ApplicationInsightsServiceOptions
            {
                EnableHeartbeat = true,
                ConnectionString = Env.AppInsightsInstrumentationKey
            };*/
            services.AddApplicationInsightsTelemetryWorkerService(Env.AppInsightsInstrumentationKey);
            services.AddApplicationInsightsKubernetesEnricher();
            services.AddSingleton<ITelemetryInitializer, CloudRoleTelemetryInitializer>();

            IServiceProvider serviceProvider = services.BuildServiceProvider();

            _telemetryClient = serviceProvider.GetRequiredService<TelemetryClient>();
            
            try
            {
                var connectingMsg = $"Connecting to message queue url: {Env.MessageQueueUrl}";
                _telemetryClient.TrackTrace(connectingMsg);
                Console.WriteLine(connectingMsg);
                using (var connection = MessageQueue.CreateConnection())
                {
                    var subscription = connection.SubscribeAsync(SaleCreatedEvent.MessageSubject, QUEUE_GROUP);
                    subscription.MessageHandler += SaveSale;
                    subscription.Start();

                    var listeningMsg = $"Listening on subject: {SaleCreatedEvent.MessageSubject}, queue: {QUEUE_GROUP}";
                    _telemetryClient.TrackTrace(listeningMsg);
                    Console.WriteLine(listeningMsg);

                    _ResetEvent.WaitOne();
                    connection.Close();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex} in Main");
                var method = new Dictionary<string, string> { { "Method", "Main" } };
                _telemetryClient.TrackException(ex, method);
            }

        }
        private static void SaveSale(object sender, MsgHandlerEventArgs e)
        {
            try
            {
                var receivedMsg = $"Received message, subject: {e.Message.Subject}";
                _telemetryClient.TrackTrace(receivedMsg);
                Console.WriteLine(receivedMsg);
                var eventMessage = MessageHelper.FromData<SaleCreatedEvent>(e.Message.Data);

                var savingMsg = $"Saving new sale, created at: {eventMessage.CreatedAt}; event ID: {eventMessage.CorrelationId}, on host {Environment.MachineName}";
                _telemetryClient.TrackTrace(savingMsg);
                Console.WriteLine(savingMsg);
                var sale = eventMessage.Sale;

                using (var db = new MegaStoreContext())
                {
                    db.Sale.Add(sale);
                    db.SaveChanges();
                }

                var savedMsg = $"Sale saved. Sale ID: {sale.SaleID}; event ID: {eventMessage.CorrelationId}";
                _telemetryClient.TrackTrace(savedMsg);
                Console.WriteLine(savedMsg);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex} in SaveSale");
                var method = new Dictionary<string, string> { { "Method", "SaveSale" } };
                _telemetryClient.TrackException(ex, method);
            }
        }
    }
}
