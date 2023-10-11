using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Messaging;
using System.Text.Json;

namespace Systemintegration
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Create a sample message
            Message message = new Message
            {
                Sender = "Sample Airline",
                Timestamp = DateTime.UtcNow,
                Body = new MessageBody
                {
                    Airline = "Sample Airlines",
                    ScheduledTime = DateTime.UtcNow.AddHours(2),
                    FlightNumber = "SA123",
                    Destination = "Bluff City",
                    CheckInAvailable = true
                }
            };

            MessageQueue messageQueue = null;
            messageQueue = new MessageQueue(@".\Private$\TestQueue");
            if (!MessageQueue.Exists(@".\Private$\TestQueue"))
            {
                MessageQueue.Create(@".\Private$\TestQueue");
            }

            messageQueue.Send(message);
            Console.ReadLine();


            string jsonString = JsonSerializer.Serialize(messageQueue.Receive().Body, new JsonSerializerOptions { WriteIndented = true });
            Console.WriteLine(jsonString);
            Console.ReadLine();

            // Serialize the message to JSON

            
        }

    } 
}
