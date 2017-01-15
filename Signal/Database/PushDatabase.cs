using libsignalservice.messages;
using SQLite.Net;
using SQLite.Net.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Signal.Util;
using System.Diagnostics;

namespace Signal.Database
{
    [Table("Push")]
    public class Push
    {
        [PrimaryKey, AutoIncrement]
        public long PushId { get; set; }
        public long Type { get; set; }
        public string Source { get; set; }
        public long DeviceId { get; set; }
        public string LegacyMessage { get; set; }
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class PushDatabase
    {
        SQLiteConnection conn;

        public PushDatabase(SQLiteConnection conn)
        {
            this.conn = conn;
            conn.CreateTable<Push>();
        }

        public long Insert(SignalServiceEnvelope envelope)
        {
            // TODO check if exists
            var push = new Push()
            {
                Type = envelope.getType(),
                Source = envelope.getSource(),
                DeviceId = envelope.getSourceDevice(),
                LegacyMessage = envelope.hasLegacyMessage() ? Base64.encodeBytes(envelope.getLegacyMessage()) : "",
                Content = envelope.hasContent() ? Base64.encodeBytes(envelope.getContent()) : "",
                Timestamp = TimeUtil.GetDateTime(envelope.getTimestamp())
            };

            try
            {
                conn.Insert(push);
            } catch(Exception e) { Debug.WriteLine(e.Message); }
            

            return push.PushId;
        }

        public void Delete(long pushId)
        {
            conn.Delete<Push>(pushId);
        }

        public SignalServiceEnvelope GetEnvelope(long pushId)
        {
            var push = conn.Get<Push>(pushId);


            return new SignalServiceEnvelope((int)push.Type, push.Source, (int)push.DeviceId, "", (long)TimeUtil.GetUnixTimestamp(push.Timestamp),
                push.LegacyMessage.Equals("") ? null : Base64.decode(push.LegacyMessage),
                push.Content.Equals("") ? null : Base64.decode(push.Content));
        }
    }
}
