namespace LetterWordExercise
{
    public class WordSolver
    {
        private readonly List<string> words;
        private readonly Dictionary<string, int> partCounts;
        private readonly Dictionary<(string word, string partKey), List<List<string>>> cache;

        public WordSolver(List<string> words, List<string> parts)
        {
            this.words = words;
            partCounts = parts.GroupBy(p => p).ToDictionary(g => g.Key, g => g.Count());
            cache = [];
        }

        public List<Word> GetWords()
        {
            var result = new List<Word>();

            foreach (var word in words)
            {
                var combinations = AppendParts(word, partCounts);
                var distinct = combinations
                    .Select(c => string.Join(",", c))
                    .Distinct()
                    .Select(c => c.Split(',').ToList())
                    .ToList();

                result.Add(new Word(word, distinct));
            }

            return result;
        }

        private List<List<string>> AppendParts(string word, Dictionary<string, int> partCounts)
        {
            if (word.Length == 0)
                return [new List<string>()];

            var key = (
                word,
                string.Join(",", partCounts.OrderBy(p => p.Key).Select(p => $"{p.Key}:{p.Value}"))
            );
            if (cache.TryGetValue(key, out var cached))
                return cached;

            var result = new List<List<string>>();

            foreach (var partCount in partCounts)
            {
                var part = partCount.Key;
                var count = partCount.Value;

                if (count == 0 || !word.StartsWith(part))
                    continue;

                var remainingWord = word.Substring(part.Length);

                partCounts[part]--;

                var subCombinations = AppendParts(remainingWord, partCounts);

                foreach (var combination in subCombinations)
                {
                    result.Add([part, .. combination]);
                }

                partCounts[part]++;
            }

            cache[key] = result;
            return result;
        }
    }
}
