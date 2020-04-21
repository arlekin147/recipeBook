using System.Net.Http;

namespace RecipeBook.Fabrics
{
    public interface IHttpClientFabric
    {
         HttpClient GetClientWithDisabledCerts();
    }
}