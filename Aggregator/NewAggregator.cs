using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Aggregator
{
    public class NewAggregator
    {
        protected MessageQueue passengerQueue;
        protected MessageQueue luggageQueue;
        protected MessageQueue messageOutQueue;

        private XmlDocument fullMessage = new XmlDocument();

        private int luggageCount = 0;
        private int passengerCount = 0;

        private bool passengerReceived = false;
        private bool luggage1Received = false;
        private bool luggage2Received = false;


        public NewAggregator(MessageQueue passengerQueue, MessageQueue luggageQueue, MessageQueue messageOutQueue)
        {
            this.passengerQueue = passengerQueue;
            this.luggageQueue = luggageQueue;
            this.messageOutQueue = messageOutQueue;

            fullMessage = new XmlDocument();
            fullMessage.AppendChild(fullMessage.CreateElement("Root"));

            passengerQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnPassengerMessage);
            passengerQueue.BeginReceive();

            luggageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnLuggageMessage);
            luggageQueue.BeginReceive();
        }

        private void OnPassengerMessage(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            try
            {
                MessageQueue mq = (MessageQueue)source;
                Message message = mq.EndReceive(asyncResult.AsyncResult);

                XmlDocument passengerXml = new XmlDocument();
                passengerXml.Load(message.BodyStream);

                XmlNode passengerNode = fullMessage.ImportNode(passengerXml.DocumentElement, true);
                fullMessage.DocumentElement.AppendChild(passengerNode);

                passengerCount++;
                SendFun();
                mq.BeginReceive();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error in OnPassengerMessage: " + ex.Message);
            }
        }

        private void OnLuggageMessage(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            try
            {
                MessageQueue mq = (MessageQueue)source;
                Message message = mq.EndReceive(asyncResult.AsyncResult);

                XmlDocument luggageXml = new XmlDocument();
                luggageXml.Load(message.BodyStream);

                XmlNode luggageNode = fullMessage.ImportNode(luggageXml.DocumentElement, true);
                fullMessage.DocumentElement.AppendChild(luggageNode);

                luggageCount++;
                SendFun();
                mq.BeginReceive();
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error in OnPassengerMessage: " + ex.Message);
            }
        }

        private void SendFun()
        {
            if (luggageCount == 2 && passengerCount == 1) 
            {
                messageOutQueue.Send(fullMessage);
            }
        }
    }
}
