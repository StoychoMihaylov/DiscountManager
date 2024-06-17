namespace DiscountManager.Models.Options
{
    public class MongoDbOptions
    {
        public static string SectionName => "MongoDbOptions";
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
    }
}
