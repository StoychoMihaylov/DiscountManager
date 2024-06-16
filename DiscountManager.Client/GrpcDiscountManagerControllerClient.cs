namespace DiscountManager.Client
{
    using DiscountManagerController.Grpc;
    using Grpc.Net.Client;

    public class GrpcDiscountManagerControllerClient
    {
        private readonly Uri uri;
        public GrpcDiscountManagerControllerClient(Uri uri)
        {
            this.uri = uri;
        }

        public DiscountManagerService.DiscountManagerServiceClient OpenGrpcDiscountManagerServerConnection()
        {
            // Ensure the switch is set to allow HTTP/2 without TLS (for local testing purposes).
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            using var channel = GrpcChannel.ForAddress(uri);
            return new DiscountManagerService.DiscountManagerServiceClient(channel);
        }
    }
}
