using CarRentals.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Domain.Aggregates.Booking
{
    public class BookingState : IBookingState
    {
        public string LockoutMessage { get; private set; }
        public BookingId bookingId { get; private set; }
        public int FailuresAllowed { get; private set; }
        public TimeSpan FailureLockoutWindow { get; private set; }
        public TimeSpan ActivityTrackingThreshold { get; private set; }
        //public bool Locked { get; private set; }

        public BookingState(IEnumerable<IEvent> events)
        {
            FailuresAllowed = 5;
            FailureLockoutWindow = TimeSpan.FromMinutes(5);
            // track every login by default
            ActivityTrackingThreshold = TimeSpan.FromMinutes(0);

            foreach (var e in events)
            {
                Mutate(e);
            }
        }

        public void When(BookingCreated e)
        {
            bookingId = e.Id;
        }

        public void When(BookingChanged e)
        {
            bookingId = e.Id;
            ActivityTrackingThreshold = e.ActivityThreshold;
        }
        public void When(BookingExtented e)
        {
            bookingId = e.Id;
            ActivityTrackingThreshold = e.ActivityThreshold;
        }
        public int Version { get; private set; }

        public void Mutate(IEvent e)
        {
            Version += 1;
            RedirectToWhen.InvokeEventOptional(this, e);
        }
    }
}
