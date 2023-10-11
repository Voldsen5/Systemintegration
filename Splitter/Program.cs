using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Splitter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MessageQueue messageQueueIn = new MessageQueue(@".\Private$\AirportCheckInOutput");
            MessageQueue messageQueuePassenger = new MessageQueue(@".\Private$\passenger");
            MessageQueue messageQueueluggage = new MessageQueue(@".\Private$\luggage");



            
            Message receivedMessage = messageQueuePassenger.Receive();
            Console.WriteLine(receivedMessage.Body);
            Console.ReadLine();
        }
    }
}
