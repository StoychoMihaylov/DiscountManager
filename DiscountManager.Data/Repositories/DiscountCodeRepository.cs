namespace DiscountManager.Data.Repositories
{
    using DiscountManager.Data.Context;
    using DiscountManager.Data.Documents;
    using MongoDB.Driver;

    public interface IDiscountCodeRepository
    {
        Task<List<DiscountCodeDocument>> GetAllDiscountCodesAsync();
        Task InserNewDiscountCodesAsync(List<DiscountCodeDocument> codes);
    }

    public class DiscountCodeRepository : IDiscountCodeRepository
    {
        private readonly IMongoCollection<DiscountCodeDocument> discountCodeCollection;

        public DiscountCodeRepository(MongoDbContext mongoContext) 
        {
            this.discountCodeCollection = mongoContext
                .Database
                .GetCollection<DiscountCodeDocument>(nameof(DiscountCodeDocument));
        }

        public async Task<List<DiscountCodeDocument>> GetAllDiscountCodesAsync()
        {
            return await this.discountCodeCollection.Find(_ => true).ToListAsync();
        }

        public async Task InserNewDiscountCodesAsync(List<DiscountCodeDocument> codes)
        { 
            await this.discountCodeCollection.InsertManyAsync(codes);
        }
    }
}
