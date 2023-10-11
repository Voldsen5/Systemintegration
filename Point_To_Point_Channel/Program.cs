using System;
using System.Messaging;

namespace Point_To_Point_Channel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Air_Trafic air_Trafic = new Air_Trafic();
            Air_Info air_Info = new Air_Info();

            Console.WriteLine("Hvilket flyselskab?: ");
            string messageText = Console.ReadLine();


            // Send a message
            Message message = new Message();
            message.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
            message.Body = messageText;
            message.Label = DateTime.Now + "";
            

            air_Trafic.Send("testqueue", message);
            Console.WriteLine("Message sent: " + messageText);

            // Receive and display the message
            Message receivedMessage = air_Info.Receive("testqueue");
            Console.WriteLine("Flyselskab: " + receivedMessage.Body + " - ETA: " + receivedMessage.Label);
            

            Console.ReadLine();
        }
    }
}

