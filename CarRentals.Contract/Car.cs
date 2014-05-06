using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Contract
{
    [DataContract]
    public partial class CarInfo
    {
        [DataMember(Order = 1)]
        public int CarId { get; private set; }
        [DataMember(Order = 2)]
        public string CarModel { get; private set; }
        [DataMember(Order = 3)]
        public float RentalCost { get; private set; }
        [DataMember(Order = 4)]
        public DateTime CreatedDate { get; private set; }
        [DataMember(Order = 5)]
        public DateTime ModifiedDate { get; private set; }

        public CarInfo(int carId, string carModel, float rentalCost, DateTime createdDate, DateTime modifiedDate)
        {
            CarId = carId;
            CarModel = carModel;
            RentalCost = rentalCost;
            CreatedDate = createdDate;
            ModifiedDate = modifiedDate;
        }
    }

   
}
