using System;
using static LexicAnalyzer.utils.Enums;
using static LexicAnalyzer.LexicAnalyzer;
namespace LexicAnalyzer {
    public class SintaticAnalyzer {
        public static void synteticAnalizer() {
            string actionPath = "C:\\Users\\harll\\Documents\\workspace\\Analisador-Lexico\\LexicAnalyzer\\action_table.csv";
            string lenLeftPath = "C:\\Users\\harll\\Documents\\workspace\\Analisador-Lexico\\LexicAnalyzer\\len_left_table.csv";
            List<List<string>> lenLeftTable = CreateLenLeftTable(lenLeftPath);
            List<List<string>> actionTable = CreateActionTable(actionPath);
            int finalState = 1;
            int q = 0;
            Stack<int> stateStack = new Stack<int>();
            stateStack.Push(q);
            nextChar = readChar();
            t_token a = nextToken();
            do {

                int aPos = convertToPosition(a);
                int p = 0;
                Int32.TryParse(actionTable[q + 1][aPos], out p);
                if (IsShift(p)) {
                    stateStack.Push(p);
                    a = nextToken();
                } else if (IsReduction(p)) {
                    int r = Rule(p);
                    int len = 0;
                    Int32.TryParse(lenLeftTable[r][0], out len);
                    for (int i = 0; i < len; i++) {
                        stateStack.Pop();
                    }
                    int tokenPos = convertToToken(lenLeftTable[r][1], actionTable);
                    int state = 0;
                    Int32.TryParse(actionTable[stateStack.Peek()+1][tokenPos-1], out state);
                    stateStack.Push(state);
                } else {
                    SyntaxError();
                }
                q = stateStack.Peek();
            } while (q != finalState);
        }

        public static bool IsShift(int p) {
            return p > 0;
        }

        public static bool IsReduction(int p) {
            return p < 0;
        }

        public static int Rule(int p) {
            return -p;
        }

        public static void SyntaxError() {
            Console.WriteLine("Syntax Error");
        }

        public static int convertToPosition(t_token token) {
            t_token[] actionHeader = {t_token.NOT, t_token.NOT_EQUAL, t_token.AND, t_token.LEFT_PARENTHESIS, t_token.RIGHT_PARENTHESIS, t_token.TIMES, t_token.PLUS, t_token.PLUS_PLUS, t_token.COMMA, t_token.MINUS, t_token.MINUS_MINUS, t_token.DOT, t_token.DIVIDE, t_token.COLON, t_token.SEMI_COLON, t_token.LESS_THAN, t_token.LESS_OR_EQUAL, t_token.EQUALS, t_token.EQUAL_EQUAL, t_token.GREATER_THAN, t_token.GREATER_OR_EQUAL, t_token.LEFT_SQUARE, t_token.RIGHT_SQUARE, t_token.ARRAY, t_token.BOOLEAN, t_token.BREAK, t_token.CHARACTER, t_token.CHAR, t_token.CONTINUE, t_token.DO, t_token.ELSE, t_token.FALSE, t_token.FUNCTION, t_token.ID, t_token.IF, t_token.INTEGER, t_token.NUMERAL, t_token.OF, t_token.STRINGVAL, t_token.STRING, t_token.STRUCT, t_token.TRUE, t_token.TYPE, t_token.VAR, t_token.WHILE, t_token.LEFT_BRACES, t_token.OR, t_token.RIGHT_BRACES, t_token.END};
            for (int i = 0; i < actionHeader.Length; i++) {
                if (token == actionHeader[i]) return i + 1;
            }
            return -1;
        }

        public static int convertToToken(string noTerminal, List<List<String>> actionTable) {
            for (int i = 0; i < actionTable[0].Count; i++) {
                if (noTerminal.Equals(actionTable[0][i])) return i;
            }
            return -1;
        }

        public static List<List<String>> CreateActionTable(string path) {
            string[] lines = System.IO.File.ReadAllLines(path);
            List<List<string>> actionTable = new List<List<string>>();
            foreach (string line in lines) {
                string[] columns = line.Split(',');
                List<string> lista = new List<string>();
                for (int i = 0; i < columns.Length; i++) {
                    lista.Add(columns[i]);
                }
                actionTable.Add(lista);
            }
            return actionTable;
        }

        public static List<List<String>> CreateLenLeftTable(string path) {
            string[] lines = System.IO.File.ReadAllLines(path);
            List<List<string>> lenLeft = new List<List<string>>();
            foreach (string line in lines) {
                string[] columns = line.Split(',');
                List<string> lista = new List<string>();
                lista.Add(columns[0]);
                lista.Add(columns[1]);
                lenLeft.Add(lista);
            }
            return lenLeft;
        }

    }
}
