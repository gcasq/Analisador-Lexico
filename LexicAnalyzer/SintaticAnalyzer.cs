using static LexicAnalyzer.utils.SemanticAnalyserTables;

namespace LexicAnalyzer {
    public class SintaticAnalyzer {

        public static void synteticAnalizer() {
            int finalState = 1;
            int q = 0;
            state.push(q);
            int a = nextToken();
            do {
                int p = actionTable[q][a];
                if (IS_SHIFT(p)) {
                    state.push(p);
                    a = nextToken();
                } else if (IS_REDUCTION(p)) {
                    int r = Rule(p);
                    for (int i = 0; i < ruleLen[r]; i++) {
                        state.pop();
                    }
                    state.push(actionTable[state.top()][ruleLeft[r]]);
                } else {
                    syntaxError();
                }
                q = state.top();
            } while (q != finalState);
        }
    }
}
