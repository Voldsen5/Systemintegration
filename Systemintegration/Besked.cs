using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace Systemintegration {


    // Body for the message
    public class MessageBody
    {
        public string Airline { get; set; }
        public DateTime ScheduledTime { get; set; }
        public string FlightNumber { get; set; }
        public string Destination { get; set; }
        public bool CheckInAvailable { get; set; }
    }

    // Complete message structure
    public class Message
    {
        public string Sender { get; set; }
        public DateTime Timestamp { get; set; }
        public MessageBody Body { get; set; }
    }
}
