using CarRentals.Contract;
using Lokad.Cloud.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CarRentals.Domain.Aggregates.Booking
{
     public class BookingApplicationService : IBookingApplicationService, IApplicationService
    {
         readonly IEventStore _store;

         public BookingApplicationService(IEventStore store)
        {
            _store = store;
        }

        void Update(ICommand<BookingId> c, Action<BookingAggregate> action)
        {
            var stream = _store.LoadEventStream(c.Id);
            var state = new BookingState(stream.Events);
            var agg = new BookingAggregate(state);

            using (Context.CaptureForThread()) {
                agg.ThrowOnInvalidStateTransition(c);
                action(agg);
                _store.AppendEventsToStream(c.Id, stream.StreamVersion, agg.Changes);
            }
        }

        public void When(CreateBooking c)
        {
            Update(c, ar => ar.CreateBooking(c.Id, c.bookingInfo));
            SaveBooking(c.Id, c.bookingInfo);
        }

        public void When(ChangeBooking c)
        {
            Update(c, ar => ar.Change(c.Id, c.bookingDate));
        }
        public void When(ExtendBooking c)
        {
            Update(c, ar => ar.Extend(c.Id, c.extendBookingDate));
        }
        public void Execute(object command)
        {
            RedirectToWhen.InvokeCommand(this, command);
        }
        private void SaveBooking(BookingId Id, BookingInfo bookingInfo)
        {
            string storageConn = Lokad.Cqrs.AzureSettingsProvider.GetStringOrThrow(Conventions.StorageConfigName);
            var providers = CloudStorage.ForAzureConnectionString(storageConn);
            var booking = new CloudTable<BookingInfo>(providers.BuildTableStorage(), "booking");

            booking.Upsert(

                new CloudEntity<BookingInfo>
                {
                    PartitionKey = bookingInfo.CarId.ToString(),
                    RowKey = Id.Id.ToString(),
                    Timestamp = DateTime.UtcNow,
                    Value = bookingInfo
                }
            );
        }
    }
}
