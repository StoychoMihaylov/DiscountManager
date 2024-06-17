namespace DiscountManager.Services
{
    using DiscountManager.Data.Documents;
    using DiscountManager.Data.Repositories;
    using DiscountManager.Models.Options;
    using Microsoft.Extensions.Options;

    public interface IDiscountCodeGeneratorService
    {
        Task<List<DiscountCodeDocument>> GenerateDiscountCodesAsync(int count);
        Task<List<DiscountCodeDocument>> GetAllDiscountCodesAsync();
    }

    public class DiscountCodeGeneratorService : IDiscountCodeGeneratorService
    {
        private readonly IDiscountCodeRepository discountCodeRepository;
        private readonly ServiceSettings serviceSettings;
        private static readonly Random Random = new Random();

        public DiscountCodeGeneratorService(IDiscountCodeRepository discountCodeRepository, IOptions<ServiceSettings> serviceSettings)
        {
            this.discountCodeRepository = discountCodeRepository;
            this.serviceSettings = serviceSettings.Value;
        }

        public async Task<List<DiscountCodeDocument>> GetAllDiscountCodesAsync()
        {
            return await this.discountCodeRepository.GetAllDiscountCodesAsync();
        }

        public async Task InsertNewDiscountCodesAsync(List<DiscountCodeDocument> codes)
        {
            if (codes == null || codes.Count == 0)
            {
                throw new ArgumentException("No discount codes provided to insert.");
            }

            if (codes.Count > serviceSettings.MaxDiscountCodesPerRequest)
            {
                throw new ArgumentException($"Cannot insert more than {serviceSettings.MaxDiscountCodesPerRequest} discount codes in a single request.");
            }

            await this.discountCodeRepository.InserNewDiscountCodesAsync(codes);
        }

        public async Task<List<DiscountCodeDocument>> GenerateDiscountCodesAsync(int count)
        {
            if (count < 1 || count > serviceSettings.MaxDiscountCodesPerRequest)
            {
                throw new ArgumentException($"Count must be between 1 and {serviceSettings.MaxDiscountCodesPerRequest}.");
            }

            var generatedCodes = new HashSet<string>();

            while (generatedCodes.Count < count)
            {
                var code = GenerateRandomDiscountCode();
                generatedCodes.Add(code);
            }

            var discountCodeDocuments = generatedCodes
                .Select(code => 
                    new DiscountCodeDocument { Code = code }
                )
                .ToList();

            await InsertNewDiscountCodesAsync(discountCodeDocuments);

            return discountCodeDocuments;
        }

        private static string GenerateRandomDiscountCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            return new string(Enumerable.Repeat(chars, 8)
              .Select(s => s[Random.Next(s.Length)])
              .ToArray());
        }
    }
}
