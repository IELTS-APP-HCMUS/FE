using System.Collections.Generic;
using System;
using login_full.API_Services;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Microsoft.ML.Tokenizers;
using SharpToken;
using login_full.Models;


public class DictionaryService
{
	private readonly Dictionary<string, DictionaryEntry> _dictionary;
	private readonly ClientCaller _clientCaller;

	public DictionaryService()
	{
		_dictionary = new Dictionary<string, DictionaryEntry>(StringComparer.OrdinalIgnoreCase){};
		_clientCaller = new ClientCaller();
	}
	public async Task<DictionaryEntry> FetchWordFromApiAsync(int quizId, int sentenceIndex, int wordIndex, string word)
	{
		try
		{
			string url = $"/v1/vocabs/reading?quiz_id={quizId}&sentence_index={sentenceIndex}&vocab_index={wordIndex}&word={word}";
			HttpResponseMessage response = await _clientCaller.GetAsync(url);
			System.Diagnostics.Debug.WriteLine($"Response: {response}");

			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();
				var apiResponse = JsonConvert.DeserializeObject<ApiResponse<ApiData>>(content);

				if (apiResponse?.Data != null)
				{
					var data = apiResponse.Data;
					return new DictionaryEntry
					{
						Word = data.WordDisplay,
						Pronunciation = data.Ipa,
						WordClass = data.WordClass,
						Explanation = data.Explanation,
						Meaning = data.Meaning,
						RelatedWords = new List<string> { data.Collocation },
						VietnameseMeaning = data.Meaning,
						Examples = data.Example
					};
				}
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error fetching word details: {ex.Message}");
		}

		return null;
	}

	public DictionaryEntry GetWord(string word)
	{
		return _dictionary.TryGetValue(word, out var entry) ? entry : null;
	}

	public Dictionary<string, List<(int sentenceIndex, int wordIndex)>> GenerateWordIndices(string content)
	{
		var wordIndexMap = new Dictionary<string, List<(int, int)>>(StringComparer.OrdinalIgnoreCase);

		var sentences = Regex.Split(content, @"(?<=[.!?])(?<!Mr|Mrs|Dr|Ms|Jr)\s+(?=[A-Z])|\n{2,}");

		for (int sentenceIndex = 0; sentenceIndex < sentences.Length; sentenceIndex++)
		{
			var sentence = sentences[sentenceIndex].Trim();
			if (string.IsNullOrEmpty(sentence)) continue;

			var words = Regex.Split(sentence, @"(?<!\w)-|[^\w'-]+");

			for (int wordIndex = 0; wordIndex < words.Length; wordIndex++)
			{
				string word = words[wordIndex].Trim();
				if (string.IsNullOrEmpty(word)) continue;

				if (!wordIndexMap.ContainsKey(word))
				{
					wordIndexMap[word] = new List<(int, int)>();
				}

				// Lưu vị trí với chỉ số gốc
				wordIndexMap[word].Add((sentenceIndex + 1, wordIndex +1));
			}
		}

		return wordIndexMap;
	}
	public string GetVocabId(string word, string content, int quizId)
	{
		var indices = GenerateWordIndices(content);
		if (indices.ContainsKey(word))
		{
			var position = indices[word][0];
			return $"{quizId}_{position.sentenceIndex}_{position.wordIndex}";
		}
		return null;
	}

	public async Task<bool> AddVocabularyAsync(VocabularyItem vocab)
	{
		try
		{
			var content = ClientCaller.GetContent(vocab);
			HttpResponseMessage response = await _clientCaller.PostAsync("/v1/vocabs", content);
			if (response.IsSuccessStatusCode)
			{
				return true; 
			}
			else
			{
				Console.WriteLine($"Failed to add vocabulary. Status Code: {response.StatusCode}");
				return false;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error adding vocabulary: {ex.Message}");
			return false;
		}
	}
}