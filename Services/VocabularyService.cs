using login_full.Context;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Windows.Media.Protection.PlayReady;
using login_full.Models;
using Newtonsoft.Json.Linq;
using System.Linq;
using login_full.API_Services;

namespace login_full.Services
{
    public class VocabularyService
    {
        private readonly ClientCaller clientCaller = new();
        private readonly string apiUrl = "v1/vocabs";

        /// <summary>
        /// Gửi một lịch trình mới đến API bằng phương thức POST.
        /// </summary>
        public async Task<bool> AddVocabularyAsync(VocabularyItem newVocab)
        {
            try
            {
                var content = ClientCaller.GetContent(newVocab);

                HttpResponseMessage response = await clientCaller.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Thành công
                }
                else
                {
                    Console.WriteLine($"Failed to add schedule. Status Code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding schedule: {ex.Message}");
                return false;
            }
        }
        /// <summary>
        /// Gọi API để lấy danh sách lịch trình.
        /// </summary>
        /// <returns>Danh sách các đối tượng ScheduleItem.</returns>
        public async Task<List<VocabularyItem>> GetVocabularyAsync()
        {
            try
            {
                HttpResponseMessage response = await clientCaller.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string stringResponse = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(stringResponse);
                    var vocabs = new List<VocabularyItem>();
                    var data = jsonResponse["data"].ToList();
                    data.ForEach(x =>
                        vocabs.Add(new VocabularyItem
                        {
                            WordKey = x["key"]?.ToString(),
                            Word = x["value"]?.ToString(),
                            WordType = x["word_class"]?.ToString(),
                            Meaning = x["meaning"]?.ToString(),
                            Example = x["example"]?.ToString(),
                            Status = x["is_learned_status"]?.Type == JTokenType.Boolean && x["is_learned_status"].ToObject<bool>() ? "Đã học" : "Đang học",
                            Note = x["explanation"]?.ToString()
                        })
                    );

                    return vocabs ?? [];
                }
                else
                {
                    Console.WriteLine($"Failed to fetch schedules. Status Code: {response.StatusCode}");
                    return [];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching schedules: {ex.Message}");
                return [];
            }
        }
        public async Task<bool> UpdateVocabularyAsync(VocabularyItem vocab)
        {
            try
            {
                string patch_apiUrl = $"{apiUrl}?key={vocab.WordKey}"; // Thay thế bằng API thực tế

                var content = ClientCaller.GetContent(vocab);

                HttpResponseMessage response = await clientCaller.PatchAsync(patch_apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Thành công
                }
                else
                {
                    Console.WriteLine($"Failed to update schedule. Status Code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating schedule: {ex.Message}");
                return false;
            }
        }
        public async Task<bool> DeleteVocabularyAsync(string word)
        {
            try
            {
                string delete_apiUrl = $"{apiUrl}?key={word}"; // Thay thế bằng API thực tế

                HttpResponseMessage response = await clientCaller.DeleteAsync(delete_apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return true; // Thành công
                }
                else
                {
                    Console.WriteLine($"Failed to delete schedule. Status Code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting schedule: {ex.Message}");
                return false;
            }
        }
    }
}
