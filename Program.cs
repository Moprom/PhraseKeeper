using System;
using System.Collections.Generic;

namespace PhraseKeeper
{
    class Program
    {
        static void Main(string[] args)
        {
            string book = IO.LoadBookFile("C:\\Users\\Kimakim\\Downloads\\small_prince");
            Dictionary<char,int> config = IO.LoadConfigurationFile("C:\\Users\\Kimakim\\Downloads\\config");
            PhraseSearcher phraseSearcher = new PhraseSearcher(config, book);
            IEnumerable<string> phrases = phraseSearcher.Generate10LongestPhrases();
            IO.SavePhrasesToFile("C:\\Users\\Kimakim\\Downloads\\result.txt", phrases);
        }
    }
}
