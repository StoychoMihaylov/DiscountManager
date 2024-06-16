using DiscountManagerController.Grpc;

namespace DiscountManager.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello from DiscountManager Client!");
            Console.WriteLine();
            Console.WriteLine("To see all commands use: 'help'");
            Console.WriteLine("To create discount codes use command: 'run create-discount-codes' ");

            while (true) 
            {
                string? command = Console.ReadLine();

                while (String.IsNullOrEmpty(command))
                {
                    Console.WriteLine("Command can't be empty!");
                    Console.WriteLine("Try again!");
                    command = Console.ReadLine();

                    if (!String.IsNullOrEmpty(command)) continue;
                }

                if (command.Equals("run create-discount-codes"))
                {
                    Console.WriteLine("Please give a number of codes to be created:");

                    var countAsString = Console.ReadLine();
                    while (String.IsNullOrEmpty(countAsString))
                    {
                        Console.WriteLine("Count can't be empty!");
                        Console.WriteLine("Try again!");
                        countAsString = Console.ReadLine();

                        if (!String.IsNullOrEmpty(countAsString)) continue;
                    }

                    if (int.TryParse(countAsString, out var numberOfCodes))
                    {
                        var grpcClinet = new GrpcDiscountManagerControllerClient(new Uri("https://localhost:8080"));
                        var connection = grpcClinet.OpenGrpcDiscountManagerServerConnection();
                        connection.GenerateDiscountCodesAsync(new GenerateDiscountCodesRequest { Count = numberOfCodes });
                    }
                    else
                    {
                        Console.WriteLine("Invalid number. Please enter a valid integer.");
                    }
                }
            }
        }
    }
}
