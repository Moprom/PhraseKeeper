using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;

namespace PhraseKeeper
{
    class PhraseSearcher
    {
        private const int maxPhrases = 10;
        
        private readonly Dictionary<char, int> config;
        private readonly string book;

        private Dictionary<char, int> resourse;
        private List<string> resultPhrases;
        private int maxPhraseLenght;        
        private string currentPhrase;       
        public PhraseSearcher(Dictionary<char, int> config, string book)
        {
            this.book = book;
            this.config = config;

            Init();
            CalcMaxPhraseLength();
        }
        public IEnumerable<string> Generate10LongestPhrases()
        {
            string currentSentense = string.Empty;
            int counter = 0;

            for (int i = 0; i < book.Length; i++)
            {
                if (String.IsNullOrEmpty(currentSentense))
                {
                    if (IsBeginOfSentense(book[i]))
                    {
                        currentSentense = book[i].ToString();
                    }
                    continue;
                }
                
                if (IsBadCharacterForSentense(book[i])||IsSentenseOverLimit(counter))
                {
                    currentSentense = string.Empty;
                    counter = 0;

                    continue;
                }
               
                if (Char.IsLetter(book[i]))
                {
                    counter++;
                }

                currentSentense = String.Concat(currentSentense,book[i]);

                if (IsFinalPunctual(book[i]))
                {
                    ProcessSentense(currentSentense);

                    counter = 0;
                    currentSentense = string.Empty;
                }
            }

            return resultPhrases;
        }
        private void Init()
        {
            resultPhrases = new List<string>();
            currentPhrase = string.Empty;
            resourse = new Dictionary<char, int>();
        }
        private void CalcMaxPhraseLength()
        {
            foreach (var ch in config)
            {
                maxPhraseLenght += ch.Value;
            }
        }
        private void ProcessSentense(string currentSentense)
        {
            if (IsConcatableSentenses(currentSentense.Length))
            {
                if (ReadSentense(String.Concat(currentPhrase, currentSentense)))
                {
                    currentPhrase = String.Concat(currentPhrase, currentSentense);
                    AddSentense(currentPhrase);
                }
                else
                {
                    while (true)
                    {
                        RemoveFirstSentenseFromPhrase();
                        if (ReadSentense(String.Concat(currentPhrase, currentSentense)))
                        {
                            currentPhrase = String.Concat(currentPhrase, currentSentense);
                            AddSentense(currentPhrase);
                            break;
                        }
                        else
                        {
                            if (currentPhrase == string.Empty)
                                break;
                        }
                    }
                }
            }
            else
            {
                if (ReadSentense(currentSentense))
                {
                    currentPhrase = currentSentense;
                    AddSentense(currentPhrase);
                }
            }
        }
        private bool ReadSentense(string sentense)
        {
            UnloadResourse();
            
            sentense = sentense.ToLower();
            foreach(var character in sentense)
            {
                if (Char.IsLetter(character) && resourse.ContainsKey(character))
                {
                    resourse[character] = resourse[character] -1;
                    var count = resourse[character];

                    if (UsedLimitOfChar(character))
                    {                        
                        return false;
                    }
                }
            }
            return true;
        }
        private void UnloadResourse()
        {
            resourse.Clear();
            foreach (var item in config)
            {
                resourse.Add(item.Key, item.Value);
            }
        }
        private bool UsedLimitOfChar(char character)
        {
            if (resourse[character] < 0)
            {
                return true;
            }

            return false;
        }
        private void AddSentense(string sentense)
        {
            if (resultPhrases.Count < maxPhrases)
            {
                InsertSentense(sentense);
            }
            if (resultPhrases.Count == maxPhrases && (resultPhrases[maxPhrases - 1].Length < sentense.Length))
            {
                resultPhrases.Remove(resultPhrases[maxPhrases - 1]);
                InsertSentense(sentense);
            }
        }
        private void InsertSentense(string sentense)
        {
            resultPhrases.Add(sentense);

            for(int i=0; i< resultPhrases.Count-1;i++)
            {
                if (resultPhrases[i].Length < sentense.Length)
                {
                    for(int j = resultPhrases.Count-1; j > i; j--)
                    {
                        resultPhrases[j] = resultPhrases[j - 1];
                    }
                    resultPhrases[i] = sentense;

                    return;
                }
            }
        }
        private void RemoveFirstSentenseFromPhrase()
        {
            for(int i=0; i < currentPhrase.Length; i++)
            {
                if(IsFinalPunctual(currentPhrase[i]))
                {
                    if (i == currentPhrase.Length - 1)
                    {
                        currentPhrase = string.Empty;
                    }
                    else
                    {
                        currentPhrase = currentPhrase.Substring(i + 1, currentPhrase.Length - i - 1);
                    }

                    break;
                }
            }
        }
        private bool IsFinalPunctual(char character)
        {
            if (character == '!' || character == '.' || character == '?')
            {
                return true;
            }
            return false;
        }
        private bool IsBeginOfSentense(char firstChar)
        {
            if (Char.IsLetter(firstChar) && Char.IsUpper(firstChar))
            {
                return true;
            }
            return false;
        }
        private bool IsBadCharacterForSentense(char character)
        {
            if (!Char.IsLetter(character) && !Char.IsPunctuation(character) && !Char.IsWhiteSpace(character) || character == '\r' || character == '\n')
            { 
                return true;
            }
            return false;
        }
        private bool IsSentenseOverLimit(int size)
        {
            if(size>maxPhraseLenght)
            {
                return true;
            }
            return false;
        }
        private bool IsConcatableSentenses(int additionSentenseLength)
        {
            if (currentPhrase.Length + additionSentenseLength < maxPhraseLenght && !String.IsNullOrEmpty(currentPhrase))
            {
                return true;
            }
            return false;
        }
    }
}
