using static LexicAnalyzer.utils.Enums;
using static LexicAnalyzer.utils.Enums.LeftRules;
namespace LexicAnalyzer.utils {
    public class SintaticAnalyzerTables {
        public List<LeftRules> ruleLeft = new List<LeftRules> { P, LDE, LDE, DE, DE, DE, DF, DT, DT, DT, DC, DC, LI, LI, DV, LP, LP, LP, B, B, B, B, LDV, LDV, LS, LS, S, S, S, S, S, S, S, S, S, E, E, E, E, L, L, L, L, L, L, L, R, R, R, K, K, K, F, F, F, F, F, F, F, F, F, F, F, F, F, F, LE, LE, LE, LV, LV, LV, T, T, T, T, T, TRU, FALS, CHR, STR, NUM, IDD, IDU, ID, NB, MF, MC, NF, MT, ME, MW, MA };
        public List<int> ruleLen = new List<int> { 2, 2, 2, 1, 1, 1, 1, 10, 9, 10, 5, 5, 3, 3, 1, 5, 5, 3, 0, 4, 3, 3, 2, 2, 1, 2, 1, 2, 2, 7, 8, 6, 9, 2, 2, 3, 3, 3, 1, 4, 3, 3, 3, 3, 3, 3, 1, 3, 3, 1, 3, 3, 1, 1, 2, 2, 2, 2, 3, 5, 2, 2, 1, 1, 1, 1, 1, 3, 1, 0, 3, 4, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0, 0, 0, 0, 0, 0, 0, 0 };
    }
}
