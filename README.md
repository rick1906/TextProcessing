# TextProcessing
A C# library for processing text and extracting numeric values from it. Provides regular-expression-like interfaces to build a pattern sequence used for parsing text. For parsing and storing numeric values `NumberProcessing` library is used.

The project was created for extracting numeric values from scietific articles and data tables converted to plain or html-like formats. Patterns can be extended and configured to find standard wording/formatting for certain scientific fields, and then extract numeric data. The base classes are published here under free licence.

#### Base pattern classes
`PatternSequence` - Pattern for a sequence of elements, each matching it's own pattern.

`PatternBlock` - Pattern for multiple occurrences of the same element. Min and max occurrences count is configured.

`PatternVariants` - Pattern for an element matching any provided pattern.

`PatternUnion` - Pattern for an element matching multiple patterns.

`SimplePattern` - Pattern for a simple single element like word or number. `NumberPattern`, `StringPattern` and other basic patterns are derived from this class.

#### Usage example
````cs
string s1 = "We bought 1.5 kg of apples, 2 bananas and 20(1) kg of potatoes.";
string s2 = "They bought 2.1±0.2 kg of oranges and 1.3E2 kg of tomatoes.";

StringPattern word = new StringPattern("\\s*([a-z]+)\\s*", RegexOptions.IgnoreCase);
PatternUnion wordNotAnd = new PatternUnion() {
    word,
    new StringPattern("^((?!and).)+", RegexOptions.IgnoreCase),
};
PatternBlock wordsNotAnd = new PatternBlock(wordNotAnd, 1, -1);
PatternVariants commaOrAnd = new PatternVariants() {
    new StringPattern("\\s*,\\s*"),
    new StringPattern("\\s*and\\s+", RegexOptions.IgnoreCase),
};
PatternSequence ofSomething = new PatternSequence() {
    new StringPattern("\\s+of\\s+"),
    wordsNotAnd.Copy("name"),
};
PatternSequence massWithSep = new PatternSequence() {
    new NumberContainerPattern(n => n.Unit == "kg") { Name = "mass" },
    ofSomething,
    new PatternBlock(commaOrAnd, 0, 1),
};
PatternSequence numWithSep = new PatternSequence() {
    new NumberPattern() { Name = "count" },
    wordsNotAnd.Copy("name"),
    new PatternBlock(commaOrAnd, 0, 1),
};

PatternSequence mainSequence = new PatternSequence();
mainSequence.Add(new StringPattern(".+\\sbought\\s+", RegexOptions.IgnoreCase));
mainSequence.Add(new PatternVariants() { massWithSep, numWithSep }, '+');
mainSequence.Add("\\.", '?');

ResultSequence result1 = mainSequence.Match(s1);
ResultSequence result2 = mainSequence.Match(s2);
Console.WriteLine(result1 != null ? result1.ToString(true) : "NULL");
Console.WriteLine(result2 != null ? result2.ToString(true) : "NULL");
if (result1 != null && result1.GetResults("mass").Length == 2) {
    Console.WriteLine(((NumberContainer)result1.GetResults("mass")[0].GetValue()).ToString());
    Console.WriteLine(((NumberContainer)result1.GetResults("mass")[1].GetValue()).ToString());
}
if (result2 != null && result2.GetResults("mass").Length == 2) {
    Console.WriteLine(((NumberContainer)result2.GetResults("mass")[0].GetValue()).ToString());
    Console.WriteLine(((NumberContainer)result2.GetResults("mass")[1].GetValue()).ToString());
}
````
Outputs:
````
[{We bought },[[mass:(NumberContainer){1.5 kg},[{ of },name:{apples}],{, }],[count:(Double){2},name:{ bananas },{and }],[mass:(NumberContainer){20(1) kg},[{ of },name:{potatoes}],{}]],{.}]
[{They bought },[[mass:(NumberContainer){2.1±0.2 kg},[{ of },name:{oranges }],{and }],[mass:(NumberContainer){1.3E2 kg},[{ of },name:{tomatoes}],{}]],{.}]
1.5 kg
20 (1) kg
2.1 (2) kg
130 kg
````
