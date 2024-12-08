using System.Text.Json;

namespace GA360.Commons.Helpers;

public static class JsonHelper
{
    public class UpperCaseNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return name;
            }

            return char.ToLower(name[0]) + name.Substring(1);
        }
    }
}
