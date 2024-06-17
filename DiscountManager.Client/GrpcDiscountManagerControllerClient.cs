namespace DiscountManager.Client
{
    using System;
    using System.Threading.Tasks;
    using DiscountManagerController.Grpc;
    using Grpc.Net.Client;

    public class GrpcDiscountManagerControllerClient : IDisposable
    {
        private readonly GrpcChannel _channel;
        private readonly DiscountManagerService.DiscountManagerServiceClient _client;

        public GrpcDiscountManagerControllerClient(Uri serverUri)
        {
            _channel = GrpcChannel.ForAddress(serverUri, new GrpcChannelOptions
            {
                HttpHandler = new HttpClientHandler()
                {
                    UseCookies = false,
                    AllowAutoRedirect = false,
                    AutomaticDecompression = System.Net.DecompressionMethods.None
                }
            });
            _client = new DiscountManagerService.DiscountManagerServiceClient(_channel);
        }

        public async Task GetAllDiscountCodesAsync()
        {
            var request = new GetAllDiscountCodesRequest();
            var response = await _client.GetAllDiscountCodesAsync(request);
            foreach (var code in response.Codes)
            {
                Console.WriteLine(code);
            }
        }

        public async Task GenerateDiscountCodesAsync(int count)
        {
            var request = new GenerateDiscountCodesRequest { Count = count };
            try
            {
                var response = await _client.GenerateDiscountCodesAsync(request);
                Console.WriteLine("Generated Discount Codes:");
                foreach (var code in response.Codes)
                {
                    Console.WriteLine(code);
                }
            }
            catch (Grpc.Core.RpcException ex)
            {
                Console.WriteLine($"gRPC error: {ex.Status.Detail}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public void Dispose()
        {
            _channel?.Dispose();
        }
    }
}
