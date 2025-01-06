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
    /// <summary>
    ///Service quản lý các thao tác với từ vựng thông qua API.
    // Cung cấp các chức năng thêm, lấy, cập nhật và xóa từ vựng.
    // </summary>
    public class VocabularyService
    {
        /// <summary>
        /// Đối tượng gọi API
        /// </summary>
        private readonly ClientCaller clientCaller = new();
        /// <summary>
        /// URL cơ bản của API từ vựng
        /// </summary>
        private readonly string apiUrl = "v1/vocabs";

  
        /// 
        /// <summary>
        /// Thêm từ vựng mới vào hệ thống thông qua API.
        /// </summary>
        /// <param name="newVocab">Đối tượng từ vựng cần thêm</param>
        /// <returns>Trả về true nếu thêm thành công, ngược lại false</returns>
        /// <remarks>
        /// Sử dụng phương thức POST để gửi dữ liệu từ vựng mới.
        /// </remarks>
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
        /// Lấy danh sách từ vựng từ API.
        /// </summary>
        /// <returns>Danh sách các đối tượng VocabularyItem</returns>
        /// <remarks>
        /// Sử dụng phương thức GET để lấy dữ liệu từ API.
        /// </remarks>
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

        /// <summary>
        /// Lấy từ vựng theo khóa từ API.
        /// </summary>
        /// <param name="key">Khóa của từ vựng cần lấy</param>
        /// <returns>Trả về true nếu lấy thành công, ngược lại false</returns>
        public async Task<bool> GetVocabularyByKeyAsync(string key)
        {
            try
            {
                string get_apiUrl = $"{apiUrl}?key={key}";
                HttpResponseMessage response = await clientCaller.GetAsync(get_apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine($"Failed to fetch schedules. Status Code: {response.StatusCode}");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching schedules: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Cập nhật thông tin từ vựng thông qua API.
        /// </summary>
        /// <param name="vocab">Đối tượng từ vựng cần cập nhật</param>
        /// <returns>Trả về true nếu cập nhật thành công, ngược lại false</returns>
        /// <remarks>
        /// Sử dụng phương thức PATCH để cập nhật dữ liệu từ vựng.
        /// </remarks>
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

        /// <summary>
        /// Xóa từ vựng khỏi hệ thống thông qua API.
        /// </summary>
        /// <param name="word">Từ vựng cần xóa</param>
        /// <returns>Trả về true nếu xóa thành công, ngược lại false</returns>
        /// <remarks>
        /// Sử dụng phương thức DELETE để xóa dữ liệu từ vựng.
        /// </remarks>
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
