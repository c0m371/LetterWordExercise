using LetterWordExercise;

var input = new List<string>();
string? line = Console.ReadLine();
while (!string.IsNullOrWhiteSpace(line))
{
    input.Add(line.Trim());
    line = Console.ReadLine();
}

var targetWordLength = input.Max(i => i.Length);
var targetWords = input.Where(i => i.Length == targetWordLength).ToList();
var parts = input.Where(i => i.Length < targetWordLength).ToList();

var solver = new WordSolver(targetWords, parts);
var words = solver.GetWords();

foreach (var word in words)
foreach (var combination in word.Combinations)
    Console.WriteLine($"{string.Join("+", combination)}={word.Value}");
