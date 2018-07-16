using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace FMASolutionsCore.DataServices.WebHelper
{
    public class WebCrawler
    {
        public static string HTMLResultFromGETRequest(string url)
        {
            return new HttpClient().GetStringAsync(url).Result;
        }
        public static string APIPostCall(string uri, string actionName, string bodyText)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new System.Uri(uri);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
              new MediaTypeWithQualityHeaderValue(Web.C.appJson));

            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = new System.Uri(uri + actionName),
                Content = new StringContent("\"" + bodyText + "\"", Encoding.UTF8, Web.C.appJson),
                Method = HttpMethod.Post
            };
            string message = client.SendAsync(request).Result.Content.ReadAsStringAsync().Result;
            return message;
        }
    }
}
