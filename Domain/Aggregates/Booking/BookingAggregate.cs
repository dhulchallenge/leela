using CarRentals.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Domain.Aggregates.Booking
{
    public sealed class BookingAggregate
    {
        readonly BookingState _state;
        public IList<IEvent> Changes = new List<IEvent>();
        public static readonly TimeSpan DefaultActivityThreshold = TimeSpan.FromMinutes(10);
        public BookingAggregate(BookingState state)
        {
            _state = state;
        }

        public void CreateBooking(BookingId bookingId, BookingInfo bookingInfo)
        {
            if (_state.Version != 0)
                throw new DomainError("Booking already has non-zero version");
            CreateBooking createBooking = new CarRentals.Contract.CreateBooking(bookingId, bookingInfo);
            Apply(new BookingCreated(bookingId, DefaultActivityThreshold));
        }

        public void Change(BookingId bookingId, BookingDate bookingDate)
        {
            if (_state.Version != 0)
                throw new DomainError("Cannot Update the booking date");
            ChangeBooking revisedBooking = new CarRentals.Contract.ChangeBooking(bookingId, bookingDate);
            Apply(new BookingChanged(bookingId, DefaultActivityThreshold));
        }

        public void Extend(BookingId bookingId, ExtendBookingDate extendBookingDate)
        {
            if (_state.Version != 0)
                throw new DomainError("Cannot Update the booking date");
            ExtendBooking extendedBooking = new CarRentals.Contract.ExtendBooking(bookingId, extendBookingDate);
            Apply(new BookingExtented(bookingId, DefaultActivityThreshold));
        }

        public void ThrowOnInvalidStateTransition(ICommand<BookingId> e)
        {
            if (_state.Version == 0)
            {
                if (e is CreateBooking)
                {
                    return;
                }
                throw DomainError.Named("premature", "Can't do anything to unexistent aggregate");
            }
            if (_state.Version == -1)
            {
                throw DomainError.Named("zombie", "Can't do anything to deleted aggregate.");
            }
            if (e is CreateBooking)
                throw DomainError.Named("rebirth", "Can't create aggregate that already exists");
        }

        void Apply(IEvent<BookingId> e)
        {
            _state.Mutate(e);
            Changes.Add(e);
        }
    }
}
