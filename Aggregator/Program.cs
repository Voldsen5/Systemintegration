using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Aggregator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MessageQueue messageQueueLuggageSort = null;
            if (MessageQueue.Exists(@".\Private$\AirportLuggageSort"))
            {
                messageQueueLuggageSort = new MessageQueue(@".\Private$\AirportLuggageSort");
                messageQueueLuggageSort.Label = "Sorted Luggage Queue";
            }
            else
            {
                // Create the Queue
                MessageQueue.Create(@".\Private$\AirportLuggageSort");
                messageQueueLuggageSort = new MessageQueue(@".\Private$\AirportLuggageSort");
                messageQueueLuggageSort.Label = "Sorted Luggage Queue";
            }

            MessageQueue messageQueuePassenger = null;
            if (MessageQueue.Exists(@".\Private$\AirportPassenger"))
            {
                messageQueuePassenger = new MessageQueue(@".\Private$\AirportPassenger");
                messageQueuePassenger.Label = "Passenger Queue";
            }
            else
            {
                // Create the Queue
                MessageQueue.Create(@".\Private$\AirportPassenger");
                messageQueuePassenger = new MessageQueue(@".\Private$\AirportPassenger");
                messageQueuePassenger.Label = "Passenger Queue";
            }

            MessageQueue mQOut = new MessageQueue(@".\private$\aggregatorOut");

            NewAggregator ag1 = new NewAggregator(messageQueueLuggageSort, messageQueuePassenger, mQOut);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();
        }
    }


}

