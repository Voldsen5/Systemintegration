using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Publish_Subcribe_Channel
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Air_Trafic air_Trafic = new Air_Trafic();
            Air_Info air_Info = new Air_Info();
            Airline KLM = new Airline("KLM");

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

            air_Info.Redirect(receivedMessage);

            Console.WriteLine("Fra AIO til airline");

            Console.ReadLine();

            Message rM = KLM.Receive();
            Console.WriteLine("Flyselskab: " + rM.Body + " - ETA: " + rM.Label);
        }
    }
}
