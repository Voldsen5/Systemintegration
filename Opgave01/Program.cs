using System;
using System.Collections.Generic;
using System.Messaging;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;

namespace Opgave01
{
    internal class Program
    {

        static void Main(string[] args)
        {
            ETAMessage message1 = new ETAMessage("united", "14:00", "KLM112", "Copenhagen");

            Message message = new Message();

            message.Body = message1;

            message.TimeToBeReceived = TimeSpan(0, 0, 30);

            
           

            // Serialize the object to XML

            string xmlMessage = Msg_XML_Serializer(message);

            // Create MessageQueue´s

            MessageQueue messageQueue = new MessageQueue(@".\Private$\KLM");

            // Send the XML message

            messageQueue.Send(xmlMessage);

            Console.WriteLine("Message sent: " + xmlMessage);

            Console.ReadLine();

            // Receive and Deserialize the XML message

            Message receivedMessage = messageQueue.Receive();
            string receivedXmlMessage = receivedMessage.Body.ToString();
            ETAMessage receivedETAMessage;

            using (StringReader stringReader = new StringReader(receivedXmlMessage))
            {
                receivedETAMessage = (ETAMessage)serializer.Deserialize(stringReader);
            }

            Console.WriteLine("MQ NAVN: " + messageQueue.QueueName + " - Message received: ETA - " + receivedETAMessage.ETA + ", PlaneID - " + receivedETAMessage.PlaneID + ", DepartureFrom - " + receivedETAMessage.DepartureFrom);

            Console.ReadLine();

        }
        public static void AirlineQueueChecker(string MQ_Name)
        {
            String y = MQ_Name.ToUpper().Trim();

            if (!MessageQueue.Exists(@".\Private$\" + y))
            {
                MessageQueue.Create(@".\Private$\" + y);
            }
        }

        public static XmlSerializer serializer = new XmlSerializer(typeof(Message));

        public static String Msg_XML_Serializer(Message message)
        {
            StringWriter stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, message);
            string xmlMessage = stringWriter.ToString();
            return xmlMessage;
        }

       




    }

    [Serializable]
    public class ETAMessage
    {
        public String Airline { get; set; }
        public String ETA { get; set; }
        public String PlaneID { get; set; }
        public String DepartureFrom { get; set; }

        public ETAMessage()
        {

        }

        public ETAMessage(string airline, string eTA, string planeID, string departureFrom)
        {
            Airline = airline;
            ETA = eTA;
            PlaneID = planeID;
            DepartureFrom = departureFrom;
        }
    }
}
