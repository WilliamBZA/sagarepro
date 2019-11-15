using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Messages;
using NServiceBus;
using NServiceBus.Persistence.Sql;

class Program
{
    static async Task Main()
    {
        Console.Title = "Samples.Sender";
        var endpointConfiguration = new EndpointConfiguration("Samples.Sender");
        endpointConfiguration.EnableInstallers();
        endpointConfiguration.SendFailedMessagesTo("error");

        var transport = endpointConfiguration.UseTransport<MsmqTransport>();
        transport.Routing().RouteToEndpoint(typeof(TestMessage), "Samples.Subscriber");


        var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
        persistence.SqlDialect<SqlDialect.MsSqlServer>();
        persistence.SubscriptionSettings().DisableCache();
        persistence.ConnectionBuilder(() => new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Test;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"));

        var endpointInstance = await Endpoint.Start(endpointConfiguration)
            .ConfigureAwait(false);

        Console.WriteLine();
        Console.WriteLine("Press 'Enter' to send a StartOrder message");
        Console.WriteLine("Press any other key to exit");

        while (true)
        {
            Console.WriteLine();
            if (Console.ReadKey().Key != ConsoleKey.Enter)
            {
                break;
            }
            var message = new TestMessage();
            await endpointInstance.Send(message).ConfigureAwait(false);
            Console.WriteLine($"Sent TestMessage with OrderId {message.SagaGuid}.");
        }

        await endpointInstance.Stop()
            .ConfigureAwait(false);
    }
}