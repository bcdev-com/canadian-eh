using System.Text.RegularExpressions;
using BCDev.Canadian;

var americanize = new RegexList(true);
var canadianize = new RegexList(false);
var version = DateTime.UtcNow.ToString("u");
File.WriteAllText("docs/replacements.js", $"""
    export const version = '{version}';
    export {americanize.ToJavaScript()}
    export {canadianize.ToJavaScript()}
    """);

var about = File.ReadAllText("about-source.html");
about = Regex.Replace(about, "<!--americanize-->", americanize.ToHTML());
about = Regex.Replace(about, "<!--canadianize-->", canadianize.ToHTML());
about = Regex.Replace(about, "<!--version-->", version);
File.WriteAllText("docs/about.html", about);
