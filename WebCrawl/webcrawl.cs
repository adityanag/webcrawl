using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace WebCrawl
{
    class webcrawl
    {
        private static string url;
        private static HttpWebRequest webRequest;
        private static HttpWebResponse response;
        private static HttpStatusCode responseStatusCode;
        private static string[] lines;


        static void Main(string[] args)
        {

            ReadArray();

            Console.WriteLine("Enter the url you want to test");
            url = Console.ReadLine();
            webRequest = (HttpWebRequest)WebRequest.Create(url);

            try
            {
               
                HttpWebResponse response = (HttpWebResponse)webRequest.GetResponse();
                responseStatusCode = response.StatusCode;
                Console.Write((int)response.StatusCode);
                Console.ReadKey();

            }

            catch (WebException errorCode)
            {
                responseStatusCode = ((HttpWebResponse)errorCode.Response).StatusCode;
                Console.Write((int)responseStatusCode);
                Console.ReadKey();

            }


            
        }

        static void ReadArray()
        {
            lines = File.ReadLines("file.txt").ToArray();
            Console.ReadKey();
        }



    }
}
