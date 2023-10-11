using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Content_Filter
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MessageQueue mqIn = new MessageQueue(@".\Private$\AirportCheckInOutput");
            MessageQueue mqOut = new MessageQueue(@".\Private$\ContentFilter");

            Content_Filter content_Filter = new Content_Filter(mqIn, mqOut);

            Console.ReadLine();
        }
    }
}
