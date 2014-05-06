using CarRentals.Wires;
using Lokad.Cqrs;
using Lokad.Cqrs.Envelope;
using Lokad.Cqrs.Evil;
using Lokad.Cqrs.StreamingStorage;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Engine
{
    public sealed class EnvelopeQuarantine : IEnvelopeQuarantine
    {
        readonly IStreamContainer _root;
        readonly IEnvelopeStreamer _streamer;

        readonly MemoryQuarantine _quarantine = new MemoryQuarantine();
        readonly TypedMessageSender _writer;

        public EnvelopeQuarantine(IEnvelopeStreamer streamer,
            TypedMessageSender writer, IStreamContainer root)
        {
            _streamer = streamer;
            _writer = writer;
            _root = root;
            _root.Create();
        }

        public bool TryToQuarantine(ImmutableEnvelope context, Exception ex)
        {
            // quit immediately, we don't want an endless cycle!
            //if (context.Items.Any(m => m.Content is MessageQuarantined))
            //    return true;
            var quarantined = _quarantine.TryToQuarantine(context, ex);

            try
            {
                var file = string.Format("{0:yyyy-MM-dd}-{1}-engine.txt",
                    DateTime.UtcNow,
                    context.EnvelopeId.ToLowerInvariant());



                var data = "";
                if (_root.Exists(file))
                {
                    using (var stream = _root.OpenRead(file))
                    using (var reader = new StreamReader(stream))
                    {
                        data = reader.ReadToEnd();
                    }
                }

                var builder = new StringBuilder(data);
                if (builder.Length == 0)
                {
                    DescribeMessage(builder, context);
                }

                builder.AppendLine("[Exception]");
                builder.AppendLine(DateTime.UtcNow.ToString(CultureInfo.InvariantCulture));
                builder.AppendLine(ex.ToString());
                builder.AppendLine();

                var text = builder.ToString();

                using (var stream = _root.OpenWrite(file))
                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(text);
                }
            }
            catch (Exception x)
            {
                SystemObserver.Notify(x.ToString());
            }

            return quarantined;
        }

        public void Quarantine(byte[] message, Exception ex) { }


        public void TryRelease(ImmutableEnvelope context)
        {
            _quarantine.TryRelease(context);
        }

        static void DescribeMessage(StringBuilder builder, ImmutableEnvelope context)
        {
            builder.AppendLine(context.PrintToString(o => JsvFormatter.Format(JsonSerializer.SerializeToString(o))));
        }


    }
}
