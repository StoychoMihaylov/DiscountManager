namespace DiscountManager.Data.Documents
{
    using MongoDB.Bson.Serialization.Attributes;

    public class DiscountCodeDocument
    {
        public DiscountCodeDocument() {}

        [BsonId]
        public string Code { get; set; }
    }
}
