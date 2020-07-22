using System;

namespace MqttServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var mqttConsumer = new MQTTConsumer();
            //mqttConsumer.DataReceived += MqttConsumer_DataReceived;
            mqttConsumer.StartConsume();
            Console.ReadLine();
        }

        private static void MqttConsumer_DataReceived(object sender, byte[] e)
        {
            //Console.WriteLine(sender);
        }
    }
}
