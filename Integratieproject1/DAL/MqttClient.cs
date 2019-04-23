using System;
using System.Text;
using System.Threading.Tasks;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Ideations;
using MQTTnet;
using MQTTnet.Client;

namespace Integratieproject1.DAL
{
    public class MqttClient
    {
        private static IdeationsManager ideationsManager = new IdeationsManager();
        private static IoTManager ioTManager = new IoTManager();
    
        //("m24.cloudmqtt.com", 15459, "jdvewwvn", "9S3vDhi54u1", "client1")
        public static async Task MqttClientTask(string connection, int port, string username, string pwd,
            string clientId = "client")
        {
            var factory = new MqttFactory();
            var mqttClient = factory.CreateMqttClient();
            var options = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithTcpServer(connection, port)
                .WithCredentials(username, pwd)
                .Build();
            await mqttClient.ConnectAsync(options);
            Console.WriteLine("**connected**");
            await mqttClient.SubscribeAsync("ms16/#");

            mqttClient.ApplicationMessageReceived += (s, e) =>
            {
                Console.WriteLine("********************Received");
                string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine(payload);
                if (e.ApplicationMessage.Topic.Equals("ms16"))
                {
                    if (Int32.TryParse(payload, out int i))
                    {
                        ideationsManager.CreateVote(i, VoteType.IOT);
                        Console.WriteLine("simple vote " + i);
                    }
                }
                else
                {
                    string[] split = payload.Split('/');
                    foreach (var VARIABLE in split)
                    {
                        Console.WriteLine("********" + VARIABLE);
                    }
                    if (Int32.TryParse(split[0], out int i) && Int32.TryParse(split[1], out int j))
                    {
                        Console.WriteLine("complex vote " + i + j);
                        ioTManager.RegisterComplexVote(i,j);
                    }
                }
            };

            mqttClient.Connected += async (s, e) =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                Console.WriteLine("subbed");
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("ms16").Build());

                Console.WriteLine("### SUBSCRIBED ###");
            };

            mqttClient.Disconnected += async (s, e) =>
            {
                Console.WriteLine("### DISCONNECTED FROM SERVER ###");
                await Task.Delay(TimeSpan.FromSeconds(5));

                try
                {
                    await mqttClient.ConnectAsync(options);
                }
                catch
                {
                    Console.WriteLine("### RECONNECTING FAILED ###");
                }
            };
            
             #region Code to publish a message //commented
             /*
            var message = new MqttApplicationMessageBuilder()
                .WithTopic("ms16")
                .WithPayload("payloadedIntegratie"+DateTime.Now.ToString())
                .WithExactlyOnceQoS()
                .WithRetainFlag()
                .Build();
            await mqttClient.PublishAsync(message);
            */
            #endregion
        }
    }
}