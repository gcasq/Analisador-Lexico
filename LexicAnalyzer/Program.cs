using System;
using System.Runtime.InteropServices;
using LexicAnalyzer.Utils;
using static LexicAnalyzer.Enums;

namespace MyApp // Note: actual namespace depends on the project name.
{
    public class Program {
        public const int MAX_CONSTS = 1000;

        public static List<string> SecondaryToken { get; set; } = new();

        [StructLayout(LayoutKind.Explicit)]
        public struct t_const {
            [FieldOffset(0)] public byte Type; // 0 - char, 1 - int, 2 - string

            [FieldOffset(1)] public char cVal;
            [FieldOffset(1)] public int nVal;
            [FieldOffset(1)] public string sVal;
        };

        public static t_const[] VConsts = new t_const[MAX_CONSTS];
        public static int nNumConsts = 0;


        public static void Main(string[] args) {



        }

        public static int SearchName(string name) { 
            if(SecondaryToken.Contains(name)) return SecondaryToken.IndexOf(name);
            SecondaryToken.Add(name);
            return SecondaryToken.Count-1;
        }

        public static bool IsReservedKeyword(string keyword) =>
            Utils.SearchKeyword(keyword) != t_token.UNKNOWN;

        public static int AddCharConst(char c) {
            VConsts[nNumConsts] = new t_const {
                Type = 0,
                cVal = c
            };
            nNumConsts++;
            return nNumConsts - 1;
        }

        public static int AddIntConst(int n) {
            VConsts[nNumConsts] = new t_const {
                Type = 1,
                nVal = n
            };
            nNumConsts++;
            return nNumConsts - 1;
        }

        public static int AddStringConst(string s) {
            VConsts[nNumConsts] = new t_const {
                Type = 2,
                sVal = s
            };
            nNumConsts++;
            return nNumConsts - 1;
        }

        public static char GetCharConst(int n) => VConsts[n].cVal;
        public static int GetIntConst(int n) => VConsts[n].nVal;
        public static string GetStringConst(int n) => VConsts[n].sVal;

    }
}