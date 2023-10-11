using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Messaging;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Splitter
{
    internal class Splitter
    {
        protected MessageQueue inQueue;


        public Splitter(MessageQueue inQueue)
        {
            this.inQueue = inQueue;


            inQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnMessage);
            inQueue.BeginReceive();
            string label = inQueue.Label;
        }

        protected void OnMessage(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)source;
            Message message = mq.EndReceive(asyncResult.AsyncResult);
                string label = message.Label;

            XmlDocument xml = new XmlDocument();
                string XMLDocument = null;
                Console.WriteLine(label);
                Stream body = message.BodyStream;
                StreamReader reader = new StreamReader(body);
                XMLDocument = reader.ReadToEnd().ToString();
                xml.LoadXml(XMLDocument);

            XmlNodeList passengerNodeList = xml.SelectNodes("/FlightDetailsInfoResponse/passenger");
            XmlNodeList luggageNode = xml.SelectNodes("/FlightDetailsInfoResponse/Luggage");

            foreach (XmlNode passengerNode in passengerNodeList)
            {
                MessageQueue messageQueuePassenger = new MessageQueue();
                messageQueuePassenger.se
            }
        }

        //protected void OnMessage(Object source, ReceiveCompletedEventArgs asyncResult)
        //{
        //    MessageQueue mq = (MessageQueue)source;
        //    mq.Formatter = new ActiveXMessageFormatter();
        //    Message message = mq.EndReceive(asyncResult.AsyncResult);

        //    XmlDocument doc = new XmlDocument();
        //    doc.LoadXml((String)message.Body);

        //    XmlNodeList xmlNodeList;
        //    XmlElement root = doc.DocumentElement;

        //    XmlNode firstName = root.SelectSingleNode("FirstName");
        //    XmlNode lastName = root.SelectSingleNode("LastName");
        //    XmlNode reservationNumber = root.SelectSingleNode("ReservationNumber");
        //    XmlElement passengerId = doc.CreateElement("PassengerId");
        //    passengerId.InnerText = reservationNumber.InnerXml;

        //    xmlNodeList = root.SelectNodes("/FlightDetailsInfoResponse/Passenger");

        //    foreach (XmlNode node in xmlNodeList)
        //    {
        //        XmlDocument passengerDoc = new XmlDocument();
        //        passengerDoc.LoadXml("Passenger");
        //        XmlElement passenger = passengerDoc.DocumentElement;

        //        passenger.AppendChild(passengerDoc.ImportNode(firstName,true));
        //        passenger.AppendChild(passengerDoc.ImportNode(lastName, true));
        //        passenger.AppendChild(passengerDoc.ImportNode(reservationNumber, true));

        //        for (int i = 0; i < node.ChildNodes.Count; i++) 
        //        {
        //            passenger.AppendChild(passengerDoc.ImportNode(node.ChildNodes[i], true));   
        //        }
        //        passengerQueue.Send(passenger.OuterXml);
        //    }

        //    mq.BeginReceive();
        //}
    }
}
