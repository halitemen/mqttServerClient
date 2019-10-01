using Broker.MqttConsumer;
using System;

namespace mqttServerTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var a = new MQTTConsumer();
            
            a.DataReceived += A_DataReceived;
            a.StartConsume();
            Console.ReadLine();
        }

        private static void A_DataReceived(object sender, byte[] e)
        {
            Console.WriteLine(sender);
            Console.ReadKey();
        }
    }
}
