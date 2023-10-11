using System;
using System.Text;
using System.Linq;
using System.Xml.Linq;
using System.Messaging;

namespace MYFirstMSMQ
{
    class Program
    {
        static void Main(string[] args)
        {

            MessageQueue messageQueue = null;
            if (MessageQueue.Exists(@".\Private$\AirportCheckInOutput"))
            {
                messageQueue = new MessageQueue(@".\Private$\AirportCheckInOutput");
                messageQueue.Label = "CheckIn Queue";
            }
            else
            {
                // Create the Queue
                MessageQueue.Create(@".\Private$\AirportCheckInOutput");
                messageQueue = new MessageQueue(@".\Private$\AirportCheckInOutput");
                messageQueue.Label = "Newly Created Queue";
            }

            XElement CheckInFile = XElement.Load(@"CheckedInPassenger.xml");
            Console.WriteLine(CheckInFile);
            string AirlineCompany = "SAS";

            messageQueue.Send(CheckInFile, AirlineCompany);
 
        }
    }
}

