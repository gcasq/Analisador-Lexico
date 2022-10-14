namespace LexicAnalyzer.utils {
    public class Enums {

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
            UNKNOWN,

            //end of file
            END
        };

        public enum LeftRules {
            P,
            LDE,
            DE,
            DT,
            TP,
            DC,
            DF,
            LP,
            B,
            LDV,
            LS,
            DV,
            LI,
            S,
            E,
            L,
            R,
            TM,
            F,
            LE,
            LV,
            IDD,
            IDU,
            ID,
            NT_TRUE,
            NT_FALSE,
            NT_CHR,
            NT_STR,
            NT_NUM,
            NB,
            MF,
            MC
        }

        public enum errorcode {
            ERR_REDCL,
            ERR_NO_DECL,
            ERR_TYPE_EXPECTED,
            ERR_BOOL_TYPE_EXPECTED,
            ERR_TYPE_MISMATCH,
            ERR_INVALID_TYPE,
            ERR_KIND_NOT_STRUCT,
            ERR_FIELD_NOT_DECL,
            ERR_KIND_NOT_ARRAY,
            ERR_INVALID_INDEX_TYPE,
            ERR_KIND_NOT_VAR,
            ERR_KIND_NOT_FUNCTION,
            ERR_TOO_MANY_ARGS,
            ERR_PARAM_TYPE,
            ERR_TOO_FEW_ARGS,
            ERR_RETURN_TYPE_MISMATCH
        }

        public enum t_kind {
            NO_KIND_DEF_ = -1,
            VAR_,
            PARAM_,
            FUNCTION_,
            FIELD_,
            ARRAY_TYPE_,
            STRUCT_TYPE_,
            ALIAS_TYPE_,
            SCALAR_TYPE_,
            UNIVERSAL_
        }

        public enum t_nont {
            ACCEPT = 63, END, P, LDE, DE, DF, DT, DC, LI, DV, LP, B, LDV, LS, S, E, L, R, K, F, LE, LV, T, TRU, FALS, CHR, STR, NUM, IDD, IDU, ID, NB, MF, MC, NF, MT, ME, MW, MA
        };
    }
}
