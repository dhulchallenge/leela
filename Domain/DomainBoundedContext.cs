using CarRentals.Contract;
using CarRentals.Domain.Aggregates.Booking;
using Lokad.Cqrs.AtomicStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarRentals.Domain
{
    public static class DomainBoundedContext
    {
        public static string EsContainer = "hub-domain-tape";

        public static IEnumerable<Func<CancellationToken, Task>> Tasks(ICommandSender service, IDocumentStore docs,
            bool isTest)
        {
            var flow = new DomainSender(service);
            // more tasks go here
            yield break;
        }


        public static IEnumerable<object> EntityApplicationServices(IDocumentStore docs, IEventStore store, DomainIdentityGenerator id)
        {
            yield return new BookingApplicationService(store);
            yield return id;
        }
        public static IEnumerable<object> FuncApplicationServices()
        {
            yield break;

        }
    }
}
