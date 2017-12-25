using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HtmlParser
{
    internal class Program
    {
        public static void Main(string[] args)
        {

            int depth = 1;
            
            string link = "https://github.com/BaybaraEvgeny/C-Sharp-Homework/blob/master/Parallel/Hashes.cs";

            Console.WriteLine("Enter the link: ");
            try
            {
                //link = Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Ok, using default");
            }
            
            Page mainPage = new Page(link);

            Page[] pages = new[] {mainPage};

            Stopwatch timer = Stopwatch.StartNew();
            pages = ParseRecurrently(pages, depth, 0).Result;
            
            Console.WriteLine("Time of parsing: " + timer.Elapsed);
            Console.WriteLine("Total number of pages: " + pages.Length);
            
            for (int i = 0; i < pages.Length; i++)
            {
                Console.WriteLine("Link: " + pages[i].GetName() + " |>> symbol count: " + pages[i].GetContent().Result.Length);
            }

            Console.ReadKey();

        }

        static async Task<Page[]> ParsePage(Page page)
        {
            Regex reg = new Regex(@"<a.*? href=""(?<url>((https?:\/\/)([\w\.]+)\.([a-z]{2,6})(\/[\w\.]*)*\/?))");

            string htmlCode = await page.GetContent();

            MatchCollection m = reg.Matches(htmlCode);
            
            Page[] pages = new Page[m.Count];

            for (int i = 0; i < pages.Length; i++)
            {
                pages[i] = new Page(m[i].Groups["url"].Value);
            }

            return pages;

        }

        static async Task<Page[]> ParseRecurrently(Page[] pages, int depth, int startPoint)
        {

            Page[] newPages = pages;
            
            for (int i = startPoint; i < pages.Length; i++)
            {
                newPages = newPages.Concat(await ParsePage(pages[i])).ToArray();
            }

            if (depth == 0) return newPages;

            return ParseRecurrently(newPages, depth - 1, pages.Length).Result;

        }
        
    }
}