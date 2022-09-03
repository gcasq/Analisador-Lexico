
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LexicAnalyzer
{
    public class Enums{

        public enum t_token {
            // palavras reservadas
            ARRAY, 
            BOOLEAN, 
            BREAK, 
            CHAR, 
            CONTINUE, 
            DO, 
            ELSE, 
            FALSE, 
            FUNCTION, 
            IF,
            INTEGER, 
            OF, 
            STRING, 
            STRUCT, 
            TRUE, 
            TYPE, 
            VAR, 
            WHILE,

            // simbolos
            COLON, 
            SEMI_COLON, 
            COMMA, 
            EQUALS, 
            LEFT_SQUARE, 
            RIGHT_SQUARE,
            LEFT_BRACES,
            RIGHT_BRACES, 
            LEFT_PARENTHESIS, 
            RIGHT_PARENTHESIS, 
            AND,
            OR, 
            LESS_THAN, 
            GREATER_THAN, 
            LESS_OR_EQUAL, 
            GREATER_OR_EQUAL,
            NOT_EQUAL, 
            EQUAL_EQUAL, 
            PLUS, 
            PLUS_PLUS, 
            MINUS, 
            MINUS_MINUS, 
            TIMES,
            DIVIDE, 
            DOT, 
            NOT,

            // tokens regulares
            CHARACTER, 
            NUMERAL, 
            STRINGVAL, 
            ID,

            // token deconhecido
            UNKNOWN
        };
    }
}
