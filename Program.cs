using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace KeywordPart2
{
    class Program
    {
        private static Article article = new Article() { SentenceList = new List<Sentence>(), KeywordList = new List<Keyword>(), WordList = new List<Word>() };
        private static Sentence sentence = new Sentence();
        private static Keyword keyword = new Keyword();
        private static List<string> used_words = new List<string>();
        private static Word word = new Word();

        static void Main(string[] args)
        {
            foreach (var v in Reader.ReadFromFile())
            {
                article.raw_text += v;
            }

            // Prints extracted text
            Console.WriteLine(article.raw_text);
            
            for (int i = 0; i < article.raw_text.Length - 1; ++i)
            {
                // If we detect the end of a sentence
                if (article.raw_text[i] == '.' && article.raw_text[i+1] == ' ')
                {
                    // Save the sentence to a list
                    // Clear the sentence object
                    article.SentenceList.Add(new Sentence { contents = sentence.contents + '.' });
                    sentence.contents = "";
                }
                else
                {
                    // Build the sentence
                    sentence.contents += article.raw_text[i];
                }
            }

            // Prints each gathered sentence
            foreach(var s in article.SentenceList)
            {
                Console.WriteLine(s.contents);
            }

            for (int i = 0; i < article.raw_text.Length - 1; ++i)
            {
                if (word.ContainsPunctuation(article.raw_text[i]))
                {
                    if (!used_words.Contains(word.contents) && !word.isJunkWord(word.contents))
                    {
                        used_words.Add(word.contents);
                        article.KeywordList.Add(new Keyword { contents = word.contents });
                    }
                    article.WordList.Add(new Word { contents = word.contents });
                    word.contents = "";
                }
                else
                {
                    // Build the word
                    word.contents += article.raw_text[i];
                }
            }
            
            foreach (var key in article.KeywordList)
            {
                foreach (var word in article.WordList)
                {
                    if (word.contents == key.contents)
                    {
                        key.count += 1;
                    }
                }
            }

            // Prints each gathered keyword
            foreach (var w in article.KeywordList)
            {
                Console.WriteLine(w.contents + " : " + w.count);
            }

            foreach (var s in article.SentenceList)
            {
                foreach (var k in article.KeywordList)
                {
                    if (s.contents.Contains(k.contents))
                    {
                        s.score += k.count;
                    }
                }

                article.average_score += s.score;
            }

            article.average_score = article.average_score / article.SentenceList.Count;
            
            foreach (var s in article.SentenceList)
            {
                if (s.score > article.average_score)
                {
                    article.summary += s.contents;
                }
            }

            Console.WriteLine("\n\nSummary:\n" + article.summary);
            Reader.WriteListToText("", "tesla_summary", article.summary);
            Console.ReadLine();
        }
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

