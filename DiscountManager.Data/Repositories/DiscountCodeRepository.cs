namespace DiscountManager.Data.Repositories
{
    using DiscountManager.Data.Context;
    using DiscountManager.Data.Documents;
    using MongoDB.Driver;

    public interface IDiscountCodeRepository
    {
        Task<List<DiscountCodeDocument>> GetAllDiscountCodesAsync(CancellationToken cancellationToken);
        Task InserNewDiscountCodesAsync(List<DiscountCodeDocument> codes, CancellationToken cancellationToken);
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

        public async Task<List<DiscountCodeDocument>> GetAllDiscountCodesAsync(CancellationToken cancellationToken)
        {
            var result = await this.discountCodeCollection.FindAsync(_ => true, cancellationToken: cancellationToken);
            return await result.ToListAsync(cancellationToken);
        }

        public async Task InserNewDiscountCodesAsync(List<DiscountCodeDocument> codes, CancellationToken cancellationToken)
        { 
            await this.discountCodeCollection.InsertManyAsync(codes, null, cancellationToken);
        }
    }
}
