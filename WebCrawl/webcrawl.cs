using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;

namespace WebCrawl
{
    class webcrawl
    {
        //private static string[] url; //Array for user input file
        private static HttpWebRequest urlRequest; // webrequest
       // private static HttpStatusCode responseStatusCode;
        private static List<String> errors = new List<string>(); //List of errors - it's a list because I don't know how many errors there may be
        private static string[] errorArray; //Array of errors - I dump the list into an array before writing  errors.txt
        private static int x=0; //This is a counter that increments independently of i - just keeps the error file neat, as it increments the list by 1 every time I write an error

        static void Main(string[] args)
        {

            string[] url;
            url = ReadArray(); //Reads file.txt and creates an array of Urls 
                       
           
            for (int i=0; i<url.Length;i++) //start to loop through the array
            {
                
                try
                {
                     //If response is 200 OK, continue silently
                    urlRequest = (HttpWebRequest)WebRequest.Create(url[i]);
                    urlRequest.Proxy = null; //Set null proxy to speed up request
                    urlRequest.Timeout = 2000; //2-second timeout
                    
                    HttpWebResponse response = (HttpWebResponse)urlRequest.GetResponse();
                   // responseStatusCode = response.StatusCode; // In case I want to do something with the response
                  
                }

                catch (WebException errorCode)
                {
                    if (errorCode.Response == null) //Catch Null Exception - this is usually a timeout
                        {                                         
                        errors.Add("URL " + url[i] + " " + errorCode.Message);//Write error to the list
                         x++;//increment list counter
                        }
                    else //Catch any other error - 404, 403, etc
                    { 
                   // responseStatusCode = ((HttpWebResponse)errorCode.Response).StatusCode;//Leaving this in case I want to do something later
                    errors.Add("URL " + url[i] + " " + errorCode.Message);//Write error to the list
                    x++;//increment list counter
                     }
                  

                }
            }

            errorArray = errors.ToArray(); //Convert the list to an array - this helps with writing the error file
            WriteErrorFile();
            Console.WriteLine("Done! Check errors.txt for errors.");

        }

//Methods to Read from the file and write the error file follow

        static string[] ReadArray()
        {
            string[] array = File.ReadLines("file.txt").ToArray();
            return array;

        }

        static void WriteErrorFile()
        {
            System.IO.File.WriteAllLines("errors.txt", errorArray);
        }


    }
}
