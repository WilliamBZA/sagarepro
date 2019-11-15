using NServiceBus;
using NServiceBus.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscriber
{
    class Program
    {
        static async Task Main()
        {
            Console.Title = "Samples.Subscriber";
            var endpointConfiguration = new EndpointConfiguration("Samples.Subscriber");
            endpointConfiguration.EnableInstallers();
            endpointConfiguration.SendFailedMessagesTo("error");

            var transport = endpointConfiguration.UseTransport<MsmqTransport>();

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.SqlDialect<SqlDialect.MsSqlServer>();
            persistence.SubscriptionSettings().DisableCache();
            persistence.ConnectionBuilder(() => new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));



            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine();
            Console.WriteLine("Press any key to exit");

            Console.ReadKey();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }
    }
}
