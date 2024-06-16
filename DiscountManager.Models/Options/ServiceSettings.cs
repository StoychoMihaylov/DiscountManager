namespace DiscountManager.Models.Options
{
    public class ServiceSettings
    {
        public static string SectionName => "ServiceSettings";
        public int MaxDiscountCodesPerRequest { get; }
    }
}
