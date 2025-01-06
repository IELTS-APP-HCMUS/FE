using Newtonsoft.Json;
using System.Collections.Generic;
/// <summary>
/// Đại diện cho một mục từ điển với các thông tin chi tiết.
/// </summary>
public class DictionaryEntry
{
    public string Word { get; set; }
    public string Pronunciation { get; set; }
    public string WordClass { get; set; } 
    public string Meaning { get; set; }
	public string Explanation { get; set; } 
    public List<string> RelatedWords { get; set; } 
    public string VietnameseMeaning { get; set; }
    public List<string> Examples { get; set; }
}


/// <summary>
/// Đại diện cho dữ liệu API với các thông tin được ánh xạ từ JSON.
/// </summary>
public class ApiData
{
	[JsonProperty("word_display")]
	public string WordDisplay { get; set; }
	[JsonProperty("word_class")]
	public string WordClass { get; set; }
	[JsonProperty("meaning")]
	public string Meaning { get; set; }
	[JsonProperty("ipa")]
	public string Ipa { get; set; }

	[JsonProperty("explanation")]
	public string Explanation { get; set; }
	[JsonProperty("collocation")]
	public string Collocation { get; set; }
	[JsonProperty("example")]
	public List<string> Example { get; set; }
}