using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Receiving;
using MQTTnet.Server;

namespace Broker.MqttConsumer
{
    public class MQTTConsumer
    {
        public MQTTConsumer()
        {
            optionsBuilder = new MqttServerOptionsBuilder()
                                     .WithClientCertificate()
                                     .WithConnectionBacklog(100)
                                     .WithDefaultEndpointPort(1883)
                                     .Build();
            Server = new MqttFactory().CreateMqttServer();
        }


        private IMqttServer Server;
        private IMqttServerOptions optionsBuilder;

        public event EventHandler<byte[]> DataReceived;
        public event EventHandler DeviceConnected;
        public event EventHandler DeviceDisconnected;


        public void StartConsume()
        {

            Server.StartAsync(optionsBuilder);
            
            Server.ClientConnectedHandler = new MqttServerClientConnectedHandlerDelegate(e =>
            {
                Console.WriteLine("Client connected " + e.ClientId);
            });
            Server.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(e =>
            {
                //Console.WriteLine(e.ClientId + " " + e.ApplicationMessage.Topic);
                //Console.WriteLine();
                //Console.WriteLine(e.ClientId + " " + e.ApplicationMessage.ConvertPayloadToString());
                //Console.WriteLine();
                //Console.WriteLine(Decode(e.ApplicationMessage.ConvertPayloadToString()));
                DataReceived?.Invoke(e.ApplicationMessage.ConvertPayloadToString(), null);
            });
        }

        public void StopConsume()
        {
            Server.StopAsync();
            Server.ClientDisconnectedHandler = new MqttServerClientDisconnectedHandlerDelegate(e =>
            {
                Console.WriteLine("Client Disconnect" + e.ClientId);
            });
        }

        public static string Decode(string base64Text)
        {
            string base64Decoded;
            byte[] data = Convert.FromBase64String(base64Text);
            base64Decoded = ASCIIEncoding.ASCII.GetString(data);
            return base64Decoded;
        }
    }
}
