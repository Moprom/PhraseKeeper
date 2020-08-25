using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace PhraseKeeper
{
    static class IO
    {
        public static string LoadBookFile(string file)
        {
            string fileToString;

            try
            {
                StreamReader streamReader = new StreamReader($"{file}.txt");
                fileToString = streamReader.ReadToEnd();
                streamReader.Close();
            }
            catch
            {
                Console.WriteLine("Error: Bad way to book file");
                fileToString = string.Empty;
            }          

            return fileToString;
        }    
        public static Dictionary<char, int> LoadConfigurationFile(string file)
        {
            Dictionary<char, int> config= new Dictionary<char, int>();

            try
            {
                StreamReader streamReader = new StreamReader($"{file}.txt");
                string line = streamReader.ReadLine();

                while (line != null)
                {
                    var splitedLine = line.Split(" ");

                    if (splitedLine[1].All(char.IsDigit))
                    {
                        config.Add(Convert.ToChar(splitedLine[0]), Convert.ToInt32(splitedLine[1]));
                    }
                    else
                    {
                        Console.WriteLine($"{line} have error composition");
                    }
                    line = streamReader.ReadLine();
                }
                streamReader.Close();
            }
            catch
            {
                Console.WriteLine("Error: Bad way to config file");                
            }

            return config;
        }

        public static void SavePhrasesToFile(string fileWay, IEnumerable<string> phrases)
        {
            try
            {
                StreamWriter streamWriter = new StreamWriter(fileWay);

                foreach (var phrase in phrases)
                {
                    streamWriter.WriteLine(phrase);
                }
                streamWriter.Close();
            }
            catch
            {
                Console.WriteLine("Error: Bad way to result file");
            }
        }
    }
}
