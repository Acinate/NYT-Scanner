using System;
using System.Collections.Generic;

namespace KeywordPart2
{
    public class Article
    {
        public List<Sentence> SentenceList { get; set; }
        public List<Word> WordList { get; set; }
        public List<Keyword> KeywordList { get; set; }
        public string raw_text { get; set; }
        public int average_score { get; set; }
        public string summary { get; set; }
    }

    public class Sentence
    {
        public string contents { get; set; }
        public int score { get; set; }
        public static List<string> FixSentences(List<string> sentences)
        {
            for (int i = 0; i < sentences.Count; i++)
            {
                // REMOVE SPACE AT START OF SENTENCE
                try
                {
                    if (sentences[i].StartsWith(" "))
                    {
                        sentences[i] = sentences[i].Remove(0, 1);
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
                catch (Exception)
                {

                }
            }
            return sentences;
        }
    }

    public class Word
    {
        public string contents { get; set; }
        public bool ContainsPunctuation(char c)
        {
            if (c == ' ' || c == '.' || c == ',' || c == '-' || c == '_' || c == ';' || c == ':')
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool isJunkWord(string keyword)
        {
            keyword = keyword.ToLower();
            if (keyword == "the" || keyword == "that" || keyword == "or" || keyword == "is" || keyword == "in" || keyword == "to" || keyword == "a" || keyword == "an" || keyword == "has" || keyword == "of" || keyword == "its" || keyword == "and" || keyword == "but" || keyword == "as" || keyword == "it" || keyword == "be" || keyword == "which" ||
                keyword == "there" || keyword == "for" || keyword == "he" || keyword == "his" || keyword == "hers" || keyword == "her" || keyword == "by" || keyword == "on" || keyword == "than" || keyword == "i" || keyword == "with" || keyword == "us" || keyword == "our" || keyword == "you" || keyword == "can" || keyword == "")
                return true;
            else
                return false;
        }
    }

    public class Keyword
    {
        public int count { get; set; }
        public string contents { get; set; }
        
    }
}
