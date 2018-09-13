
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;

namespace LyricAnalizer
{
    class Program
    {
        static void Main(string[] args)
        {
            var lyrics = File.ReadAllText(@"C:\test\lyrics.txt");
            var result = FullAnalysis(lyrics);
            var wordCount = result["Word Count"];
            var pronounCount = result["Pronoun Total (excluding 'it')"];
            foreach (KeyValuePair<string, int> entry in result)
            {
                double x = (entry.Value * 100 / wordCount);
                double y = (entry.Value * 100 / pronounCount);
                if (entry.Key == "Word Count")
                {
                    Console.WriteLine(entry.ToString());
                }
                else if (entry.Key == "Pronoun Total (excluding 'it')" || entry.Key == "Third Person Generic Singular")
                {
                    Console.WriteLine(entry.ToString() + " --- " + x + "% of lyrics");
                }
                else
                    Console.WriteLine(entry.ToString() + " --- " + x + "% of lyrics, " + y + "% of pronouns");
            }
            Console.ReadLine();
        }

        static Dictionary<string, int> FullAnalysis(string lyricString)
        {
            var lyricWords = ParseLyrics(lyricString);
            var wordCount = lyricWords.Count();
            Dictionary<string, int> faDict = new Dictionary<string, int>();
            string wc = "Word Count";
            string sfps = "First Person Singular";
            string sfpp = "First Person Plural";
            string ssp = "Second Person";
            string stpgs = "Third Person Generic Singular";
            string stpgp = "Third Person Generic Plural";
            string stpm = "Third Person Masculine";
            string stpf = "Third Person Feminine";
            string sfpt = "First Person Total";
            string sspt = "Second Person Total";
            string stpgt = "Third Person Gendered Total";
            string stpei = "Third Person Total (excluding 'it')";
            string stpt = "Third Person Total";
            int fps = AnalyzeFirstPersonSingular(lyricWords);
            int fpp = AnalyzeFirstPersonPlural(lyricWords);
            int sp = AnalyzeSecondPerson(lyricWords);
            int tpgs = AnalyzeThirdPersonGenericSingular(lyricWords);
            int tpgp = AnalyzeThirdPersonGenericPlural(lyricWords);
            int tpm = AnalyzeThirdPersonMasculine(lyricWords);
            int tpf = AnalyzeThirdPersonFeminine(lyricWords);
            faDict.Add(wc, wordCount);
            faDict.Add("Pronoun Total (excluding 'it')", (fps + fpp + sp + tpm + tpf + tpgp));
            faDict.Add(sfps, fps);
            faDict.Add(sfpp, fpp);
            faDict.Add(ssp, sp);
            faDict.Add(stpgs, tpgs);
            faDict.Add(stpgp, tpgp);
            faDict.Add(stpm, tpm);
            faDict.Add(stpf, tpf);
            faDict.Add(sfpt, (fps + fpp));
            faDict.Add(sspt, sp);
            faDict.Add(stpgt, (tpm + tpf));
            faDict.Add(stpei, (tpm + tpf + tpgp));
            faDict.Add(stpt, (tpm + tpf + tpgp + tpgs));
            return faDict;
        }

        static List<string> ParseLyrics(string lyrics)
        {
            MatchCollection lyricMC = Regex.Matches(lyrics, @"[A-z']+");
            List<string> wordList = new List<string>();
            foreach (Match word in lyricMC)
            {
                wordList.Add(word.ToString().ToLower());
            }
            return wordList;
        }

        static int AnalyzeFirstPersonSingular(List<string> lyrics)
        {
            int fpsCount = 0;
            string[] firstPersonSingular = { "I", "me", "mine", "I'll", "I've", "I'd", "I'm", "my" };
            foreach (string word in lyrics)
            {
                if (Array.IndexOf(firstPersonSingular, word) >= 0)
                {
                    fpsCount++;
                }
            }
            return fpsCount;
        }

        static int AnalyzeFirstPersonPlural(List<string> lyrics)
        {
            int fppCount = 0;
            string[] firstPersonPlural = { "we", "our", "ours", "we'll", "we'd", "we've", "we're" };
            foreach (string word in lyrics)
            {
                if (Array.IndexOf(firstPersonPlural, word) >= 0)
                {
                    fppCount++;
                }
            }
            return fppCount;
        }

        static int AnalyzeSecondPerson(List<string> lyrics)
        {
            int spCount = 0;
            string[] secondPerson = { "you", "your", "yours", "you're", "you'll", "you'd", "you've", "y'all", "ya", "thee", "thy", "thou" };
            foreach (string word in lyrics)
            {
                if (Array.IndexOf(secondPerson, word) >= 0)
                {
                    spCount++;
                }
            }
            return spCount;
        }

        static int AnalyzeThirdPersonGenericSingular(List<string> lyrics)
        {
            int tpgsCount = 0;
            string[] thirdPersonGenericSingular = { "it", "its", "it's", "it'll", "it'd", "'tis" };
            foreach (string word in lyrics)
            {
                if (Array.IndexOf(thirdPersonGenericSingular, word) >= 0)
                {
                    tpgsCount++;
                }
            }
            return tpgsCount;
        }

        static int AnalyzeThirdPersonGenericPlural(List<string> lyrics)
        {
            int tpgpCount = 0;
            string[] thirdPersonGenericPlural = { "they", "them", "their", "they'd", "they'll", "they've", "they're" };
            foreach (string word in lyrics)
            {
                if (Array.IndexOf(thirdPersonGenericPlural, word) >= 0)
                {
                    tpgpCount++;
                }
            }
            return tpgpCount;
        }

        static int AnalyzeThirdPersonFeminine(List<string> lyrics)
        {
            int tpfCount = 0;
            string[] thirdPersonFeminine = { "she", "her", "hers", "she'll", "she'd", "she's" };
            foreach (string word in lyrics)
            {
                if (Array.IndexOf(thirdPersonFeminine, word) >= 0)
                {
                    tpfCount++;
                }
            }
            return tpfCount;
        }

        static int AnalyzeThirdPersonMasculine(List<string> lyrics)
        {
            int tpmCount = 0;
            string[] thirdPersonMasculine = { "he", "him", "his", "he'll", "he'd", "he's" };
            foreach (string word in lyrics)
            {
                if (Array.IndexOf(thirdPersonMasculine, word) >= 0)
                {
                    tpmCount++;
                }
            }
            return tpmCount;
        }
    }
}