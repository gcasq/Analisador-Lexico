using static LexicAnalyzer.utils.Enums;

namespace LexicAnalyzer // Note: actual namespace depends on the project name.
{
    public class LexicAnalyzer {
        public const int MAX_CONSTS = 1000;
        public static t_token token;
        public static int tokenSecundario;
        public static char nextChar;
        public static FileStream? file;
        public static bool ErrorCompilation = false;
        public static bool EOF = false;
        public static int errors = 0;

        public static List<string> SecondaryToken { get; set; } = new();

        public struct t_const {
            public byte type; // 0 - char, 1 - int, 2 - string
            public char cVal;
            public int nVal;
            public string sVal;
        };

        public static t_const[] VConsts = new t_const[MAX_CONSTS];
        public static int nNumConsts = 0;

        public static void ExecuteLexicAnalyzer() {
            file = File.OpenRead("./SeuArquivo.txt");

            nextChar = readChar();
            t_token token = nextToken();
            while (token != t_token.END) {
                if (token == t_token.UNKNOWN) {
                    ErrorCompilation = true;
                    errors++;
                }
                token = nextToken();
            }

            Console.WriteLine("Quantidade de problemas de sintaxe: " + errors);

            Console.WriteLine("\nStatus final: ");
            Console.WriteLine(ErrorCompilation ? "Erro de Sintaxe" : "Sintaxe correta");
        }

        public static int searchName(string name) {
            if (SecondaryToken.Contains(name)) return SecondaryToken.IndexOf(name);
            SecondaryToken.Add(name);
            return SecondaryToken.Count - 1;
        }

        public static bool IsReservedKeyword(string keyword) =>
            Utils.searchKeyword(keyword) != t_token.UNKNOWN;

        public static t_token nextToken() {

            while (Char.IsWhiteSpace(nextChar)) {
                nextChar = readChar();
            }

            if (Char.IsLetter(nextChar)) {
                string text = "";

                do {
                    text = text + nextChar;
                    nextChar = readChar();
                } while (Char.IsNumber(nextChar) || Char.IsLetter(nextChar) || nextChar == '_');

                //text = text + '\0';
                token = Utils.searchKeyword(text);
                if (token == t_token.ID) {
                    tokenSecundario = searchName(text);
                }
            } else {
                if (Char.IsDigit(nextChar)) {
                    string numeral = "";
                    do {
                        numeral = numeral + nextChar;
                        nextChar = readChar();
                    } while (Char.IsDigit(nextChar));

                    token = t_token.NUMERAL;
                    tokenSecundario = addIntConst(int.Parse(numeral));
                } else {
                    if (nextChar == '"') {
                        string str = "";
                        do {
                            str = str + nextChar;
                            nextChar = readChar();
                        } while (nextChar != '"');
                        nextChar = readChar();
                        token = t_token.STRINGVAL;
                        tokenSecundario = addStringConst(str);
                    } else {
                        if (nextChar == '\0') return t_token.END;
                        else {
                            switch (nextChar) {

                                case '\'':
                                    nextChar = readChar();
                                    token = t_token.CHARACTER;
                                    tokenSecundario = addCharConst(nextChar);
                                    nextChar = readChar();
                                    nextChar = readChar();
                                    break;

                                case ':':
                                    nextChar = readChar();
                                    token = t_token.COLON;
                                    break;

                                case '+':
                                    nextChar = readChar();
                                    if (nextChar == '+') {
                                        token = t_token.PLUS_PLUS;
                                        nextChar = readChar();
                                    } else {
                                        token = t_token.PLUS;
                                    }
                                    break;

                                case ';':
                                    nextChar = readChar();
                                    token = t_token.SEMI_COLON;
                                    break;

                                case ',':
                                    nextChar = readChar();
                                    token = t_token.COMMA;
                                    break;

                                case '[':
                                    nextChar = readChar();
                                    token = t_token.LEFT_SQUARE;
                                    break;

                                case ']':
                                    nextChar = readChar();
                                    token = t_token.RIGHT_SQUARE;
                                    break;

                                case '{':
                                    nextChar = readChar();
                                    token = t_token.LEFT_BRACES;
                                    break;

                                case '}':
                                    nextChar = readChar();
                                    token = t_token.RIGHT_BRACES;
                                    break;

                                case '(':
                                    nextChar = readChar();
                                    token = t_token.LEFT_PARENTHESIS;
                                    break;

                                case ')':
                                    nextChar = readChar();
                                    token = t_token.RIGHT_PARENTHESIS;
                                    break;

                                case '&':
                                    nextChar = readChar();
                                    if (nextChar == '&') {
                                        token = t_token.AND;
                                    } else {
                                        token = t_token.UNKNOWN;
                                    }
                                    nextChar = readChar();
                                    break;

                                case '|':
                                    nextChar = readChar();
                                    if (nextChar == '|') {
                                        token = t_token.OR;
                                    } else {
                                        token = t_token.UNKNOWN;
                                    }
                                    nextChar = readChar();
                                    break;

                                case '*':
                                    nextChar = readChar();
                                    token = t_token.TIMES;
                                    break;

                                case '/':
                                    nextChar = readChar();
                                    token = t_token.DIVIDE;
                                    break;

                                case '.':
                                    nextChar = readChar();
                                    token = t_token.DOT;
                                    break;

                                case '!':
                                    nextChar = readChar();
                                    if (nextChar == '=') {
                                        token = t_token.NOT_EQUAL;
                                        nextChar = readChar();
                                    } else {
                                        token = t_token.NOT;
                                    }
                                    break;

                                case '=':
                                    nextChar = readChar();
                                    if (nextChar == '=') {
                                        token = t_token.EQUAL_EQUAL;
                                        nextChar = readChar();
                                    } else {
                                        token = t_token.EQUALS;
                                    }
                                    break;

                                case '-':
                                    nextChar = readChar();
                                    if (nextChar == '-') {
                                        token = t_token.MINUS_MINUS;
                                        nextChar = readChar();
                                    } else {
                                        token = t_token.MINUS;
                                    }
                                    break;
                                case '<':
                                    nextChar = readChar();
                                    if (nextChar == '=') {
                                        token = t_token.LESS_OR_EQUAL;
                                        nextChar = readChar();
                                    } else {
                                        token = t_token.LESS_THAN;
                                    }
                                    break;

                                case '>':
                                    nextChar = readChar();
                                    if (nextChar == '=') {
                                        token = t_token.GREATER_OR_EQUAL;
                                        nextChar = readChar();
                                    } else {
                                        token = t_token.GREATER_THAN;
                                    }
                                    break;
                                default:
                                    token = t_token.UNKNOWN;
                                    nextChar = readChar();
                                    break;
                            }
                        }
                    }
                }
            }
            return token;
        }

        public static char readChar() {
            if (!EOF) {
                char read = (char)file.ReadByte();
                if (file.Position < file.Length) return read;
                if (file.Position == file.Length) { EOF = true; return read; }
            }
            return '\0';
        }

        public static int addCharConst(char c) {
            VConsts[nNumConsts] = new t_const {
                type = 0,
                cVal = c
            };
            nNumConsts++;
            return nNumConsts - 1;
        }

        public static int addIntConst(int n) {
            VConsts[nNumConsts] = new t_const {
                type = 1,
                nVal = n
            };
            nNumConsts++;
            return nNumConsts - 1;
        }

        public static int addStringConst(string s) {
            VConsts[nNumConsts] = new t_const {
                type = 2,
                sVal = s
            };
            nNumConsts++;
            return nNumConsts - 1;
        }

        public static char getCharConst(int n) => VConsts[n].cVal;
        public static int getIntConst(int n) => VConsts[n].nVal;
        public static string getStringConst(int n) => VConsts[n].sVal;
    }
}