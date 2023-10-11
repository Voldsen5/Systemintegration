using System;
using System.Collections.Generic;
using System.IO;
using System.Messaging;
using System.Xml.Serialization;

namespace Opgave2
{
    internal class Program
    {

        static void Main(string[] args)
        {

            
            ETAMessage msg_1 = new ETAMessage("Klm", "15:11", "KLM911","Oslo");
            ETAMessage message = new ETAMessage("united", "14:00", "KLM112", "Copenhagen");

            // Serialize the object to XML

            string xmlMessage = Msg_XML_Serializer(message);

            // Create MessageQueue´s

            MessageQueue messageQueue = null;

            messageQueue = message.Open_MQ();

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

            if (receivedETAMessage.GetType == ) { }



            List<ETAMessage> list = new List<ETAMessage>();
            list.Add(message);
            list.Add(msg_1);




        }
        public static void AirlineQueueChecker(string MQ_Name)
        {
            String y = MQ_Name.ToUpper().Trim();

            if (!MessageQueue.Exists(@".\Private$\" + y))
            {
                MessageQueue.Create(@".\Private$\" + y);
            }
        }

        public static XmlSerializer serializer = new XmlSerializer(typeof(ETAMessage));

        public static String Msg_XML_Serializer(ETAMessage message)
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

        public MessageQueue Open_MQ()
        {
            string x = this.Airline.Trim().ToUpper();
            return new MessageQueue(@".\Private$\" + x);
        }
    }
}


