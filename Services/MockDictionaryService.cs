using System.Collections.Generic;
using System;

public class MockDictionaryService
{
    private readonly Dictionary<string, DictionaryEntry> _dictionary;

    public MockDictionaryService()
    {
        _dictionary = new Dictionary<string, DictionaryEntry>(StringComparer.OrdinalIgnoreCase)
        {
            {
                "to", new DictionaryEntry
                {
                    Word = "continent",
                    Pronunciation = "/ˈkɒntɪnənt/",
                    WordClass = "noun",
                    Meaning = "Any of the world's main continuous expanses of land",
                    RelatedWords = new List<string>
                    {
                        "continental (adj)",
                        "intercontinental (adj)",
                        "subcontinental (adj)"
                    },
                    VietnameseMeaning = "Lục địa, châu lục",
                    Examples = new List<string>
                    {
                        "The continent of Asia is Earth's largest landmass.",
                        "The seven continents are Asia, Africa, North America, South America, Antarctica, Europe, and Australia."
                    }
                }
            },
            {
                "have", new DictionaryEntry
                {
                    Word = "drift",
                    Pronunciation = "/drɪft/",
                    WordClass = "verb, noun",
                    Meaning = "To be carried slowly by a current of air or water",
                    RelatedWords = new List<string>
                    {
                        "drifting (n)",
                        "drifter (n)",
                        "adrift (adj)"
                    },
                    VietnameseMeaning = "Trôi dạt, di chuyển chậm",
                    Examples = new List<string>
                    {
                        "The boat drifted away from the shore.",
                        "Continental drift is the movement of Earth's continents relative to each other."
                    }
                }
            }
            // Thêm các từ khác tương tự
        };
    }

    public DictionaryEntry GetWord(string word)
    {
        return _dictionary.TryGetValue(word, out var entry) ? entry : null;
    }
}