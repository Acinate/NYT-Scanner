using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeywordPart2
{
    class Program
    {
        static void Main(string[] args)
        {
            // READS LINES FROM TEXT FILE, SAVES TO STRING
            string paragraphText = "";
            foreach (var v in Reader.ReadFromFile())
            {
                paragraphText += v;
            }

            // DEBUG
            Console.WriteLine(paragraphText);

            // BREAKS PARAGRAPH TEXT STRING INTO LIST OF SENTENCES
            List<string> sentences = new List<string>();
            string sentence = "";
            for (int i = 0; i < paragraphText.Length; i++)
            {
                if (paragraphText[i] == '.' && paragraphText[i + 1] == ' ')
                {
                    sentence += paragraphText[i];
                    sentences.Add(sentence);
                    sentence = "";
                } else
                {
                    sentence += paragraphText[i];
                }
            }

            // FIXES SENTENCES
            sentences = Word.FixSentences(sentences);

            // DEBUG
            foreach (var s in sentences)
            {
                // Console.WriteLine(s + "\n");
            }

            // GRABS KEYWORDS -- DOES NOT ADD JUNK KEYWORDS
            List<string> paragraphs = new List<string>();
            List<string> keywords = new List<string>();
            string keyword = "";
            foreach (string s in sentences)
            {
                foreach (char c in s)
                {
                    if (!char.IsWhiteSpace(c) && Word.hasPunctionation(c))
                    {
                        keyword += c;
                    }
                    else
                    {
                        if (keyword != "" && !keyword.Contains('.'))
                        {
                            if (keyword.Contains("’s"))
                                keyword = Word.removeApostrophy(keyword);
                            if (!Word.isJunkWord(keyword))
                            {
                                keywords.Add(keyword);
                            }
                        }
                        keyword = "";
                    }
                }
            }

            // COUNTS THE OCCURENCES OF EACH KEYWORD
            var numKeywords = new List<int>();
            foreach (string s in keywords)
            {

                int count = 0;
                foreach (string z in keywords)
                {
                    if (z == s) 
                    {

                        count++;
                    }
                }
                numKeywords.Add(count);
            }

            // CALCULATIONS FOR DETECTING AVERAGE KEYWORD OCCURENCE
            int sum = 0, average = 0, average75 = 0;
            foreach (int i in numKeywords)
            {
                sum += numKeywords[i];
            }

            // AVERAGE KEYWORD OCCURENCE
            average = sum / (numKeywords.Count() / 2);

            // AVERAGE + AVERAGE / 2 CALCULATION
            average75 = average;

            // && !keywords.Contains(keyword)
            List<string> goodKeyWords = new List<string>();
            List<int> sentenceScore = new List<int>();
            for (int i = 0; i < sentences.Count; i++)
            {
                int score = 0;
                sentenceScore.Add(score);
                for (int j = 0; j < numKeywords.Count; j++)
                {
                    if (numKeywords[j] >= average75)
                    {
                        if (!goodKeyWords.Contains(keywords[j]))
                            goodKeyWords.Add(keywords[j]);
                        if (sentences[i].Contains(keywords[j]))
                        {
                            sentenceScore[i] += 1;
                        }
                    }
                }
            }

            // CALCULATIONS FOR DETECTING AVERAGE KEYWORD OCCURENCE
            foreach (int i in numKeywords)
            {
                sum += i;
            }

            // AVERAGE KEYWORD OCCURENCE
            average = sum / (numKeywords.Count() / 8);

            // AVERAGE + AVERAGE / 2 CALCULATION
            average75 = average + (average / 3);

            string result = "";
            List<string> summarySentences = new List<string>();
            for (int i = 0; i < sentenceScore.Count; i++)
            {
                if (sentenceScore[i] > average75)
                {
                    summarySentences.Add(sentences[i]);
                }
            }

            foreach (string ss in summarySentences)
            {
                result += ss + " ";
            }

            result += "\n\nKeywords Used:\n";

            foreach (string gkw in goodKeyWords)
            {
                result += gkw + "\n";
            }

            Console.WriteLine("\n\nSummary:\n" + result);
            Reader.WriteListToText("", "tesla_summary", result);
            Console.ReadLine();
        }
    }

    class Word
    {
        public static string keyword { get; set; }

        //public bool isKeyword(List<String> Sentences)
        //{
        //    for (int i = 0; i < Sentences.Count(); i++)
        //    {
        //        string sent = "";
        //    }
        //}

        public string contents { get; set; }

        public static bool isJunkWord(string keyword)
        {
            keyword = keyword.ToLower();
            if (keyword == "the" || keyword == "that" || keyword == "or" || keyword == "is" || keyword == "in" || keyword == "to" || keyword == "a" || keyword == "an" || keyword == "has" || keyword == "of" || keyword == "its" || keyword == "and" || keyword == "but" || keyword == "as" || keyword == "it" || keyword == "be" || keyword == "which" ||
                keyword == "there" || keyword == "for" || keyword == "he" || keyword == "his" || keyword == "hers" || keyword == "her" || keyword == "by" || keyword == "on" || keyword == "than" || keyword == "i")
                return true;
            else
                return false;
        }

        public static string removeApostrophy(string keyword)
        {
            return keyword.Replace("’s","");
        }

        public static bool hasPunctionation(char c)
        {
            if (c != ',' && c != '-' && c != '—' && c != ':' && c != '(' && c != ')' && c != '”' && c != '“' && c != '?')
                return true;
            else
                return false;
        }

        public static List<string> FixSentences(List<string> sentences)
        {
            for (int i = 0; i < sentences.Count; i++)
            {
                // REMOVE SPACE AT START OF SENTENCE
                if (sentences[i].StartsWith(" "))
                {
                    sentences[i] = sentences[i].Remove(0,1);
                }

                // FIX NAME PREFIX
                if (sentences[i].Contains("Mr.") || sentences[i].Contains("Mrs.") || sentences[i].Contains("Dr.") || sentences[i].Contains("Prof."))
                {
                    sentences[i] += sentences[i + 1];
                    sentences.RemoveAt(i + 1);
                }

                // DETECTS ABBREVIATED WORDS (CAN BE SCALED => ADJUST LENGTH BASED ON NUMBER OF '.')
                if (sentences[i].Contains(".") && sentences[i].Length < 5)
                {
                    sentences[i + 1] = sentences[i + 1].Insert(0, sentences[i]);
                    sentences.RemoveAt(i);
                }
            }
            return sentences;
        }
    }

    class Sentence
    {
        public string contents { get; set; }
    }

    class Reader
    {
        public static List<string> ReadFromFile()
        {
            List<string> lines = new List<string>();

            string[] line = File.ReadAllLines("paragraph.txt");

            foreach(string l in line)
            {
                if (l == "")
                    lines.Add(" ");
                else
                    lines.Add(l);
            }
            return lines;
        }

        public static void WriteListToText(string fileUrl, string fileName, string fileMessage)
        {
            StreamWriter sw = new StreamWriter(fileUrl + fileName + ".txt");
            sw.Write(fileMessage);
            sw.Close();
        }
    }
}

