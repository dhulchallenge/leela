using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Contract
{
    public partial class BookingInfo
    {
        [DataMember(Order = 1)]
        public int CarId { get; private set; }
        [DataMember(Order = 2)]
        public DateTime BookingFromDate { get; private set; }
        [DataMember(Order = 3)]
        public DateTime BookingToDate { get; private set; }
        [DataMember(Order = 4)]
        public bool IsCancelled { get; private set; }
        [DataMember(Order = 5)]
        public DateTime CreatedDate { get; private set; }
        [DataMember(Order = 6)]
        public DateTime ModifiedDate { get; private set; }

        public BookingInfo(int carId, DateTime bookingFromDate, DateTime bookingToDate, bool isCancelled, DateTime createdDate, DateTime modifiedDate)
        {
            CarId = carId;
            BookingFromDate = bookingFromDate;
            BookingToDate = bookingToDate;
            IsCancelled = isCancelled;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
        }
    }

    [DataContract]
    public sealed class BookingDate
    {
        public BookingDate(DateTime bookingStartDate,DateTime bookingEndDate)
        {
            BookingStartDate = bookingStartDate;
            BookingEndDate = bookingEndDate;
        }

        public BookingDate() { }

        [DataMember(Order = 1)]
        public DateTime BookingStartDate { get; protected set; }
        [DataMember(Order = 2)]
        public DateTime BookingEndDate { get; protected set; }
 
    }

    [DataContract]
    public sealed class ExtendBookingDate
    {
        public ExtendBookingDate(DateTime bookingEndDate)
        {
            if (bookingEndDate <= DateTime.MinValue)
                throw new InvalidOperationException("Tried to assemble invalid Booking end date");
            BookingEndDate = bookingEndDate;
        }

        public ExtendBookingDate() { }
        
        [DataMember(Order = 1)]
        public DateTime BookingEndDate { get; protected set; }

    }

    
}
