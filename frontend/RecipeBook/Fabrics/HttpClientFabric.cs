using System.Net.Http;

namespace RecipeBook.Fabrics
{
    public class HttpClientFabric : IHttpClientFabric
    {
        public HttpClient GetClientWithDisabledCerts()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            return new HttpClient(clientHandler);
        }
    }
}