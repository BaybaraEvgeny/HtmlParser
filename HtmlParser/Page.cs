using System;
using System.Net;
using System.Threading.Tasks;

namespace HtmlParser
{
    public class Page
    {
        private readonly Uri uri;

        public Page(string link)
        {
            this.uri = new Uri(link);
        }

        public string GetName()
        {
            return uri.ToString();
        }

        public async Task<string> GetContent()
        {
            string htmlCode = "";
            
            WebClient client = new WebClient();

            try
            {
                htmlCode = await client.DownloadStringTaskAsync(uri);
            }
            catch (Exception e)
            {
                Console.Write("(!) Exception >> {0} {1}",  e.Message, uri);
                Console.WriteLine();
            }

            return htmlCode;
        }
        
    }
}