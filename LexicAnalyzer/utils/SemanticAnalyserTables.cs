using static LexicAnalyzer.utils.Enums;

namespace LexicAnalyzer.utils {
    public static class SemanticAnalyserTables {
        public static bool Is_Type_Kind(this t_kind? type) =>
            type == t_kind.ARRAY_TYPE_ ||
            type == t_kind.STRUCT_TYPE_ ||
            type == t_kind.ALIAS_TYPE_ ||
            type == t_kind.SCALAR_TYPE_;

        public static int Rule(int p) => -p;
    }

    public class myObj {
        public int nName;
        public myObj? pNext = null;
        public t_kind? eKind;
        public class Union {
            public class VarParamField {
                public myObj? pType;
                public int nIndex;
                public int nSize;
            }
            public VarParamField Var = new();
            public VarParamField Param = new();
            public VarParamField Field = new();

            public class Func {
                public myObj? pRetType;
                public myObj? pParams;
                public int nIndex;
                public int nParams;
                public int nVars;
            }
            public Func Function = new();

            public class Arr {
                public myObj? pElemType;
                public int nNumElems;
                public int nSize;
            }
            public Arr Array = new();

            public class Struc {
                public myObj? pFields;
                public int nSize;
            }
            public Struc Struct = new();

            public class AliasType {
                public myObj? pBaseType;
                public int nSize;
            }
            public AliasType Alias = new();
            public AliasType Type = new();
        }
        public Union _ = new();
    }

    public class t_attrib {
        public t_nont nont;
        public int nSize;
        public class Union {
            public class C1 {
                public myObj? obj = new();
                public int name;
            }
            public C1 ID = new();

            public class C2 {
                public myObj? type = new();
            }
            public C2 T = new();
            public C2 E = new();
            public C2 L = new();
            public C2 R = new();
            public C2 K = new();
            public C2 F = new();
            public C2 LV = new();

            public class C3 {
                public myObj? type = new();
                public myObj? param = new();
                public bool err;
            }
            public C3 MC = new();

            public class C4 {
                public int label;
            }
            public C4 MT = new();
            public C4 ME = new();
            public C4 MW = new();
            public C4 MA = new();

            public class C5 {
                public myObj? type = new();
                public myObj? param = new();
                public bool err;
                public int n;
            }
            public C5 LE = new();

            public class C6 {
                public myObj? list = new();
            }
            public C6 LI = new();
            public C6 DC = new();
            public C6 LP = new();

            public class C7 {
                public myObj? type = new();
                public bool val;
            }
            public C7 TRU = new C7();
            public C7 FALS = new C7();

            public class C8 {
                public myObj? type = new();
                public int pos;
                public char val;
            }
            public C8 CHR = new();

            public class C9 {
                public myObj? type = new();
                public string val;
                public int pos;
            }
            public C9 STR = new();

            public class C10 {
                public myObj? type = new();
                public int val;
                public int pos;
            }
            public C10 NUM = new();
        }
        public Union _ = new();
    }
}
