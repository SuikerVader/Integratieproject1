using System;
using System.Text;
using System.Threading.Tasks;
using Integratieproject1.BL.Managers;
using Integratieproject1.Domain.Ideations;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Surveys;
using Microsoft.AspNetCore.Http;
using MQTTnet;
using MQTTnet.Client;

namespace Integratieproject1.DAL
{
    public class MqttClient
    {
        private static IoTManager _ioTManager = new IoTManager();

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
                    string[] split = payload.Split('-');
                    foreach (var VARIABLE in split)
                    {
                        Console.WriteLine("********" + VARIABLE);
                    }

                    Console.WriteLine("split length :" + split.Length);
                    IoTSetup setup = _ioTManager.GetIoT(split[0]);
                    Console.WriteLine(setup.Idea.Title);
                    Console.WriteLine("split length :" + split.Length);
                    
                    if (split.Length == 2)
                    {
                        Console.WriteLine("simple voting");
                        if (Int32.TryParse(split[1], out int i))
                        {
                            Console.WriteLine(i + " votes");
                            _ioTManager.RegisterSimpleVote(setup.Idea.IdeaId, i);
                        }
                    }
                    if (split.Length > 2)
                    {
                        Console.WriteLine("voting on question nr." + setup.Question.QuestionId);
                        for (int i = 1; i < split.Length; i++)
                        {
                            if (Int32.TryParse(split[i], out int u))
                            {
                                Console.WriteLine("answer "+ i + " times " + u);
                                _ioTManager.RegisterComplexVote(setup.Question.QuestionId, i,u);
                            }
                        }
                    }
                }
                Console.WriteLine("done voting");
            };

            mqttClient.Connected += async (s, e) =>
            {
                Console.WriteLine("### CONNECTED WITH SERVER ###");

                // Subscribe to a topic
                await mqttClient.SubscribeAsync(new TopicFilterBuilder().WithTopic("ms32").Build());

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