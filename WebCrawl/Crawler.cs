using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace WebCrawl
{
    internal class Crawler
    {
        private static void Main(string[] args)
        {
            System.Net.ServicePointManager.Expect100Continue = false;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.DefaultConnectionLimit = 100;
            //Variables for this method
            string[] url; //Array for user input file
            List<String> errors = new List<string>(); //List of errors - it's a list because I don't know how many errors there may be
            string[] errorArray; //Array of errors - I dump the list into an array before writing  errors.txt
            string filename;
            ReadFile reader = new ReadFile(); //Object of class ReadFile - the readfile class reads and writes to file. I should probably name it better
            //Read File
            if (args.Length == 0)
            {
                Console.Write("Please enter the filename: ");
                filename = Console.ReadLine();
            }
            else
            { filename = args[0]; }
            reader.Filename = filename; //This sends the filename that I just read to the object's private property _filename
            url = reader.Read(); //This reads the file and returns the array.

            for (int i = 0; i < url.Length; i++) //start to loop through the array
            {
                bool test = IsDomainAlive(url[i], 1); //Test with TCPclient to see if the domain exists at all
                if (test) //if it exists, then check for the rest of the URI path
                {
                    errors.Add(url[i] + " " + testURL(url[i]));
                }
                else
                { errors.Add(url[i] + " " + "No such domain exists"); }
            }

            errorArray = errors.ToArray(); //Convert the list to an array - this helps with writing the error file
            reader.Write(errorArray);

            Console.WriteLine("Done! Check errors.txt for errors.");
        }

        private static bool IsDomainAlive(string aDomain, int aTimeoutSeconds)
        //This method uses TCPClient to check the validity of the domain name and returns true if domain exists, and false if it doesn't
        {
            System.Uri uri = new Uri(aDomain);
            string uriWithoutScheme = uri.Host.TrimEnd('/');
            try
            {
                using (TcpClient client = new TcpClient())
                {
                    var result = client.BeginConnect(uriWithoutScheme, 80, null, null);

                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(aTimeoutSeconds));

                    if (!success)
                    {
                        Console.Write(aDomain + " ---- No such domain exists\n");
                        return false;
                    }

                    // we have connected
                    client.EndConnect(result);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.Write(aDomain + " ---- " + ex.Message + "\n");
            }
            return false;
        }

        private static string testURL(string v)
        //This method tests the entire URL and returns the status code
        {
            try
            {
                
                HttpWebRequest urlRequest = (HttpWebRequest)WebRequest.Create(v);
                urlRequest.Proxy = null; //Set null proxy to speed up request
                urlRequest.Timeout = 5000; //5 second timeout
                urlRequest.Method = "HEAD";
                HttpWebResponse response = (HttpWebResponse)urlRequest.GetResponse();
                Console.WriteLine(v + " ---- " + response.StatusCode.ToString());
                response.Close();
                return response.StatusCode.ToString();
            }
            catch (Exception exception)
            {
                Console.WriteLine(v + " ---- " + exception.Message);
                return exception.Message;
            }
        }
    }
}