using System.Net;
using System.Text.RegularExpressions;
using BCDev.Text;

namespace BCDev.Canadian;

[Flags] enum Endings { None = 0, S = 1, Ed = 2, SEd = 3, Ing = 4, SIng = 5, EdIng = 6, SEdIng = 7 }

public class RegexList {
    readonly List<(Regex Regex, string Replacement)> textReplacements = [];
    readonly List<(string From, string To, string Endings)> documentation = [];
    readonly bool americanize;

    void SubWithCase(string from, string to) {
        var lc = from[0];
        var uc = from.ToUpperInvariant()[0];
        var rest = from[1..];
        var tolc = to[0];
        var touc = to.ToUpperInvariant()[0];
        var torest = to[1..];
        textReplacements.Add((new Regex($"\\b([{lc}{uc}]){rest}\\b"), $"$1{to[1..]}"));
    }

    void SubWithConjugations(string from, string to, Endings endings, string reason, string? conjugate = null) {
        SubWithCase(from, to);
        if (conjugate != null) {
            var fromPrefix = Regex.Replace(from, $"{conjugate}$", "");
            var toPrefix = Regex.Replace(to, $"{conjugate}$", "");
            if (endings.HasFlag(Endings.S))
                SubWithCase(fromPrefix + Conjugation.GetPlural(conjugate), toPrefix + Conjugation.GetPlural(conjugate));
            if (endings.HasFlag(Endings.Ed))
                SubWithCase(fromPrefix + Conjugation.GetPast(conjugate), toPrefix + Conjugation.GetPast(conjugate));
            if (endings.HasFlag(Endings.Ing))
                SubWithCase(fromPrefix + Conjugation.GetParticiple(conjugate), toPrefix + Conjugation.GetParticiple(conjugate));
        } else {
            if (endings.HasFlag(Endings.S))
                SubWithCase(Conjugation.GetPlural(from), Conjugation.GetPlural(to));
            if (endings.HasFlag(Endings.Ed))
                SubWithCase(Conjugation.GetPast(from), Conjugation.GetPast(to));
            if (endings.HasFlag(Endings.Ing))
                SubWithCase(Conjugation.GetParticiple(from), Conjugation.GetParticiple(to));
        }
        var endingText = new List<string>();
        if (endings.HasFlag(Endings.S)) endingText.Add("-s");
        if (endings.HasFlag(Endings.Ed)) endingText.Add("-ed");
        if (endings.HasFlag(Endings.Ing)) endingText.Add("-ing");
        documentation.Add((from, to, string.Join(", ", endingText)));
    }

    void Spelling(string from, string to, Endings endings = Endings.None) =>
        SubWithConjugations(from, to, endings, "spelling");

    void Regional(string from, string to, Endings endings = Endings.None) {
        if (americanize)
            SubWithConjugations(from, to, endings, "regional");
        else
            SubWithConjugations(to, from, endings, "regional");
    }

    public RegexList(bool americanize) {
        this.americanize = americanize;

        if (americanize) {
            Spelling("anaesthetic", "anesthetic", Endings.S);
            Spelling("formulae", "formulas");
            Spelling("licence", "license", Endings.S);
            Spelling("likeable", "likable");
            Spelling("metre", "meter", Endings.S);
            Spelling("practise", "practice", Endings.SEdIng);
            Spelling("cheque", "check", Endings.SIng);
            Spelling("enquire", "inquire", Endings.SEdIng);
            Spelling("enquiry", "inquiry", Endings.S);
            Spelling("saleable", "salable");
            Spelling("storey", "story", Endings.S);
        } else {
            Spelling("license revenue", "licence revenue");
            Spelling("practiced", "practised");
            Spelling("practicing", "practising");
        }

        Regional("behaviour", "behavior", Endings.S);
        Regional("behavioural", "behavioral");
        Regional("belabour", "belabor", Endings.SEdIng);
        Regional("benefitted", "benefited");
        Regional("benefitting", "benefiting");
        Regional("calibre", "caliber", Endings.S);
        Regional("cancellable", "cancelable");
        Regional("cancellation", "cancelation", Endings.S);
        Regional("cancelled", "canceled");
        Regional("cancelling", "canceling");
        Regional("catalog", "catalogue", Endings.SEdIng);
        Regional("centimetre", "centimeter", Endings.S);
        Regional("centre", "center", Endings.S);
        Regional("centred", "centered");
        Regional("centring", "centering");
        Regional("counselled", "counseled");
        Regional("counselling", "counseling");
        Regional("chequing account", "checking account", Endings.S);
        Regional("chequings account", "checkings account", Endings.S);
        Regional("chequebook", "checkbook", Endings.S);
        Regional("colour", "color", Endings.SEdIng);
        Regional("combatting", "combating");
        Regional("defence", "defense", Endings.S);
        Regional("dialled", "dialed");
        Regional("dialling", "dialing");
        Regional("discolour", "discolor", Endings.SEdIng);
        Regional("discolouration", "discoloration", Endings.S);
        Regional("endeavour", "endeavor", Endings.SEdIng);
        Regional("epicentre", "epicenter", Endings.S);
        Regional("favour", "favor", Endings.SEdIng);
        Regional("favourable", "favorable");
        Regional("favourably", "favorably");
        Regional("fibre", "fiber");
        Regional("unfavourable", "unfavorable");
        Regional("flavour", "flavor", Endings.SEdIng);
        Regional("grey", "gray", Endings.SEdIng);
        Regional("honour", "honor", Endings.SEdIng);
        Regional("honourable", "honorable");
        Regional("honourably", "honorably");
        Regional("honourary",  "honorary");
        Regional("humour", "humor", Endings.SEdIng);
        Regional("humourous", "humorous");
        Regional("humourously", "humorously");
        Regional("kilometre", "kilometer", Endings.S);
        Regional("labelled", "labeled");
        Regional("labelling", "labeling");
        Regional("labour", "labor", Endings.SEdIng);
        Regional("labourious", "laborious");
        Regional("litre", "liter", Endings.S);
        Regional("marshalled", "marshaled");
        Regional("marshalling", "marshaling");
        Regional("millilitre", "milliliter", Endings.S);
        Regional("millimetre", "millimeter", Endings.S);
        Regional("misdemeanour", "misdemeanor", Endings.S);
        Regional("modelling", "modeling");
        Regional("neighbour", "neighbor", Endings.S);
        Regional("neighbourhood", "neighborhood");
        Regional("odour", "odor", Endings.S);
        Regional("odourless", "odorless");
        Regional("offence", "offense", Endings.S);
        Regional("paycheque", "paycheck", Endings.S);
        Regional("practised", "practiced");
        Regional("practising", "practicing");
        Regional("rumour", "rumor", Endings.S);
        Regional("rumoured", "rumored", Endings.S);
        Regional("sombre", "somber");
        Regional("sombrely", "somberly");
        Regional("splendour", "splendor", Endings.S);
        Regional("theatre", "theater", Endings.S);
        Regional("totalled", "totaled");
        Regional("totalling", "totaling");
        Regional("travelled", "traveled");
        Regional("travelling", "traveling");
        Regional("tumour", "tumor", Endings.S);
        Regional("vapour", "vapor", Endings.S);
    }

    public string ToHTML() => $"<table id='{(americanize ? "americanize" : "canadianize")}-documentation'>\n" +
        "  <thead>\n    <tr><th>From</th><th>To</th><th>Endings</th></tr>\n  </thead>\n  <tbody>\n" +
        string.Join('\n', documentation.OrderBy(d => d.From).Select(d =>
            $"    <tr><td>{WebUtility.HtmlEncode(d.From)}</td><td>{WebUtility.HtmlEncode(d.To)}</td><td>{WebUtility.HtmlEncode(d.Endings)}</td></tr>")) +
        "\n  </tbody>\n</table>";
    public string ToJavaScript() => $"const {(americanize ? "americanize" : "canadianize")} = [\n" +
        string.Join($",{Environment.NewLine}", textReplacements.Select(r =>
            $"    {{re: /{r.Regex}/g, s: '{r.Replacement}'}}")) +
        "\n];";

}
