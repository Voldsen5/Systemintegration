using System.Messaging;

namespace Point_To_Point_Channel
{
    internal class Air_Trafic
    {
        public void Send(string destination, Message message)
        {
            string queuePath = @".\Private$\" + destination;

            if (!MessageQueue.Exists(queuePath))
            {
                MessageQueue.Create(queuePath);
            }

            using (MessageQueue mQ = new MessageQueue(queuePath))
            {
                mQ.Send(message);
            }
        }
    }
}
