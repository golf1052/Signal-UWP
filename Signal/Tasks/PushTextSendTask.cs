using libsignalservice;
using libsignalservice.messages;
using libsignalservice.push;
using libsignalservice.util;
using Signal.Database;
using Signal.Models;
using Signal.Tasks.Library;
using Signal.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Signal.Push;
using Strilanc.Value;
using TextSecure;
using TextSecure.database;
using TextSecure.recipient;
using TextSecure.util;

namespace Signal.Tasks
{
    class PushTextSendTask : PushSendTask
    {
        private long messageId;
        protected SignalServiceMessageSender messageSender = new SignalServiceMessageSender(TextSecureCommunicationFactory.PUSH_URL, new TextSecurePushTrustStore(), TextSecurePreferences.getLocalNumber(), TextSecurePreferences.getPushServerPassword(), new TextSecureAxolotlStore(),
                                                                                  May<SignalServiceMessageSender.EventListener>.NoValue, App.CurrentVersion);

        public PushTextSendTask(long messageId, string destination) : base(destination)
        {
            this.messageId = messageId;
        }

        public override void onAdded()
        {
            var textDatabase = DatabaseFactory.getTextMessageDatabase();
            textDatabase.MarkAsSending(messageId);
            textDatabase.MarkAsPush(messageId);
        }

        public new async  void OnCanceled()
        {
            DatabaseFactory.getTextMessageDatabase().MarkAsSentFailed(messageId);

            long threadId = DatabaseFactory.getTextMessageDatabase().GetThreadIdForMessage(messageId); // TODO
            Recipients recipients = await DatabaseFactory.getThreadDatabase().getRecipientsForThreadId(threadId);

            if (threadId != -1 && recipients != null)
            {
                ToastHelper.NotifyMessageDeliveryFailed(recipients, threadId);
            }
        }

        protected override string Execute()
        {
            throw new NotImplementedException("TextSendTask Execute happened");
        }

        protected override async Task<string> ExecuteAsync()
        {
            TextMessageDatabase database = DatabaseFactory.getTextMessageDatabase();
            var record = await database.getMessageRecord(messageId);
            //var message = await database.Get(messageId);

            try
            {
                await deliver(record);
                database.MarkAsPush(messageId);
                database.MarkAsSecure(messageId);
                database.MarkAsSent(messageId);

            }
            catch (libsignalservice.crypto.UntrustedIdentityException e)
            {
                Log.Debug($"Untrusted Identity Key: {e.getIdentityKey()} {e.getE164Number()}");
                var recipients = RecipientFactory.getRecipientsFromString(e.getE164Number(), false);
                long recipientId = recipients.getPrimaryRecipient().getRecipientId();

                database.AddMismatchedIdentity(record.MessageId, recipientId, e.getIdentityKey());
                database.MarkAsSentFailed(record.MessageId);
                database.MarkAsPush(record.MessageId);
            }
            catch (Exception e)
            {
                Log.Error($"Unexpected Exception {e.Source} {e.GetType()} : {e.Message}");
            }


            return "";
            //throw new NotImplementedException();
        }

        private async Task deliver(TextMessageRecord message)
        {
            try
            {
                SignalServiceAddress address = getPushAddress(message.IndividualRecipient.Number);
                SignalServiceDataMessage textSecureMessage = SignalServiceDataMessage.newBuilder()
                    .withTimestamp(TimeUtil.GetUnixTimestampMillis(message.DateSent))
                    .withBody(message.Body.Body)
                    .asEndSessionMessage(message.IsEndSession)
                    .build();

                Debug.WriteLine("TextSendTask deliver");
                await messageSender.sendMessage(address, textSecureMessage);
            }
            catch (InvalidNumberException e /*| UnregisteredUserException e*/)
            {
                //Log.w(TAG, e);
                //throw new InsecureFallbackApprovalException(e);
            }
            catch (Exception e)
            {
                Log.Warn("Delivery of message failed");
                OnCanceled();
            }
        }
    }

}