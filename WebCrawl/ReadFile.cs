// Objects of this class read and write files.

using System;
using System.IO;
using System.Linq;

namespace WebCrawl
{
    internal class ReadFile

    {
        private string _fileName; //

        public string Filename
        {
            set
            {
                _fileName = value;
            }
        }

        public string[] Read()
        {
            try
            {
                string[] array = File.ReadLines(_fileName).ToArray();
                return array;
            }
            catch (FileNotFoundException error)
            {
                Console.Write(error.Message);
                System.Environment.Exit(1);
                return null;
            }
        }

        public void Message()
        {
            Console.WriteLine("Message");
        }

        public void Write(string[] errorList)
        {
            System.IO.File.WriteAllLines("errors.txt", errorList);
        }
    }
}