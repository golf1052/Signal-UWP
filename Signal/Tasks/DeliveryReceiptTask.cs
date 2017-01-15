using libsignalservice;
using libsignalservice.push;
using Signal.Tasks.Library;
using Strilanc.Value;
using System;
using TextSecure;
using Signal.Push;
using Signal.Util;
using TextSecure.util;
using Signal.Database;

namespace Signal.Tasks
{
    public class DeliveryReceiptTask : UntypedTaskActivity
    {

        private string destination;
        private long timestamp;
        private string relay;

        protected SignalServiceMessageSender messageSender = new SignalServiceMessageSender(TextSecureCommunicationFactory.PUSH_URL, new TextSecurePushTrustStore(), TextSecurePreferences.getLocalNumber(), TextSecurePreferences.getPushServerPassword(), new TextSecureAxolotlStore(),
                                                                          May<SignalServiceMessageSender.EventListener>.NoValue, App.CurrentVersion);


        public DeliveryReceiptTask(string destination, long timestamp, string relay)
        {
            this.destination = destination;
            this.timestamp = timestamp;
            this.relay = relay;
        }

        public override void onAdded()
        {
            //throw new NotImplementedException("SendTask onAdded");
        }

        public new void OnCanceled()
        {
            //throw new NotImplementedException("SendTask OnCanceled");
        }

        protected override string Execute()
        {
            Log.Debug("DeliveryReceiptJob : Sending delivery receipt...");
            SignalServiceAddress textSecureAddress = new SignalServiceAddress(destination, new May<string>(relay));

            messageSender.sendDeliveryReceipt(textSecureAddress, (ulong)timestamp);

            return "";
        }

    }
}
