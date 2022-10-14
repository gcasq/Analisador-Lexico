using static LexicAnalyzer.LexicAnalyzer;
using static LexicAnalyzer.SintaticAnalyzer;
using System.IO;
using System.Data.Common;
using static LexicAnalyzer.utils.Enums;

namespace Main {
    public class Program {
        public static FileStream? FFile {
            get { return file; }
            set { file = value; }
        }
        static FileStream? file = File.OpenRead("./SeuArquivo.txt");
        public static void Main(string[] args) {
            //ExecuteLexicAnalyzer();
            synteticAnalizer();
        }
    }
}
