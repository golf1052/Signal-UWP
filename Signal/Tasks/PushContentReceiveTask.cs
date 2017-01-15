﻿using libsignalservice;
using libsignalservice.messages;
using libsignalservice.push;
using libsignalservice.util;
using Signal.Tasks.Library;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TextSecure.database;
using TextSecure.util;

namespace Signal.Tasks
{
    class PushContentReceiveTask : PushReceivedTask
    {
        private string data;

        public PushContentReceiveTask()
        {
        }

        public PushContentReceiveTask(string data)
        {
            this.data = data;
        }

        public override void onAdded()
        {
        }

        protected override string Execute()
        {
            throw new NotImplementedException("PushContentReceiveTask Execute");
        }

        protected override async Task<string> ExecuteAsync()
        {
            try
            {
                String sessionKey = TextSecurePreferences.getSignalingKey();
                SignalServiceEnvelope envelope = new SignalServiceEnvelope(data, sessionKey);

                handle(envelope, true);
            }
            catch (/*IOException | InvalidVersion*/Exception e) {
                Debug.WriteLine($"{this.GetType().Name}: Error: {e.Message}");
            }

            return "";
        }

    }

}