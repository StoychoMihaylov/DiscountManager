namespace DiscountManager.Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Console.WriteLine("Please enter your name:");

            string ?name = Console.ReadLine();
            while (String.IsNullOrEmpty(name))
            { 
                Console.WriteLine("Name can't be empty!");
                name = Console.ReadLine();

                if (!String.IsNullOrEmpty(name)) continue;
            }

            Console.WriteLine($"Hello, {name}!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
