using System;
using System.Collections;
using System.Messaging;
using System.Xml;

namespace RecipientList
{
    public class RecipientList
    {
        protected MessageQueue InQueue;
        //protected MessageQueue ControlQueue;

        protected IDictionary routingTable = (IDictionary)(new Hashtable());

        public RecipientList(MessageQueue _inQueue) 
        {
            this.InQueue = _inQueue;
            //this.ControlQueue = _controlQueue;

            InQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnMessage);
            InQueue.BeginReceive();
            
            //ControlQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnMessage);
            //ControlQueue.BeginReceive();
        }

        private void OnMessage(Object source, ReceiveCompletedEventArgs asyncResult)
        {
            MessageQueue mq = (MessageQueue)source;
            Message message = mq.EndReceive(asyncResult.AsyncResult);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(message.BodyStream);

            foreach (XmlNode node in xmlDocument.SelectNodes("//CBPArrivalInfo/Passport/"))
            {
                XmlNode nationality = node.SelectSingleNode("Nationality");
                if (nationality != null)
                {
                    MessageQueue outQueue = FindQueue(nationality.ToString());
                    outQueue.Send(xmlDocument);
                }

            }
                
        }

        protected MessageQueue FindQueue(string queueName)
        {
            if (!MessageQueue.Exists(@".\Private$\" + queueName))
            {
                return MessageQueue.Create(@".\Private$\" + queueName);
            }
            else 
                return new MessageQueue(@".\Private$\" + queueName);
        }
    }
}
