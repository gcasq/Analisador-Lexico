using LexicAnalyzer.utils;
using static LexicAnalyzer.LexicAnalyzer;
using static LexicAnalyzer.utils.Enums;
using static LexicAnalyzer.utils.Enums.errorcode;
using static LexicAnalyzer.utils.Enums.t_kind;
using static LexicAnalyzer.utils.Enums.t_nont;
using static LexicAnalyzer.utils.RulesConstants;
using static LexicAnalyzer.utils.SemanticAnalyserTables;
namespace LexicAnalyzer {
    public class SemanticAnalyzer {
        public myObj pInt = new myObj { nName = -1, pNext = null, eKind = SCALAR_TYPE_ };
        public myObj pChar = new myObj { nName = -1, pNext = null, eKind = SCALAR_TYPE_ };
        public myObj pBool = new myObj { nName = -1, pNext = null, eKind = SCALAR_TYPE_ };
        public myObj pString = new myObj { nName = -1, pNext = null, eKind = SCALAR_TYPE_ };
        public myObj pUniversal = new myObj { nName = -1, pNext = null, eKind = SCALAR_TYPE_ };

        Stack<t_attrib> StackSem = new Stack<t_attrib>();

        public myObj?[] SymbolTable = new myObj[MAX_STACK];
        public myObj?[] SymbolTableLast = new myObj[MAX_STACK];
        int nCurrentLevel = 0;
        int nFuncs = 0;
        static int labelNo;
        myObj curFunction = new();

        public int newLabel() {
            labelNo = 0;
            return labelNo++;
        }

        public int newBlock() {
            SymbolTable[++nCurrentLevel] = null;
            SymbolTableLast[nCurrentLevel] = null;
            return nCurrentLevel;
        }

        public int endBlock() {
            return --nCurrentLevel;
        }

        public bool CheckTypes(myObj? t1, myObj? t2) {
            if (t1 == t2) {
                return true;
            } else if (t1 == pUniversal || t2 == pUniversal) {
                return true;
            } else if (t1.eKind == UNIVERSAL_ || t2.eKind == UNIVERSAL_) {
                return true;
            } else if (t1.eKind == ALIAS_TYPE_ && t2.eKind != ALIAS_TYPE_) {
                return CheckTypes(t1._.Alias.pBaseType, t2);
            } else if (t1.eKind != ALIAS_TYPE_ && t2.eKind == ALIAS_TYPE_) {
                return CheckTypes(t1, t2._.Alias.pBaseType);
            } else if (t1.eKind == t2.eKind) {
                //alias
                if (t1.eKind == ALIAS_TYPE_) {
                    return CheckTypes(t1._.Alias.pBaseType, t2._.Alias.pBaseType);
                }
                //array
                else if (t1.eKind == ARRAY_TYPE_) {
                    if (t1._.Array.nNumElems == t2._.Array.nNumElems) {
                        return CheckTypes(t1._.Array.pElemType, t2._.Array.pElemType);
                    }
                }
                //struct
                else if (t1.eKind == STRUCT_TYPE_) {
                    myObj? f1 = t1._.Struct.pFields;
                    myObj? f2 = t2._.Struct.pFields;

                    while (f1 != null && f2 != null) {
                        if (!CheckTypes(f1._.Field.pType, f2._.Field.pType)) {
                            return false;
                        }
                    }
                    return (f1 == null && f2 == null);
                }
            }

            return false;
        }

        private void Error(errorcode code, int currentLine) {
            Console.WriteLine("Linha: " + currentLine + " - ");
            switch (code) {
                case ERR_NO_DECL:
                    Console.WriteLine("Variavel nao declarada");
                    break;
                case ERR_REDCL:
                    Console.WriteLine("Variavel ja foi declarada");
                    break;
                case ERR_TYPE_EXPECTED:
                    Console.WriteLine("Type Expected: Um tipo nao foi declarado anteriormente");
                    break;
                case ERR_BOOL_TYPE_EXPECTED:
                    Console.WriteLine("Bool Expected: Um tipo booleano e esperado para expressao");
                    break;
                case ERR_INVALID_TYPE:
                    Console.WriteLine("Invalid Type: O tipo e invalido para a operacao");
                    break;
                case ERR_TYPE_MISMATCH:
                    Console.WriteLine("Type Mismatch: O tipo e invalido para a operacao");
                    break;
                case ERR_KIND_NOT_STRUCT:
                    Console.WriteLine("Kind not Struct: A operacao so pode ser realizada em tipos Struct");
                    break;
                case ERR_FIELD_NOT_DECL:
                    Console.WriteLine("Field not Declared: O campo nao foi declarado na estrutura");
                    break;
                case ERR_KIND_NOT_ARRAY:
                    Console.WriteLine("Kind not Array: A operacao so pode ser realizada para um Array");
                    break;
                case ERR_INVALID_INDEX_TYPE:
                    Console.WriteLine("Invalid Index: O Indice especificado para o Array e invalido");
                    break;
                case ERR_KIND_NOT_VAR:
                    Console.WriteLine("Kind not Var: A operacao so e valida com tipos Var");
                    break;
                case ERR_KIND_NOT_FUNCTION:
                    Console.WriteLine("Kind not Function: A operacao so e valida com tipos Function");
                    break;
                case ERR_TOO_FEW_ARGS:
                    Console.WriteLine("Too Few Args: O numero de parametros especificado nao e suficiente");
                    break;
                case ERR_TOO_MANY_ARGS:
                    Console.WriteLine("Too Many Args: O numero de parametros especificado e maior que o especificado");
                    break;
                case ERR_PARAM_TYPE:
                    Console.WriteLine("Param Type: O tipo especificado para o parametro e invalido");
                    break;
                case ERR_RETURN_TYPE_MISMATCH:
                    Console.WriteLine("Return Type Mismatch: O tipo de retorno não corresponde ao tipo especificado para a função");
                    break;
                default:
                    break;
            }
        }

        private myObj? search(int aName) {
            myObj? obj = SymbolTable[nCurrentLevel];
            while (obj != null) {
                if (obj.nName == aName) {
                    break;
                } else {
                    obj = obj.pNext;
                }
            }
            return obj;
        }

        private myObj? define(int aName) {
            var obj = new myObj();
            obj.nName = aName;
            obj.pNext = null;

            if (SymbolTable[nCurrentLevel] == null) {
                SymbolTable[nCurrentLevel] = obj;
                SymbolTableLast[nCurrentLevel] = obj;
            } else {
                SymbolTableLast[nCurrentLevel].pNext = obj;
                SymbolTableLast[nCurrentLevel] = obj;
            }
            return obj;
        }

        private myObj? find(int aName) {
            int i;
            var obj = new myObj();
            for (i = nCurrentLevel; i >= 0; --i) {
                obj = SymbolTable[i];
                while (obj != null) {
                    if (obj.nName == aName)
                        break;
                    else
                        obj = obj.pNext;
                }
                if (obj != null) break;
            }
            return obj;
        }

        public void semantics(int ruleNumber, int tokenSecundario, int currentLine) {
            int name, n, l, l1, l2;
            myObj? p = new();
            myObj? t = new();
            myObj? f = new();
            t_attrib IDD_, IDU_, ID_, T_, LI_, LI0_, LI1_, TRU_, FALS_, STR_, CHR_, NUM_, DC_, DC0_, DC1_, LP_, LP0_, LP1_, E_, E0_, E1_, L_, L0_, L1_, R_, R0_, R1_, K_, K0_, K1_, F_, F0_, F1_, LV_, LV0_, LV1_, MC_, LE_, LE0_, LE1_, MT_, ME_, MW_, MA_;

            switch (ruleNumber) {
                case DF_TO_FUNC:
                    endBlock();
                    Console.WriteLine("END_FUNC");
                    break;
                case DT_TO_STRUCT:
                    DC_ = StackSem.Peek();
                    StackSem.Pop();
                    IDD_ = StackSem.Peek();
                    StackSem.Pop();
                    p = IDD_._.ID.obj;
                    p.eKind = STRUCT_TYPE_;
                    p._.Struct.pFields = DC_._.DC.list;
                    p._.Struct.nSize = DC_.nSize;
                    endBlock();
                    break;
                case DT_TO_ARRAY:
                    T_ = StackSem.Peek();
                    StackSem.Pop();
                    NUM_ = StackSem.Peek();
                    StackSem.Pop();
                    IDD_ = StackSem.Peek();
                    StackSem.Pop();
                    p = IDD_._.ID.obj;
                    n = NUM_._.NUM.val;
                    t = T_._.T.type;
                    p.eKind = ARRAY_TYPE_;
                    p._.Array.nNumElems = n;
                    p._.Array.pElemType = t;
                    p._.Array.nSize = n * T_.nSize;
                    break;
                case DT_TO_IDD:
                    T_ = StackSem.Peek();
                    StackSem.Pop();
                    IDD_ = StackSem.Peek();
                    StackSem.Pop();
                    p = IDD_._.ID.obj;
                    t = T_._.T.type;
                    p.eKind = ALIAS_TYPE_;
                    p._.Alias.pBaseType = t;
                    p._.Alias.nSize = T_.nSize;
                    break;
                case DC_TO_DC:
                    T_ = StackSem.Peek();
                    StackSem.Pop();
                    LI_ = StackSem.Peek();
                    StackSem.Pop();
                    DC1_ = StackSem.Peek();
                    StackSem.Pop();
                    p = LI_._.LI.list;
                    t = T_._.T.type;
                    n = DC1_.nSize;
                    while (p != null && p.eKind == NO_KIND_DEF_) {
                        p.eKind = FIELD_;
                        p._.Field.pType = t;
                        p._.Field.nIndex = n;
                        p._.Field.nSize = T_.nSize;
                        n = n + T_.nSize;
                        p = p.pNext;
                    }
                    DC0_ = new();
                    DC0_._.DC.list = DC1_._.DC.list;
                    DC0_.nSize = n;
                    DC0_.nont = DC;
                    StackSem.Push(DC0_);
                    break;
                case DC_TO_LI:
                    T_ = StackSem.Peek();
                    StackSem.Pop();
                    LI_ = StackSem.Peek();
                    StackSem.Pop();
                    p = LI_._.LI.list;
                    t = T_._.T.type;
                    n = 0;
                    while (p != null && p.eKind == NO_KIND_DEF_) {
                        p.eKind = FIELD_;
                        p._.Field.pType = t;
                        p._.Field.nSize = T_.nSize;
                        p._.Field.nIndex = n;
                        n = n + T_.nSize;
                        p = p.pNext;
                    }
                    DC_ = new();
                    DC_._.DC.list = LI_._.LI.list;
                    DC_.nSize = n;
                    DC_.nont = DC;
                    StackSem.Push(DC_);
                    break;
                case LI_TO_LI_COMMA:
                    IDD_ = StackSem.Peek();
                    StackSem.Pop();
                    LI1_ = StackSem.Peek();
                    StackSem.Pop();
                    LI0_ = new();
                    LI0_._.LI.list = LI1_._.LI.list;
                    LI0_.nont = LI;
                    StackSem.Push(LI0_);
                    break;
                case LI_TO_IDD:
                    IDD_ = StackSem.Peek();
                    LI_ = new();
                    LI_._.LI.list = IDD_._.ID.obj;
                    LI_.nont = LI;
                    StackSem.Pop();
                    StackSem.Push(LI_);
                    break;
                case DV_TO_VAR:
                    T_ = StackSem.Peek();
                    t = T_._.T.type;
                    StackSem.Pop();
                    LI_ = StackSem.Peek();
                    StackSem.Pop();
                    p = LI_._.LI.list;
                    n = curFunction._.Function.nVars;
                    while (p != null && p.eKind == NO_KIND_DEF_) {
                        p.eKind = VAR_;
                        p._.Var.pType = t;
                        p._.Var.nSize = T_.nSize;
                        p._.Var.nIndex = n;
                        n += T_.nSize;
                        p = p.pNext;
                    }
                    curFunction._.Function.nVars = n;
                    break;
                case LP_TO_LP:
                    T_ = StackSem.Peek();
                    StackSem.Pop();
                    IDD_ = StackSem.Peek();
                    StackSem.Pop();
                    LP1_ = StackSem.Peek();
                    StackSem.Pop();
                    p = IDD_._.ID.obj;
                    t = T_._.T.type;
                    n = LP1_.nSize;
                    p.eKind = PARAM_;
                    p._.Param.pType = t;
                    p._.Param.nIndex = n;
                    p._.Param.nSize = T_.nSize;
                    LP0_ = new();
                    LP0_._.LP.list = LP1_._.LP.list;
                    LP0_.nSize = n + T_.nSize;
                    LP0_.nont = LP;
                    StackSem.Push(LP0_);
                    break;
                case LP_TO_IDD:
                    T_ = StackSem.Peek();
                    StackSem.Pop();
                    IDD_ = StackSem.Peek();
                    StackSem.Pop();
                    p = IDD_._.ID.obj;
                    t = T_._.T.type;
                    p.eKind = PARAM_;
                    p._.Param.pType = t;
                    p._.Param.nIndex = 0;
                    p._.Param.nSize = T_.nSize;
                    LP_ = new();
                    LP_._.LP.list = p;
                    LP_.nSize = T_.nSize;
                    LP_.nont = LP;
                    StackSem.Push(LP_);
                    break;
                case S_TO_NB:
                    endBlock();
                    break;
                case S_TO_WHILE:
                    MT_ = StackSem.Peek();
                    StackSem.Pop();
                    E_ = StackSem.Peek();
                    StackSem.Pop();
                    MW_ = StackSem.Peek();
                    StackSem.Pop();
                    l1 = MW_._.MW.label;
                    l2 = MT_._.MT.label;
                    t = E_._.E.type;
                    if (!CheckTypes(t, pBool)) {
                        Error(ERR_BOOL_TYPE_EXPECTED, currentLine);
                    }
                    Console.WriteLine($"JMP_BW L {l1}\nL {l2}\n");
                    break;
                case S_TO_DO_WHILE:
                    E_ = StackSem.Peek();
                    StackSem.Pop();
                    MW_ = StackSem.Peek();
                    StackSem.Pop();
                    l = MW_._.MW.label;
                    t = E_._.E.type;
                    if (!CheckTypes(t, pBool)) {
                        Error(ERR_BOOL_TYPE_EXPECTED, currentLine);
                    }
                    Console.WriteLine($"NOT\nTJMP_BW L {l}\n");
                    break;
                case S_TO_IF:
                    StackSem.Pop();
                    MT_ = StackSem.Peek();
                    StackSem.Pop();
                    E_ = StackSem.Peek();
                    StackSem.Pop();
                    t = E_._.E.type;
                    if (!CheckTypes(t, pBool)) {
                        Error(ERR_BOOL_TYPE_EXPECTED, currentLine);
                    }
                    Console.WriteLine($"L {MT_._.MT.label}\n");
                    break;
                case S_TO_IF_ELSE:
                    ME_ = StackSem.Peek();
                    StackSem.Pop();
                    MT_ = StackSem.Peek();
                    StackSem.Pop();
                    E_ = StackSem.Peek();
                    StackSem.Pop();
                    l = ME_._.ME.label;
                    t = E_._.E.type;
                    if (!CheckTypes(t, pBool)) {
                        Error(ERR_BOOL_TYPE_EXPECTED, currentLine);
                    }
                    Console.WriteLine($"L {l}\n");
                    break;
                case S_TO_BREAK:
                    MT_ = StackSem.Peek();
                    break;
                case S_TO_CONTINUE:
                    break;
                case S_TO_LV:
                    E_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(curFunction._.Function.pRetType, E_._.E.type)) {
                        Error(ERR_RETURN_TYPE_MISMATCH, currentLine);
                    }
                    Console.WriteLine($"RET\n");
                    break;
                case E_TO_AND:
                    L_ = StackSem.Peek();
                    StackSem.Pop();
                    E1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(E1_._.E.type, pBool)) {
                        Error(ERR_BOOL_TYPE_EXPECTED, currentLine);
                    }
                    if (!CheckTypes(L_._.L.type, pBool)) {
                        Error(ERR_BOOL_TYPE_EXPECTED, currentLine);
                    }
                    E0_ = new();
                    E0_._.E.type = pBool;
                    E0_.nont = E;
                    StackSem.Push(E0_);
                    Console.WriteLine($"AND\n");
                    break;
                case E_TO_OR:
                    L_ = StackSem.Peek();
                    StackSem.Pop();
                    E1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(E1_._.E.type, pBool)) {
                        Error(ERR_BOOL_TYPE_EXPECTED, currentLine);
                    }
                    if (!CheckTypes(L_._.L.type, pBool)) {
                        Error(ERR_BOOL_TYPE_EXPECTED, currentLine);
                    }
                    E0_ = new();
                    E0_._.E.type = pBool;
                    E0_.nont = E;
                    StackSem.Push(E0_);
                    Console.WriteLine($"OR\n");
                    break;
                case E_TO_L:
                    L_ = StackSem.Peek();
                    StackSem.Pop();
                    E_ = new();
                    E_._.E.type = L_._.L.type;
                    E_.nont = E;
                    StackSem.Push(E_);
                    break;
                case L_TO_LESS_THAN:
                    R_ = StackSem.Peek();
                    StackSem.Pop();
                    L1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(L1_._.L.type, R_._.R.type)) {
                        Error(ERR_TYPE_MISMATCH, currentLine);
                    }
                    L0_ = new();
                    L0_._.L.type = pBool;
                    L0_.nont = L;
                    StackSem.Push(L0_);
                    Console.WriteLine($"LT\n");
                    break;
                case L_TO_GREATER_THAN:
                    R_ = StackSem.Peek();
                    StackSem.Pop();
                    L1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(L1_._.L.type, R_._.R.type)) {
                        Error(ERR_TYPE_MISMATCH, currentLine);
                    }
                    L0_ = new();
                    L0_._.L.type = pBool;
                    L0_.nont = L;
                    StackSem.Push(L0_);
                    Console.WriteLine($"GT\n");
                    break;
                case L_TO_LESS_EQUAL:
                    R_ = StackSem.Peek();
                    StackSem.Pop();
                    L1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(L1_._.L.type, R_._.R.type)) {
                        Error(ERR_TYPE_MISMATCH, currentLine);
                    }
                    L0_ = new();
                    L0_._.L.type = pBool;
                    L0_.nont = L;
                    StackSem.Push(L0_);
                    Console.WriteLine($"LE\n");
                    break;
                case L_TO_GREATER_EQUAL:
                    R_ = StackSem.Peek();
                    StackSem.Pop();
                    L1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(L1_._.L.type, R_._.R.type)) {
                        Error(ERR_TYPE_MISMATCH, currentLine);
                    }
                    L0_ = new();
                    L0_._.L.type = pBool;
                    L0_.nont = L;
                    StackSem.Push(L0_);
                    Console.WriteLine($"GE\n");
                    break;
                case L_TO_EQUAL:
                    R_ = StackSem.Peek();
                    StackSem.Pop();
                    L1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(L1_._.L.type, R_._.R.type)) {
                        Error(ERR_TYPE_MISMATCH, currentLine);
                    }
                    L0_ = new();
                    L0_._.L.type = pBool;
                    L0_.nont = L;
                    StackSem.Push(L0_);
                    Console.WriteLine($"EQ\n");
                    break;
                case L_TO_DIFF:
                    R_ = StackSem.Peek();
                    StackSem.Pop();
                    L1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(L1_._.L.type, R_._.R.type)) {
                        Error(ERR_TYPE_MISMATCH, currentLine);
                    }
                    L0_ = new();
                    L0_._.L.type = pBool;
                    L0_.nont = L;
                    StackSem.Push(L0_);
                    Console.WriteLine($"NE\n");
                    break;
                case L_TO_R:
                    R_ = StackSem.Peek();
                    StackSem.Pop();
                    L_ = new();
                    L_._.L.type = R_._.R.type;
                    L_.nont = L;
                    StackSem.Push(L_);
                    break;
                case R_TO_PLUS:
                    K_ = StackSem.Peek();
                    StackSem.Pop();
                    R1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(R1_._.R.type, K_._.K.type)) {
                        Error(ERR_TYPE_MISMATCH, currentLine);
                    }
                    if (!CheckTypes(R1_._.R.type, pInt) && !CheckTypes(R1_._.R.type, pString)) {
                        Error(ERR_INVALID_TYPE, currentLine);
                    }
                    R0_ = new();
                    R0_._.R.type = R1_._.R.type;
                    R0_.nont = R;
                    StackSem.Push(R0_);
                    Console.WriteLine($"ADD\n");
                    break;
                case R_TO_MINUS:
                    K_ = StackSem.Peek();
                    StackSem.Pop();
                    R1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(R1_._.R.type, K_._.K.type)) {
                        Error(ERR_TYPE_MISMATCH, currentLine);
                    }
                    if (!CheckTypes(R1_._.R.type, pInt)) {
                        Error(ERR_INVALID_TYPE, currentLine);
                    }
                    R0_ = new();
                    R0_._.R.type = R1_._.R.type;
                    R0_.nont = R;
                    StackSem.Push(R0_);
                    Console.WriteLine($"SUB\n");
                    break;
                case R_TO_Y:
                    K_ = StackSem.Peek();
                    StackSem.Pop();
                    R_ = new();
                    R_._.R.type = K_._.K.type;
                    R_.nont = R;
                    StackSem.Push(R_);
                    break;
                case Y_TO_TIMES:
                    F_ = StackSem.Peek();
                    StackSem.Pop();
                    K1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(K1_._.K.type, F_._.F.type)) {
                        Error(ERR_TYPE_MISMATCH, currentLine);
                    }
                    if (!CheckTypes(K1_._.K.type, pInt)) {
                        Error(ERR_INVALID_TYPE, currentLine);
                    }
                    K0_ = new();
                    K0_._.K.type = K1_._.K.type;
                    K0_.nont = K;
                    StackSem.Push(K0_);
                    Console.WriteLine($"MUL\n");
                    break;
                case Y_TO_DIVIDE:
                    F_ = StackSem.Peek();
                    StackSem.Pop();
                    K1_ = StackSem.Peek();
                    StackSem.Pop();
                    if (!CheckTypes(K1_._.K.type, F_._.F.type)) {
                        Error(ERR_TYPE_MISMATCH, currentLine);
                    }
                    if (!CheckTypes(K1_._.K.type, pInt)) {
                        Error(ERR_INVALID_TYPE, currentLine);
                    }
                    K0_ = new();
                    K0_._.K.type = K1_._.K.type;
                    K0_.nont = K;
                    StackSem.Push(K0_);
                    Console.WriteLine($"DIV\n");
                    break;
                case Y_TO_F:
                    F_ = StackSem.Peek();
                    StackSem.Pop();
                    K_ = new();
                    K_._.K.type = F_._.F.type;
                    K_.nont = K;
                    StackSem.Push(K_);
                    break;
                case F_TO_LV:
                    LV_ = StackSem.Peek();
                    StackSem.Pop();
                    n = LV_._.LV.type._.Type.nSize;
                    F_ = new();
                    F_._.F.type = LV_._.LV.type;
                    F_.nont = F;
                    StackSem.Push(F_);
                    Console.WriteLine($"DE_REF {n}\n");
                    break;
                case F_TO_PLUS_LV:
                    LV_ = StackSem.Peek();
                    StackSem.Pop();
                    t = LV_._.LV.type;
                    if (!CheckTypes(t, pInt)) {
                        Error(ERR_INVALID_TYPE, currentLine);
                    }
                    F_ = new();
                    F_._.F.type = pInt;
                    F_.nont = F;
                    Console.WriteLine($"DUP\nDUP\nDE_REF 1\n");
                    Console.WriteLine($"INC\nSTORE_REF 1\nDE_REF 1\n");
                    StackSem.Push(F_);
                    break;
                case F_TO_MINUS_LV:
                    LV_ = StackSem.Peek();
                    StackSem.Pop();
                    t = LV_._.LV.type;
                    if (!CheckTypes(t, pInt)) {
                        Error(ERR_INVALID_TYPE, currentLine);
                    }
                    F_ = new();
                    F_._.F.type = LV_._.LV.type;
                    F_.nont = F;
                    StackSem.Push(F_);
                    Console.WriteLine($"DUP\nDUP\nDE_REF 1\n");
                    Console.WriteLine($"DEC\nSTORE_REF 1\nDE_REF 1\n");
                    break;
                case F_TO_LV_PLUS:
                    LV_ = StackSem.Peek();
                    StackSem.Pop();
                    t = LV_._.LV.type;
                    if (!CheckTypes(t, pInt)) {
                        Error(ERR_INVALID_TYPE, currentLine);
                    }
                    F_ = new();
                    F_._.F.type = LV_._.LV.type;
                    F_.nont = F;
                    StackSem.Push(F_);
                    Console.WriteLine($"DUP\nDUP\nDE_REF 1\n");
                    Console.WriteLine($"INC\nSTORE_REF 1\nDE_REF 1\n");
                    Console.WriteLine($"DEC\n");
                    break;
                case F_TO_LV_MINUS:
                    LV_ = StackSem.Peek();
                    StackSem.Pop();
                    t = LV_._.LV.type;
                    if (!CheckTypes(t, pInt)) {
                        Error(ERR_INVALID_TYPE, currentLine);
                    }
                    F_ = new();
                    F_._.F.type = t;
                    F_.nont = F;
                    StackSem.Push(F_);
                    Console.WriteLine($"DUP\nDUP\nDE_REF 1\n");
                    Console.WriteLine($"DEC\nSTORE_REF 1\nDE_REF 1\n");
                    Console.WriteLine($"INC\n");
                    break;
                case F_TO_E:
                    E_ = StackSem.Peek();
                    StackSem.Pop();
                    F_ = new();
                    F_._.F.type = E_._.E.type;
                    F_.nont = F;
                    StackSem.Push(F_);
                    break;
                case F_TO_IDU:
                    LE_ = StackSem.Peek();
                    StackSem.Pop();
                    MC_ = StackSem.Peek();
                    StackSem.Pop();
                    IDU_ = StackSem.Peek();
                    StackSem.Pop();
                    f = IDU_._.ID.obj;
                    F_ = new();
                    F_._.F.type = MC_._.MC.type;
                    if (!LE_._.LE.err) {
                        if (LE_._.LE.n - 1 < f._.Function.nParams) {
                            Error(ERR_TOO_FEW_ARGS, currentLine);
                        } else if (LE_._.LE.n - 1 > f._.Function.nParams) {
                            Error(ERR_TOO_MANY_ARGS, currentLine);
                        }
                    }
                    F_.nont = F;
                    StackSem.Push(F_);
                    Console.WriteLine($"CALL {f._.Function.nIndex}\n");
                    break;
                case F_TO_MINUS_F:
                    F1_ = StackSem.Peek();
                    StackSem.Pop();
                    t = F1_._.F.type;
                    if (!CheckTypes(t, pInt)) {
                        Error(ERR_INVALID_TYPE, currentLine);
                    }
                    F0_ = new();
                    F0_._.F.type = t;
                    F0_.nont = F;
                    StackSem.Push(F0_);
                    Console.WriteLine($"NEG\n");
                    break;
                case F_TO_DIF_F:
                    F1_ = StackSem.Peek();
                    StackSem.Pop();
                    t = F1_._.F.type;
                    if (!CheckTypes(t, pBool)) {
                        Error(ERR_INVALID_TYPE, currentLine);
                    }
                    F0_ = new();
                    F0_._.F.type = t;
                    F0_.nont = F;
                    StackSem.Push(F0_);
                    Console.WriteLine($"NOT\n");
                    break;
                case F_TO_TRUE:
                    TRU_ = StackSem.Peek();
                    StackSem.Pop();
                    F_ = new();
                    F_._.F.type = pBool;
                    F_.nont = F;
                    StackSem.Push(F_);
                    Console.WriteLine($"LOAD_TRUE\n");
                    break;
                case F_TO_FALSE:
                    FALS_ = StackSem.Peek();
                    StackSem.Pop();
                    F_ = new();
                    F_._.F.type = pBool;
                    F_.nont = F;
                    StackSem.Push(F_);
                    Console.WriteLine($"LOAD_FALSE\n");
                    break;
                case F_TO_CHR:
                    CHR_ = StackSem.Peek();
                    StackSem.Pop();
                    F_ = new();
                    F_._.F.type = pChar;
                    F_.nont = F;
                    StackSem.Push(F_);
                    n = tokenSecundario;
                    Console.WriteLine($"LOAD_CONST {n}\n");
                    break;
                case F_TO_STR:
                    STR_ = StackSem.Peek();
                    StackSem.Pop();
                    F_ = new();
                    F_._.F.type = pString;
                    F_.nont = F;
                    StackSem.Push(F_);
                    n = tokenSecundario;
                    Console.WriteLine($"LOAD_CONST {n}\n");
                    break;
                case F_TO_NUM:
                    STR_ = StackSem.Peek();
                    StackSem.Pop();
                    F_ = new();
                    F_._.F.type = pInt;
                    F_.nont = F;
                    StackSem.Push(F_);
                    n = tokenSecundario;
                    Console.WriteLine($"LOAD_CONST {n}\n");
                    break;
                case LE_TO_LE:
                    E_ = StackSem.Peek();
                    StackSem.Pop();
                    LE1_ = StackSem.Peek();
                    StackSem.Pop();
                    LE0_ = new();
                    LE0_._.LE.param = null;
                    L1_ = new();
                    LE0_._.LE.err = L1_._.LE.err;
                    n = LE1_._.LE.n;
                    if (!LE1_._.LE.err) {
                        p = LE1_._.LE.param;
                        if (p == null) {
                            Error(ERR_TOO_MANY_ARGS, currentLine);
                            LE0_._.LE.err = true;
                        } else {
                            if (!CheckTypes(p._.Param.pType, E_._.E.type)) {
                                Error(ERR_PARAM_TYPE, currentLine);
                            }
                            LE0_._.LE.param = p.pNext;
                            LE0_._.LE.n = n + 1;
                        }
                    }
                    LE0_.nont = LE;
                    StackSem.Push(LE0_);
                    break;
                case LE_TO_E:
                    E_ = StackSem.Peek();
                    StackSem.Pop();
                    MC_ = StackSem.Peek();
                    LE_ = new();
                    LE_._.LE.param = null;
                    LE_._.LE.err = MC_._.MC.err;
                    n = 1;
                    if (!MC_._.MC.err) {
                        p = MC_._.MC.param;
                        if (p == null) {
                            Error(ERR_TOO_MANY_ARGS, currentLine);
                            LE_._.LE.err = true;
                        } else {
                            if (!CheckTypes(p._.Param.pType, E_._.E.type)) {
                                Error(ERR_PARAM_TYPE, currentLine);
                            }
                            LE_._.LE.param = p.pNext;
                            LE_._.LE.n = n + 1;
                        }
                    }
                    LE_.nont = LE;
                    StackSem.Push(LE_);
                    break;
                case LV_TO_LV_IDU:
                    ID_ = StackSem.Peek();
                    StackSem.Pop();
                    LV1_ = StackSem.Peek();
                    StackSem.Pop();
                    LV0_ = new();
                    t = LV1_._.LV.type;
                    if (t.eKind != STRUCT_TYPE_) {
                        if (t.eKind != UNIVERSAL_) {
                            Error(ERR_KIND_NOT_STRUCT, currentLine);
                        }
                        LV0_._.LV.type = pUniversal;
                    } else {
                        p = t._.Struct.pFields;
                        while (p != null) {
                            if (p.nName == ID_._.ID.name) {
                                break;
                            }
                            p = p.pNext;
                        }
                        if (p == null) {
                            Error(ERR_FIELD_NOT_DECL, currentLine);
                            LV0_._.LV.type = pUniversal;
                        } else {
                            LV0_._.LV.type = p._.Field.pType;
                            LV0_._.LV.type._.Type.nSize = p._.Field.nSize;
                        }
                    }
                    LV0_.nont = LV;
                    StackSem.Push(LV0_);
                    Console.WriteLine($"ADD {p._.Field.nIndex}\n");
                    break;
                case LV_TO_LV_E:
                    E_ = StackSem.Peek();
                    StackSem.Pop();
                    LV1_ = StackSem.Peek();
                    StackSem.Pop();
                    LV0_ = new();
                    t = LV1_._.LV.type;
                    if (t == pString) {
                        LV0_._.LV.type = pChar;
                    } else if (t.eKind != ARRAY_TYPE_) {
                        if (t.eKind != UNIVERSAL_) {
                            Error(ERR_KIND_NOT_ARRAY, currentLine);
                        }
                        LV0_._.LV.type = pUniversal;
                    } else {
                        LV0_._.LV.type = t._.Array.pElemType;
                        n = t._.Array.nSize / t._.Array.nNumElems;
                        Console.WriteLine($"MUL {n}\nADD\n");
                    }
                    if (!CheckTypes(E_._.E.type, pInt)) {
                        Error(ERR_INVALID_INDEX_TYPE, currentLine);
                    }
                    LV0_.nont = LV;
                    StackSem.Push(LV0_);
                    break;
                case LV_TO_IDU:
                    IDU_ = StackSem.Peek();
                    StackSem.Pop();
                    LV_ = new();
                    p = IDU_._.ID.obj;
                    if (p.eKind != VAR_ && p.eKind != PARAM_) {
                        if (p.eKind != UNIVERSAL_) {
                            Error(ERR_KIND_NOT_VAR, currentLine);
                        }
                        LV_._.LV.type = pUniversal;
                    } else {
                        LV_._.LV.type = p._.Var.pType;
                        LV_._.LV.type._.Type.nSize = p._.Var.nSize;
                    }
                    LV_.nont = LV;
                    StackSem.Push(LV_);
                    Console.WriteLine($"LOAD_REF {p._.Var.nIndex}");
                    break;
                case T_TO_INTEGER:
                    T_ = new();
                    T_._.T.type = pInt;
                    T_.nont = T;
                    T_.nSize = 1;
                    StackSem.Push(T_);
                    break;
                case T_TO_CHR:
                    T_ = new();
                    T_._.T.type = pChar;
                    T_.nont = T;
                    T_.nSize = 1;
                    StackSem.Push(T_);
                    break;
                case T_TO_BOOL:
                    T_ = new();
                    T_._.T.type = pBool;
                    T_.nont = T;
                    T_.nSize = 1;
                    StackSem.Push(T_);
                    break;
                case T_TO_STR:
                    T_ = new();
                    T_._.T.type = pString;
                    T_.nont = T;
                    T_.nSize = 1;
                    StackSem.Push(T_);
                    break;
                case T_TO_IDU:
                    IDU_ = StackSem.Peek();
                    p = IDU_._.ID.obj;
                    StackSem.Pop();
                    T_ = new();
                    if (p.eKind.Is_Type_Kind() || p.eKind == UNIVERSAL_) {
                        T_._.T.type = p;
                        T_.nSize = p._.Alias.nSize;
                    } else {
                        T_._.T.type = pUniversal;
                        T_.nSize = 0;
                        Error(ERR_TYPE_EXPECTED, currentLine);
                    }
                    T_.nont = T;
                    StackSem.Push(T_);
                    break;
                case TRUE_RULE:
                    TRU_ = new() {
                        nont = TRU
                    };
                    TRU_._.TRU.val = true;
                    TRU_._.TRU.type = pBool;
                    StackSem.Push(TRU_);
                    break;
                case FALSE_RULE:
                    FALS_ = new() {
                        nont = FALS
                    };
                    FALS_._.FALS.val = false;
                    FALS_._.FALS.type = pBool;
                    StackSem.Push(FALS_);
                    break;
                case CHR_RULE:
                    CHR_ = new() {
                        nont = CHR
                    };
                    CHR_._.CHR.type = pChar;
                    CHR_._.CHR.pos = tokenSecundario;
                    CHR_._.CHR.val = getCharConst(tokenSecundario);
                    StackSem.Push(CHR_);
                    break;
                case STR_RULE:
                    STR_ = new() {
                        nont = STR
                    };
                    STR_._.STR.type = pString;
                    STR_._.STR.pos = tokenSecundario;
                    STR_._.STR.val = getStringConst(tokenSecundario);
                    StackSem.Push(STR_);
                    break;
                case NUM_RULE:
                    NUM_ = new() {
                        nont = NUM
                    };
                    NUM_._.NUM.type = pInt;
                    NUM_._.NUM.pos = tokenSecundario;
                    NUM_._.NUM.val = getIntConst(tokenSecundario);
                    StackSem.Push(NUM_);
                    break;
                case IDD_RULE:
                    name = tokenSecundario;
                    IDD_ = new() {
                        nont = IDD
                    };
                    IDD_._.ID.name = name;
                    if ((p = search(name)) != null) {
                        Error(ERR_REDCL, currentLine);
                    } else {
                        p = define(name);
                    }
                    p.eKind = NO_KIND_DEF_;
                    IDD_._.ID.obj = p;
                    StackSem.Push(IDD_);
                    break;
                case IDU_RULE:
                    name = tokenSecundario;
                    IDU_ = new() {
                        nont = IDU
                    };
                    IDU_._.ID.name = name;
                    if ((p = find(name)) == null) {
                        Error(ERR_NO_DECL, currentLine);
                        p = define(name);
                    }
                    IDU_._.ID.obj = p;
                    StackSem.Push(IDU_);
                    break;
                case ID_RULE:
                    ID_ = new() {
                        nont = ID
                    };
                    name = tokenSecundario;
                    ID_._.ID.name = name;
                    ID_._.ID.obj = null;
                    StackSem.Push(ID_);
                    break;
                case NB_RULE:
                    newBlock();
                    break;
                case MF_RULE:
                    T_ = StackSem.Peek();
                    StackSem.Pop();
                    LP_ = StackSem.Peek();
                    StackSem.Pop();
                    IDD_ = StackSem.Peek();
                    StackSem.Pop();
                    f = IDD_._.ID.obj;
                    f.eKind = FUNCTION_;
                    f._.Function.pRetType = T_._.T.type;
                    f._.Function.pParams = LP_._.LP.list;
                    f._.Function.nParams = LP_.nSize;
                    f._.Function.nVars = LP_.nSize;
                    curFunction = f;
                    Console.WriteLine($"BEGIN_FUNC {f._.Function.nIndex} {f._.Function.nParams} {f._.Function.nParams}");
                    break;
                case MC_RULE:
                    IDU_ = StackSem.Peek();
                    f = IDU_._.ID.obj;
                    MC_ = new();
                    if (f.eKind != FUNCTION_) {
                        Error(ERR_KIND_NOT_FUNCTION, currentLine);
                        MC_._.MC.type = pUniversal;
                        MC_._.MC.param = null;
                        MC_._.MC.err = true;
                    } else {
                        MC_._.MC.type = f._.Function.pRetType;
                        MC_._.MC.param = f._.Function.pParams;
                        MC_._.MC.err = false;
                    }
                    MC_.nont = MC;
                    StackSem.Push(MC_);
                    break;
                case NF_RULE:
                    IDD_ = StackSem.Peek();
                    f = IDD_._.ID.obj;
                    f.eKind = FUNCTION_;
                    f._.Function.nParams = 0;
                    f._.Function.nVars = 0;
                    f._.Function.nIndex = nFuncs++;
                    newBlock();
                    break;
                default:
                    break;
            }
        }
    }
}
