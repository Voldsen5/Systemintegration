using System;
using System.Messaging;
using System.Threading;
using System.Xml;

namespace Aggregator
{
        public class CheckInAggregator
        {
            protected MessageQueue passengerQueue;
            protected MessageQueue luggageQueue;
            protected MessageQueue messageOutQueue;

            private bool passengerReceived = false;
            private bool luggage1Received = false;
            private bool luggage2Received = false;

            public CheckInAggregator(MessageQueue passengerQueue, MessageQueue luggageQueue, MessageQueue messageOutQueue)
            {
                this.passengerQueue = passengerQueue;
                this.luggageQueue = luggageQueue;
                this.messageOutQueue = messageOutQueue;

                passengerQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnPassengerMessage);
                passengerQueue.BeginReceive();

                luggageQueue.ReceiveCompleted += new ReceiveCompletedEventHandler(OnLuggageMessage);
                luggageQueue.BeginReceive();
            }

            private XmlDocument passengerXml = new XmlDocument();
            private XmlDocument luggageXml1 = new XmlDocument();
            private XmlDocument luggageXml2 = new XmlDocument();

            private void OnPassengerMessage(Object source, ReceiveCompletedEventArgs asyncResult)
            {
                MessageQueue mq = (MessageQueue)source;
                Message message = mq.EndReceive(asyncResult.AsyncResult);
                string label = message.Label;

                passengerXml.Load(message.BodyStream);
                passengerReceived = true;
                TryCombineAndSend();
                mq.BeginReceive();
            }

            private void OnLuggageMessage(Object source, ReceiveCompletedEventArgs asyncResult)
            {
                MessageQueue mq = (MessageQueue)source;
                Message message = mq.EndReceive(asyncResult.AsyncResult);
                string label = message.Label;

                if (!luggage1Received)
                {
                    luggageXml1.Load(message.BodyStream);
                    luggage1Received = true;
                }
                else
                {
                    luggageXml2.Load(message.BodyStream);
                    luggage2Received = true;
                }

                TryCombineAndSend();
                mq.BeginReceive();
            }

            private void TryCombineAndSend()
              {
                if (passengerReceived && luggage1Received && luggage2Received)
                {
                    XmlNode luggageNode1 = passengerXml.ImportNode(luggageXml1.DocumentElement, true);
                    XmlNode luggageNode2 = passengerXml.ImportNode(luggageXml2.DocumentElement, true);
                    passengerXml.DocumentElement.AppendChild(luggageNode1);
                    passengerXml.DocumentElement.AppendChild(luggageNode2);

                    if (messageOutQueue != null)
                    {
                        string xmlData = passengerXml.OuterXml;
                        Console.WriteLine("Sending Combined XML: " + xmlData);

                        try
                        {
                            Message message = new Message(xmlData);
                            messageOutQueue.Send(message);
                            Console.WriteLine("Combined XML sent successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Error sending Combined XML: " + ex.Message);
                        }
                    }
                    else
                    {
                        Console.WriteLine("No output queue specified.");
                    }

                    passengerReceived = false;
                    luggage1Received = false;
                    luggage2Received = false;
                }
            }
        }
    }

