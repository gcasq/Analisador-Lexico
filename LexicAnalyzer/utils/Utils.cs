using static LexicAnalyzer.Enums;

namespace LexicAnalyzer
{
    public class Utils {
        public static t_token searchKeyword(string name) {
            t_token token;
            try {
                token = (t_token)System.Enum.Parse(typeof(t_token), name, true);
            }
            catch (ArgumentException e) {
                return t_token.ID;
            }
            return token;
        }
    }
}
