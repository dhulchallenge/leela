using CarRentals.Contract;
using CarRentals.Domain.Aggregates.Booking;
using Lokad.Cqrs;
using Lokad.Cqrs.Evil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CarRentals.Engine
{
    class Program
    {
        static void Main()
        {
            using (var env = BuildEnvironment())
            using (var cts = new CancellationTokenSource())
            {
                env.ExecuteStartupTasks(cts.Token);
                using (var engine = env.BuildEngine(cts.Token))
                {
                    var task = engine.Start(cts.Token);

                    BookingInfo rent = new BookingInfo(3, DateTime.UtcNow, DateTime.UtcNow.AddDays(1), false,DateTime.UtcNow,DateTime.UtcNow);

                    CreateBooking bookCar = new CreateBooking(new BookingId(3), rent);
                    env.SendToCommandRouter.Send(bookCar);
                    //env.SendToCommandRouter.Send(new BookingAggregate(new BookingState()));

                    Console.WriteLine(@"Press enter to stop");
                    Console.ReadLine();
                    cts.Cancel();
                    if (!task.Wait(5000))
                    {
                        Console.WriteLine(@"Terminating");
                    }
                }
            }
        }


        static void ConfigureObserver()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            var observer = new ConsoleObserver();
            SystemObserver.Swap(observer);
            Context.SwapForDebug(s => SystemObserver.Notify(s));
        }

        public static Container BuildEnvironment()
        {
            //JsConfig.DateHandler = JsonDateHandler.ISO8601;
            ConfigureObserver();
            var integrationPath = AzureSettingsProvider.GetStringOrThrow(Conventions.StorageConfigName);
            //var email = AzureSettingsProvider.GetStringOrThrow(Conventions.SmtpConfigName);


            //var core = new SmtpHandlerCore(email);
            var setup = new Setup
            {
                //Smtp = core,
                //FreeApiKey = freeApiKey,
                //WebClientUrl = clientUri,
                //HttpEndpoint = endPoint,
                //EncryptorTool = new EncryptorTool(systemKey)
            };

            if (integrationPath.StartsWith("file:"))
            {
                var path = integrationPath.Remove(0, 5);

                SystemObserver.Notify("Using store : {0}", path);

                var config = FileStorage.CreateConfig(path);
                setup.Streaming = config.CreateStreaming();
                setup.DocumentStoreFactory = config.CreateDocumentStore;
                setup.QueueReaderFactory = s => config.CreateInbox(s, DecayEvil.BuildExponentialDecay(500));
                setup.QueueWriterFactory = config.CreateQueueWriter;
                setup.AppendOnlyStoreFactory = config.CreateAppendOnlyStore;

                setup.ConfigureQueues(1, 1);

                return setup.Build();
            }
            if (integrationPath.StartsWith("Default") || integrationPath.Equals("UseDevelopmentStorage=true", StringComparison.InvariantCultureIgnoreCase))
            {
                var config = AzureStorage.CreateConfig(integrationPath);
                setup.Streaming = config.CreateStreaming();
                setup.DocumentStoreFactory = config.CreateDocumentStore;
                setup.QueueReaderFactory = s => config.CreateQueueReader(s);
                setup.QueueWriterFactory = config.CreateQueueWriter;
                setup.AppendOnlyStoreFactory = config.CreateAppendOnlyStore;
                setup.ConfigureQueues(4, 4);
                return setup.Build();
            }
            throw new InvalidOperationException("Unsupported environment");
        }
    }
}
