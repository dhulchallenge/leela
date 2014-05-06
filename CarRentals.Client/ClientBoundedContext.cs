using CarRentals.Contract;
using Lokad.Cqrs.AtomicStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentals.Client
{
    public class ClientBoundedContext
    {
        public static IEnumerable<object> Projections(IDocumentStore docs)
        {
            yield return new BookingsProjection(docs.GetWriter<BookingId, BookingView>());
           
        }
    }
}
