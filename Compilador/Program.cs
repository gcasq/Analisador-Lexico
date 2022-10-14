using LexicAnalyzer;

namespace Main {
    public class Program {
        public static FileStream? FFile {
            get { return file; }
            set { file = value; }
        }
        static FileStream? file = File.OpenRead("./SeuArquivo.txt");
        public static void Main(string[] args) {
            SyntacticAnalyzer Syntactic = new();

            //LexicAnalyzer.LexicAnalyzer.ExecuteLexicAnalyzer();
            Syntactic.syntacticAnalizer();
        }
    }
}
