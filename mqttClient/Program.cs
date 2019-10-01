using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace MQTT_CLIENT
{
     class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Client console ");
            ConnectAsync();
            while (true)
            {
                string message = Console.ReadLine();

                PublishAsync("sayhello", message);
                Console.Read();
            }
           
        }

        static IManagedMqttClient client = new MqttFactory().CreateManagedMqttClient();
        public static async Task ConnectAsync()
        {
            string clientId = Guid.NewGuid().ToString();
            string mqttURI = "localhost";
            string mqttUser = "test";
            string mqttPassword = "test";
            int mqttPort = 1883;

            var messageBuilder = new MqttClientOptionsBuilder()
                .WithClientId(clientId)
                .WithCredentials(mqttUser, mqttPassword)
                .WithTcpServer(mqttURI, mqttPort)
                .WithCleanSession();

            var options = messageBuilder
                .Build();

            var managedOptions = new ManagedMqttClientOptionsBuilder()
                .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
                .WithClientOptions(options)
                .Build();

            await client.StartAsync(managedOptions);

            client.UseConnectedHandler(e => { Console.WriteLine("Connected successfully with MQTT Brokers."); });

            client.UseDisconnectedHandler(e => { Console.WriteLine("Disconnected from MQTT Brokers."); });

        }

        private static void Handler(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                string topic = e.ApplicationMessage.Topic;
                if (string.IsNullOrWhiteSpace(topic) == false)
                {
                    string payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                    Console.WriteLine($"Topic: {topic}. Message Received: {payload}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message, ex);
            }
        }

        public static async Task PublishAsync(string topic, string payload, bool retainFlag = true, int qos = 1) =>
                await client.PublishAsync(new MqttApplicationMessageBuilder()
                    .WithTopic(topic)
                    .WithPayload(payload)
                    .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
                    .WithRetainFlag(retainFlag)
                    .Build());

        public static string Base64Encode(string plainText)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(plainText));
        }
    }
}


