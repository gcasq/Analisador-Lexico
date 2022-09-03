using System;
using System.IO;
using LexicAnalyzer.Utils;
using static LexicAnalyzer.Enums;



namespace MyApp // Note: actual namespace depends on the project name.
{
    bool firstCall = true;
    public class Program {

        public static List<string> SecondaryToken { get; set; } = new();

        

        public static void Main(string[] args) {



        }

        public static int SearchName(string name) { 
            if(SecondaryToken.Contains(name)) return SecondaryToken.IndexOf(name);
            SecondaryToken.Add(name);
            return SecondaryToken.Count-1;
        }

        public static bool IsReservedKeyword(string keyword) =>
            Utils.SearchKeyword(keyword) != t_token.UNKNOWN;

        t_token nextToken(){

            while(isspace(nextChar)){
                nextChar = readChar();
            }

            if(isalpha(nextChar)){
                string text = "";

                do{
                    text = text + nextChar;
                    nextChar = readChar();
                }while(isalnum(nextChar) || nextChar == '_');

                //text = text + '\0';
                token = searchKeyWord(text);
                if(nextToken == ID){
                    tokenSecundario = searchName(text);
                }
            }else{
                if(isdigit(nextChar)){
                    string numeral = "";
                    do{
                        numeral = numeral + nextChar;
                        nextChar = readChar();
                    }while(isdigit(nextChar));

                    token = NUMERAL;
                    tokenSecundario = addIntConst(numeral);
                }else{
                    if(nextChar == '"'){
                        string str = "";
                        do{
                            str = str + nextChar;
                            nextChar = readChar();
                        }while(nextChar != '"');
                        nextChar = readChar();
                        token = STRINGVAL;
                        tokenSecundario = addStringConts(str);
                    }else{
                        if(nextChar == '\0') return END;
                        else{
                            switch(nextChar){

                                case '\'':
                                    nextChar = readChar();
                                    token = CHARACTER;
                                    tokenSecundario = addCharConst(nextChar);
                                    nextChar = readChar();
                                    nextChar = readChar();
                                    break;

                                case ':':
                                    nextChar = readChar();
                                    token = COLON;
                                    break;

                                case '+':
                                    nextChar = readChar();
                                    if(nextChar == '+'){
                                        token = PLUS_PLUS;
                                        nextChar = readChar();
                                    }else{
                                        token = PLUS;
                                    }
                                    break;

                                case ';':
                                    nextChar = readChar();
                                    token = SEMI_COLON;
                                    break;

                                case ',':
                                    nextChar = readChar();
                                    token = COMMA;
                                    break;

                                case '[':
                                    nextChar = readChar();
                                    token = LEFT_SQUARE;
                                    break;

                                case ']':
                                    nextChar = readChar();
                                    token = RIGHT_SQUARE;
                                    break;

                                case '{':
                                    nextChar = readChar();
                                    token = LEFT_BRACES;
                                    break;

                                case '}':
                                    nextChar = readChar();
                                    token = RIGHT_BRACES;
                                    break;

                                case '(':
                                    nextChar = readChar();
                                    token = LEFT_PARENTHESIS;
                                    break;

                                case ')':
                                    nextChar = readChar();
                                    token = RIGHT_PARENTHESIS;
                                    break;

                                case '&':
                                    nextChar = readChar();
                                    if (nextChar == '&'){
                                        token = AND;
                                    }else{
                                        token = UNKNOWN;
                                    }
                                    nextChar = readChar();
                                    break;

                                case '|':
                                    nextChar = readChar();
                                    if (nextChar == '|'){
                                            token = OR;
                                    }else{
                                        token = UNKNOWN;
                                    }
                                    nextChar = readChar();
                                    break;

                                case '*':
                                    nextChar = readChar();
                                    token = TIMES;
                                    break;

                                case '/':
                                    nextChar = readChar();
                                    token = DIVIDE;
                                    break;

                                case '.':
                                    nextChar = readChar();
                                    token = DOT;
                                    break;

                                case '!':
                                    nextChar = readChar();
                                    if(nextChar == '='){
                                        token = NOT_EQUAL;
                                        nextChar = readChar();
                                    }else{
                                        token = NOT;
                                    }
                                    break;

                                case '=':
                                    nextChar = readChar();
                                    if(nextChar == '='){
                                        token = EQUAL_EQUAL;
                                        nextChar = readChar();
                                    }else{
                                        token = EQUALS;
                                    }
                                    break;

                                case '-':
                                    nextChar = readChar();
                                    if(nextChar == '-'){
                                        token = MINUS_MINUS;
                                        nextChar = readChar();
                                    }else{
                                        token = MINUS;
                                    }
                                    break;
                                case '<':
                                    nextChar = readChar();
                                    if(nextChar == '='){
                                        token = LESS_OR_EQUAL;
                                        nextChar = readChar();
                                    }else{
                                        token = LESS_THAN;
                                    }
                                    break;

                                case '>':
                                    nextChar = readChar();
                                    if(nextChar == '='){
                                        token = GREATER_OR_EQUAL;
                                        nextChar = readChar();
                                    }else{
                                        token = GREATER_THAN;
                                    }
                                    break;
                                default:
                                    token = UNKNOWN;
                            }
                        }
                    }
                }
            }
            return token;
        }

        char readChar()
        {   
            if(firstCall){
                FileStream arquivo = File.OpenRead(@"C:\SeuArquivo.txt");
                firstCall = false;
            }
            char read = (char)arquivo.ReadByte();;
            if (read == '\n') currentLine++;
            if (arquivo.Position < arquivo.Length) return read;
            return '\0';
        }
    }
}