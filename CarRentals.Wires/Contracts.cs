using CarRentals.Contract;
using Lokad.Cqrs.Envelope;
using Lokad.Cqrs.Evil;
using ProtoBuf.Meta;
using ServiceStack.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Wires
{
    public static class Contracts
    {
        static Type[] LoadMessageContracts()
        {
            var messages = new[] { typeof(InstanceStarted) }
                .SelectMany(t => t.Assembly.GetExportedTypes())
                .Where(t => typeof(ISampleMessage).IsAssignableFrom(t))
                .Where(t => !t.IsAbstract)
                .ToArray();
            return messages;

        }

        public static EnvelopeStreamer CreateStreamer()
        {
            return new EnvelopeStreamer(new DataSerializer(LoadMessageContracts()));
        }


        class DataSerializer : AbstractMessageSerializer
        {
            public DataSerializer(ICollection<Type> knownTypes)
                : base(knownTypes)
            {
                RuntimeTypeModel.Default[typeof(DateTimeOffset)].Add("m_dateTime", "m_offsetMinutes");
            }

            protected override Formatter PrepareFormatter(Type type)
            {
                var name = ContractEvil.GetContractReference(type);
                return new Formatter(name, type, s => JsonSerializer.DeserializeFromStream(type, s), (o, s) =>
                {
                    using (var writer = new StreamWriter(s))
                    {
                        writer.WriteLine();
                        writer.WriteLine(JsvFormatter.Format(JsonSerializer.SerializeToString(o, type)));
                    }

                });
                //var formatter = RuntimeTypeModel.Default.CreateFormatter(type);
                //return new Formatter(name, formatter.Deserialize, (o, stream) => formatter.Serialize(stream, o));
            }
        }
    }
}
