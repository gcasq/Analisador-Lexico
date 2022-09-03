using System;
using LexicAnalyzer.Utils;
using static LexicAnalyzer.Enums;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program {

        public static List<string> SecondaryToken { get; set; } = new();

        

        public static void Main(string[] args) {



        }

        public static int SearchName(string name) { 
            if(SecondaryToken.Contains(name)) return SecondaryToken.IndexOf(name);
            SecondaryToken.Add(name);
            return SecondaryToken.Count;
        }

        public static bool IsReservedKeyword(string keyword) =>
            Utils.SearchKeyword(keyword) != t_token.UNKNOWN;
    }
}