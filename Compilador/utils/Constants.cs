namespace LexicAnalyzer.utils {
    public static class RulesConstants {
        public const int MAX_STACK = 1000;

        public const int CHR_RULE = 2;

        public const int DC_TO_DC = 3;
        public const int DC_TO_LI = 4;

        public const int DF_TO_FUNC = 7;

        public const int DT_TO_ARRAY = 8;
        public const int DT_TO_STRUCT = 9;
        public const int DT_TO_IDD = 10;

        public const int DV_TO_VAR = 11;

        public const int E_TO_AND = 12;
        public const int E_TO_OR = 13;
        public const int E_TO_L = 14;

        public const int F_TO_LV = 15;
        public const int F_TO_PLUS_LV = 16;
        public const int F_TO_MINUS_LV = 17;
        public const int F_TO_LV_PLUS = 18;
        public const int F_TO_LV_MINUS = 19;
        public const int F_TO_E = 20;
        public const int F_TO_IDU = 21;
        public const int F_TO_MINUS_F = 22;
        public const int F_TO_DIF_F = 23;
        public const int F_TO_TRUE = 24;
        public const int F_TO_FALSE = 25;
        public const int F_TO_CHR = 26;
        public const int F_TO_STR = 27;
        public const int F_TO_NUM = 28;

        public const int FALSE_RULE = 29;

        public const int ID_RULE = 30;
        public const int IDD_RULE = 31;
        public const int IDU_RULE = 32;

        public const int L_TO_LESS_THAN = 33;
        public const int L_TO_GREATER_THAN = 34;
        public const int L_TO_LESS_EQUAL = 35;
        public const int L_TO_GREATER_EQUAL = 36;
        public const int L_TO_EQUAL = 37;
        public const int L_TO_DIFF = 38;
        public const int L_TO_R = 39;

        public const int LE_TO_LE = 44;
        public const int LE_TO_E = 45;

        public const int LI_TO_LI_COMMA = 46;
        public const int LI_TO_IDD = 47;

        public const int LP_TO_LP = 48;
        public const int LP_TO_IDD = 49;

        public const int LV_TO_LV_IDU = 52;
        public const int LV_TO_LV_E = 53;
        public const int LV_TO_IDU = 54;

        public const int MC_RULE = 55;
        public const int MF_RULE = 56;
        public const int NB_RULE = 57;
        public const int NF_RULE = 58;

        public const int NUM_RULE = 59;

        public const int R_TO_PLUS = 61;
        public const int R_TO_MINUS = 62;
        public const int R_TO_Y = 63;

        public const int S_TO_IF = 64;
        public const int S_TO_IF_ELSE = 65;
        public const int S_TO_WHILE = 66;
        public const int S_TO_DO_WHILE = 67;
        public const int S_TO_NB = 68;
        public const int S_TO_LV = 69;
        public const int S_TO_BREAK = 70;
        public const int S_TO_CONTINUE = 71;

        public const int STR_RULE = 72;

        public const int T_TO_INTEGER = 73;
        public const int T_TO_CHR = 74;
        public const int T_TO_BOOL = 75;
        public const int T_TO_STR = 76;
        public const int T_TO_IDU = 77;

        public const int TRUE_RULE = 78;

        public const int Y_TO_TIMES = 79;
        public const int Y_TO_DIVIDE = 80;
        public const int Y_TO_F = 81;
    }
}
