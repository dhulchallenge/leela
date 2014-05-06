using CarRentals.Contract;
using Lokad.Cqrs.AtomicStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Client
{
    [DataContract]
    public sealed class BookingView
    {
        [DataMember(Order = 1)]
        public string Status { get; set; }
        [DataMember(Order = 2)]
        public int CarId { get; set; }
        [DataMember(Order = 3)]
        public DateTime BookingFromDate { get; set; }
        [DataMember(Order = 4)]
        public DateTime BookingToDate { get; set; }
        [DataMember(Order = 5)]
        public bool IsCancelled { get; set; }
        [DataMember(Order = 6)]
        public DateTime CreatedDate { get; set; }
        [DataMember(Order = 7)]
        public DateTime ModifiedDate { get; set; }
        [DataMember(Order = 8)]
        public BookingId Id { get; set; }
        public BookingView()
        {
        }
    }

    public sealed class BookingsProjection
    {
        readonly IDocumentWriter<BookingId, BookingView> _entity;
        public BookingsProjection(IDocumentWriter<BookingId, BookingView> entity)
        {
            _entity = entity;
        }

        public void When(BookingCreated e)
        {
            _entity.Add(e.Id, new BookingView
            {
                Status ="Booking Created",
                 Id = e.Id,
            });
        }
    }

}
