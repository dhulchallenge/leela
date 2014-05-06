using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Contract
{
    /// <summary>
    /// Started {role}. {instance}
    /// </summary>
    [DataContract(Namespace = "Lokad.SaaS")]
    public partial class InstanceStarted : IFuncEvent
    {
        [DataMember(Order = 1)]
        public string CodeVersion { get; private set; }
        [DataMember(Order = 2)]
        public string Role { get; private set; }
        [DataMember(Order = 3)]
        public string Instance { get; private set; }

        InstanceStarted() { }
        public InstanceStarted(string codeVersion, string role, string instance)
        {
            CodeVersion = codeVersion;
            Role = role;
            Instance = instance;
        }

        public override string ToString()
        {
            return string.Format(@"Started {0}. {1}", Role, Instance);
        }
    }

    [DataContract]
    public partial class CreateCar : ICommand<CarId>
    {
        [DataMember(Order = 1)] public CarId Id { get; private set; }
        [DataMember(Order = 2)] public CarInfo carInfo { get; private set; }
        
        CreateCar () {}
        public CreateCar(CarId voCarId, CarInfo voCarInfo)
        {
            Id = voCarId;
            carInfo = voCarInfo;
        }
        
        public override string ToString()
        {
            return string.Format(@"Create car {0} ", carInfo.CarModel);
        }
    }

    [DataContract]
    public partial class CarCreated : IEvent<CarId>
    {
        [DataMember(Order = 1)] public CarId Id { get; private set; }
        [DataMember(Order = 2)] public TimeSpan ActivityThreshold { get; private set; }
        
        CarCreated () {}
        public CarCreated(CarId voCarId, TimeSpan activityThreshold)
        {
            Id = voCarId;
            ActivityThreshold = activityThreshold;
        }
        
        public override string ToString()
        {
            return string.Format(@"Created user {0} with threshold {2}", Id.Id, ActivityThreshold);
        }
    }

    [DataContract]
    public partial class CreateBooking: ICommand<BookingId>
    {
        [DataMember(Order = 1)]
        public BookingId Id { get; private set; }
        [DataMember(Order = 2)] public BookingInfo bookingInfo { get; private set; }

        CreateBooking () {}
        public CreateBooking(BookingId voBookingId,BookingInfo voBookingInfo)
        {
            Id = voBookingId;
            bookingInfo = voBookingInfo;
        }
        public override string ToString()
        {
            return string.Format(@"Create car {0} ", Id.Id);
        }
    }

    [DataContract]
    public partial class BookingCreated : IEvent<BookingId>
    {
        [DataMember(Order = 1)]
        public BookingId Id { get; private set; }
        [DataMember(Order = 2)]
        public TimeSpan ActivityThreshold { get; private set; }

        BookingCreated() { }
        public BookingCreated(BookingId voBookingId, TimeSpan activityThreshold)
        {
            Id = voBookingId;
            ActivityThreshold = activityThreshold;
        }
        public override string ToString()
        {
            return string.Format(@"Create car {0} ", Id.Id);
        }
    }

    [DataContract]
    public partial class ChangeBooking : ICommand<BookingId>
    {
        [DataMember(Order = 1)]
        public BookingId Id { get; private set; }
        [DataMember(Order = 2)]
        public BookingDate bookingDate { get; private set; }

        ChangeBooking() { }
        public ChangeBooking(BookingId voBookingId, BookingDate voBookingDate)
        {
            Id = voBookingId;
            bookingDate = voBookingDate;
        }
        public override string ToString()
        {
            return string.Format(@"Changing booking duration of car {0} as {1} to {2} ", Id.Id , bookingDate.BookingStartDate , bookingDate.BookingEndDate);
        }
    }

    [DataContract]
    public partial class BookingChanged : IEvent<BookingId>
    {
        [DataMember(Order = 1)]
        public BookingId Id { get; private set; }
        [DataMember(Order = 2)]
        public TimeSpan ActivityThreshold { get; private set; }

        BookingChanged() { }
        public BookingChanged(BookingId voBookingId, TimeSpan activityThreshold)
        {
            Id = voBookingId;
            ActivityThreshold = activityThreshold;
        }
        public override string ToString()
        {
            return string.Format(@"Booking chnaged for car {0} ", Id.Id);
        }
    }


    [DataContract]
    public partial class ExtendBooking : ICommand<BookingId>
    {
        [DataMember(Order = 1)]
        public BookingId Id { get; private set; }
        [DataMember(Order = 2)]
        public ExtendBookingDate extendBookingDate { get; private set; }

        ExtendBooking() { }
        public ExtendBooking(BookingId voBookingId, ExtendBookingDate voExtendBookingDate)
        {
            Id = voBookingId;
            extendBookingDate = voExtendBookingDate;
        }
        public override string ToString()
        {
            return string.Format(@"Ex6tending booking duration of car {0} till {1} ", Id.Id, extendBookingDate.BookingEndDate);
        }
    }

    [DataContract]
    public partial class BookingExtented : IEvent<BookingId>
    {
        [DataMember(Order = 1)]
        public BookingId Id { get; private set; }
        [DataMember(Order = 2)]
        public TimeSpan ActivityThreshold { get; private set; }

        BookingExtented() { }
        public BookingExtented(BookingId voBookingId, TimeSpan activityThreshold)
        {
            Id = voBookingId;
            ActivityThreshold = activityThreshold;
        }
        public override string ToString()
        {
            return string.Format(@"Booking Extented for car {0} ", Id.Id);
        }
    }

    public interface ICarApplicationService
    {
        void When(CreateCar e);
    }
    public interface ICarState
    {
        void When(CarCreated e);
    }

    public interface IBookingState
    {
        void When(BookingCreated e);
        void When(BookingChanged e);
        void When(BookingExtented e);
    }

    public interface IBookingApplicationService
    {
        void When(CreateBooking e);
        void When(ChangeBooking e);
        void When(ExtendBooking e);
    }

}
