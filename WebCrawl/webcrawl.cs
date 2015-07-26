﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.IO;

namespace WebCrawl
{
    class webcrawl
    {
       
       static void Main(string[] args)
        {
            //Variables for this method
            string[] url; //Array for user input file
            HttpWebRequest urlRequest;  // webrequest
            int x = 0; //This is a counter that increments independently of i - just keeps the error file neat, as it increments the list by 1 every time I write an error
            List<String> errors = new List<string>(); //List of errors - it's a list because I don't know how many errors there may be
            string[] errorArray; //Array of errors - I dump the list into an array before writing  errors.txt
            string filename;

            if (args.Length==0)
              {
               Console.Write("Please enter the filename: ");
               filename = Console.ReadLine();               
              }
            else
            { filename = args[0]; }
           
            url = ReadArray(filename); //Reads file.txt and creates an array of Urls 

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
                catch (Exception errorCode)
                {
                    errors.Add("URL " + url[i] + " " + errorCode.Message);
                    x++;
                }
            }


            errorArray = errors.ToArray(); //Convert the list to an array - this helps with writing the error file
            WriteErrorFile(errorArray);
            Console.WriteLine("Done! Check errors.txt for errors.");
            
        }

//Methods to Read from the file and write the error file follow

        static string[] ReadArray(string fileName)
        {
            try
            {
                string[] array = File.ReadLines(fileName).ToArray();
                return array;
            }
            catch (FileNotFoundException error)
            {
                Console.Write(error.Message);
                System.Environment.Exit(0);
                return null;

            }


        }

        static void WriteErrorFile(string[] errorList)
        {
            System.IO.File.WriteAllLines("errors.txt", errorList);
        }


    }



}
