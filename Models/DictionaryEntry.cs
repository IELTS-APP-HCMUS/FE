using System.Collections.Generic;

public class DictionaryEntry
{
    public string Word { get; set; }
    public string Pronunciation { get; set; }
    public string PartOfSpeech { get; set; } // Loại từ
    public string Meaning { get; set; }
    public List<string> RelatedWords { get; set; } // Từ/cấu trúc liên quan
    public string VietnameseMeaning { get; set; }
    public List<string> Examples { get; set; }
} 