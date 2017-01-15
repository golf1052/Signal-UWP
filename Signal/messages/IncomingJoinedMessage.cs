using libsignalservice.messages;
using Signal.Util;
using Strilanc.Value;

namespace Signal.Messages
{
    internal class IncomingJoinedMessage : IncomingTextMessage
    {
        public IncomingJoinedMessage(string sender) : base(sender, 1, (long)TimeUtil.GetUnixTimestampMillis(), null, May<SignalServiceGroup>.NoValue)
        {
        }

        public bool IsJoined => true;

        public bool IsSecureMessage => true;
    }
}
