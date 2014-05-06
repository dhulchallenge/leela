using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Contract
{
    public static class IdentityConvert
    {
        public static string ToStream(IIdentity identity)
        {
            return identity.GetTag() + "-" + identity.GetId();
        }

        public static string ToTransportable(IIdentity identity)
        {
            return identity.GetTag() + "-" + identity.GetId();
        }
    }


    [DataContract]
    public sealed class CarId : AbstractIdentity<int>
    {
        public const string TagValue = "Car";

        public CarId(int id)
        {
            System.Diagnostics.Contracts.Contract.Requires(id > 0);
            Id = id;
        }

        public override string GetTag()
        {
            return TagValue;
        }


        [DataMember(Order = 1)]
        public override int Id { get; protected set; }

        public CarId() { }
    }


    [DataContract]
    public sealed class BookingId : AbstractIdentity<int>
    {
        public const string TagValue = "Booking";

        public BookingId(int id)
        {
            if (id <= 0)
                throw new InvalidOperationException("Tried to assemble non-existent Booking");
            Id = id;
        }

        public override string GetTag()
        {
            return TagValue;
        }

        public BookingId() { }

        [DataMember(Order = 1)]
        public override int Id { get; protected set; }
    }
}
