using BCDev.Canadian;

var americanize = new RegexList(true);
var canadianize = new RegexList(false);
File.WriteAllText("replacements.js", $"""
    export const version = '{DateTime.UtcNow:u}';
    export {americanize.ToJavaScript()}
    export {canadianize.ToJavaScript()}
    """);
File.WriteAllText("documentation.html", $"""
    {americanize.ToHTML()}
    {canadianize.ToHTML()}
    """);
