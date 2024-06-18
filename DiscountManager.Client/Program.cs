namespace DiscountManager.Client
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello from DiscountManager Client!");
            Console.WriteLine();
            Console.WriteLine("To see all commands use: 'help'");
            Console.WriteLine("To create discount codes use command: 'run create-discount-codes' ");

            while (true)
            {
                string? command = Console.ReadLine();

                while (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine("Command can't be empty!");
                    Console.WriteLine("Try again!");
                    command = Console.ReadLine();

                    if (!string.IsNullOrEmpty(command)) continue;
                }

                if (command.Equals("run create-discount-codes", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Please give a number of codes to be created:");

                    var countAsString = Console.ReadLine();
                    while (string.IsNullOrEmpty(countAsString))
                    {
                        Console.WriteLine("Count can't be empty!");
                        Console.WriteLine("Try again!");
                        countAsString = Console.ReadLine();

                        if (!string.IsNullOrEmpty(countAsString)) continue;
                    }

                    if (int.TryParse(countAsString, out var numberOfCodes))
                    {
                        using var grpcClient = new GrpcDiscountManagerControllerClient(new Uri("http://discountmanager-server:8080"));
                        await grpcClient.GenerateDiscountCodesAsync(numberOfCodes);
                    }
                    else
                    {
                        Console.WriteLine("Invalid number. Please enter a valid integer.");
                    }
                }

                if (command.Equals("run get-discount-codes", StringComparison.OrdinalIgnoreCase)) 
                {
                    using var grpcClient = new GrpcDiscountManagerControllerClient(new Uri("http://discountmanager-server:8080"));
                    await grpcClient.GetAllDiscountCodesAsync();
                }

                if (command.Equals("help", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Existing commands:");
                    Console.WriteLine("run create-discount-codes  'creates discount codes'");
                    Console.WriteLine("run get-discount-codes  'returns existing discount codes'");
                }
            }
        }
    }
}
