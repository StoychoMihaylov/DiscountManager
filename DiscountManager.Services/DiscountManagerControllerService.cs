namespace DiscountManagerController.Grpc
{
    using DiscountManager.Services;
    using global::Grpc.Core;

    public class DiscountManagerControllerService : DiscountManagerService.DiscountManagerServiceBase
    {
        private readonly IDiscountCodeGeneratorService discountCodeGeneratorService;

        public DiscountManagerControllerService(IDiscountCodeGeneratorService discountCodeGeneratorService)
        {
            this.discountCodeGeneratorService = discountCodeGeneratorService;
        }

        public override async Task<GenerateDiscountCodesResponse> GenerateDiscountCodes(GenerateDiscountCodesRequest request, ServerCallContext context)
        {
            var discountCodes = await this.discountCodeGeneratorService.GenerateDiscountCodesAsync(request.Count);
            var response = new GenerateDiscountCodesResponse();
            response.Codes.AddRange(discountCodes.Select(doc => doc.Code));
            return response;
        }
    }
}
