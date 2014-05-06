using CarRentals.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Domain
{
    
    public sealed class DomainSender
    {
        readonly ICommandSender _service;

        public DomainSender(ICommandSender service)
        {
            _service = service;
        }

        public void ToBooking(ICommand<BookingId> cmd)
        {
            _service.SendCommand(cmd);
        }
    }
}
