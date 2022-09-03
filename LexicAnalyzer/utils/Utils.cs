using static LexicAnalyzer.Enums;

namespace LexicAnalyzer.utils
{
    public class Utils {
        public static t_token SearchKeyWord(string name) {
            t_token token;
            try {
                token = (t_token)System.Enum.Parse(typeof(t_token), name, true);
            }
            catch (ArgumentException e) {
                return t_token.UNKNOWN;
            }
            return token;
        }
    }
}
