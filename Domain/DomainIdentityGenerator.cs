using Lokad.Cqrs.AtomicStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Domain
{
    public sealed class DomainIdentityGenerator : IDomainIdentityService
    {
        readonly NuclearStorage _storage;

        public DomainIdentityGenerator(NuclearStorage storage)
        {
            _storage = storage;
        }

        public long GetId()
        {
            var ix = new long[1];
            _storage.UpdateSingletonEnforcingNew<DomainIdentityVector>(t => t.Reserve(ix));
            return ix[0];
        }

        public void IncrementDomainIdentity(long id)
        {
            _storage.UpdateSingletonEnforcingNew<DomainIdentityVector>(t =>
            {
                if (t.EntityId < id)
                {
                    t.EntityId = id;
                }
            });
        }
    }

    [DataContract(Namespace = "hub-domain-data", Name = "domainidentityvector")]
    public sealed class DomainIdentityVector
    {
        [DataMember(Order = 1)]
        public long EntityId { get; set; }

        public void Reserve(long[] indexes)
        {
            for (int i = 0; i < indexes.Length; i++)
            {
                EntityId += 1;
                indexes[i] = EntityId;
            }
        }
    }
}
