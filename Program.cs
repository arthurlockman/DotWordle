// See https://aka.ms/new-console-template for more information

var words = await File.ReadAllLinesAsync("words.txt");
var randomSeed = new Random();
var bannedLetters = new List<char>();
var knownLetters = new List<char>();
var confirmedLetters = new Dictionary<int, char>();
var knownBadPositions = new Dictionary<int, char>();

Console.WriteLine("Welcome to DotWordle! Use this key for your answer." +
                  "\n\nUse '-' to indicate a missed letter." +
                  "\nUse 'x' to indicate a yellow (included) letter." +
                  "\nUse 'c' to indicate a green (confirmed) letter.");

var firstWord = words[randomSeed.Next(words.Length - 1)];


Console.WriteLine($"\n\nEnter '{firstWord}' as your first word.");

SolveGame(firstWord);

void SolveGame(string startingWord, int iterations = 4)
{
    Console.WriteLine("What was the result?");

    var result = Console.ReadLine();

    foreach (var character in result.Select((c, i) => new {c, i}))
    {
        var l = startingWord[character.i];
        switch (character.c)
        {
            case '-':
                bannedLetters.Add(l);
                break;
            case 'x':
            case 'X':
                knownLetters.Add(l);
                knownBadPositions[character.i] = l;
                break;
            case 'c':
            case 'C':
                confirmedLetters[character.i] = l;
                break;
        }
    }

    bannedLetters.RemoveAll(c => knownLetters.Contains(c));

    var nextWords = words
        .Where(w => knownLetters.All(w.Contains))
        .Where(w => !bannedLetters.Any(w.Contains))
        .Where(w => knownBadPositions.All(cl => w[cl.Key] != cl.Value))
        .Where(w => confirmedLetters.All(cl => w[cl.Key] == cl.Value))
        .ToList();

    var nextWord = nextWords[randomSeed.Next(nextWords.Count - 1)];

    if (iterations > 0)
    {
        Console.WriteLine($"Enter '{nextWord}' as your next word.");
        SolveGame(nextWord, iterations - 1);
    }
    else
    {
        Console.Write($"The answer is '{nextWord}'");
    }
}