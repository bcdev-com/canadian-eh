using System.Text.RegularExpressions;

namespace BCDev.Text;

public static class Conjugation {
    readonly static Dictionary<string, string> irregularPlurals = new Dictionary<string, string> {
        {"man", "men"},
        {"woman", "women"},
        {"person", "people"}
    };
    public static string GetPlural(string word) {
        var w = word.ToLowerInvariant();
        if (irregularPlurals.ContainsKey(w))
            return irregularPlurals[w];
        else if (Regex.IsMatch(word, @"(?in)[aeiou]o$"))
            return word + "s";
        else if (Regex.IsMatch(word, @"(?in)o$"))
            return word + "es";
        else if (Regex.IsMatch(word, @"(?n)[aeiou]y$|^[A-Z].*y$"))
            return word + "s";
        else if (Regex.IsMatch(word, @"(?in)y$"))
            return word.Substring(0, word.Length - 1) + "ies";
        else if (Regex.IsMatch(word, @"(?in)(ss|sh|ch)$"))
            return word + "es";
        else
            return word + "s";
    }

    readonly static Dictionary<string, string> irregularPasts = new Dictionary<string, string> {
        {"bring", "brought"},
        {"buy", "bought"},
        {"fight", "fought"},
        {"seek", "sought"},
        {"think", "thought"},
        {"write", "written"},
        {"ride", "ridden"}
    };
    public static string GetPast(string word) {
        var w = word.ToLowerInvariant();
        if (irregularPasts.ContainsKey(w))
            return irregularPasts[w];
        else if (Regex.IsMatch(word, @"(?in)^[^aeiou]+[aeiou][^aeiouwxy]$"))
            return Regex.Replace(word, @"(?i)(.)$", "$1$1ed");
        else if (Regex.IsMatch(word, @"(?in)[aeiou]y$"))
            return word + "ed";
        else if (Regex.IsMatch(word, @"(?in)y$"))
            return word.Substring(0, word.Length - 1) + "ied";
        else if (Regex.IsMatch(word, @"(?in)e$"))
            return word + "d";
        else if (Regex.IsMatch(word, @"(?in)c$"))
            return word + "ked";
        else
            return word + "ed";
    }
    readonly static Dictionary<string, string> irregularParticiples = new Dictionary<string, string> {
    };
    public static string GetParticiple(string word) {
        var w = word.ToLowerInvariant();
        if (irregularParticiples.ContainsKey(w))
            return irregularParticiples[w];
        else if (Regex.IsMatch(word, @"(?in)^[^aeiou]+[aeiou][^aeiouwxy]$"))
            return Regex.Replace(word, @"(?i)(.)$", "$1$1ing");
        else if (Regex.IsMatch(word, @"(?in)ie$"))
            return word.Substring(0, word.Length - 2) + "ying";
        else if (Regex.IsMatch(word, @"(?in)[^eoy]e$"))
            return word.Substring(0, word.Length - 1) + "ing";
        else if (Regex.IsMatch(word, @"(?in)c$"))
            return word + "king";
        else
            return word + "ing";
    }
}
