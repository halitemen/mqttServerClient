using Broker.MqttConsumer;
using System;

namespace mqttServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var mqttConsumer = new MQTTConsumer();

            mqttConsumer.DataReceived += PayloadDataReceived;
            mqttConsumer.StartConsume();
            Console.ReadLine();
        }

        private static void PayloadDataReceived(object sender, byte[] e)
        {
            Console.WriteLine(sender);
        }
    }
}
