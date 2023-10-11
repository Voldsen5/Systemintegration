using System;
using System.Messaging;

namespace Point_To_Point_Channel
{
    internal class Air_Info
    {
        public Message Receive(string receiveFrom)
        {
            string queuePath = @".\Private$\" + receiveFrom;

            if (!MessageQueue.Exists(queuePath))
            {
                throw new InvalidOperationException("Queue does not exist.");
            }

            using (MessageQueue mQ = new MessageQueue(queuePath))
            {
                mQ.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                Message message = mQ.Receive();
                return message;
            }
        }
    }
}
