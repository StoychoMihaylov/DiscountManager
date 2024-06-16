namespace DiscountManager.Models.Options
{
    public class MongoDbOptions
    {
        public static string SectionName => "MongoDbOptions";
        public string DatabaseName { get; }
        public string ConnectionString { get; }
    }
}
