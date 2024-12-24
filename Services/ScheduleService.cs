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

namespace login_full.Services
{
    public class ScheduleService
    {
        private static readonly HttpClient HttpClient = new HttpClient();
        /// <summary>
        /// Gửi một lịch trình mới đến API bằng phương thức POST.
        /// </summary>
        public static async Task<bool> AddScheduleAsync(ScheduleItem newSchedule)
        {
            try
            {
                string apiUrl = "http://localhost:8080/v1/plans"; // Thay thế bằng API thực tế

                string json = JsonSerializer.Serialize(newSchedule);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string accessToken = GlobalState.Instance.AccessToken;

                // Add Bearer Token for Authorization
                HttpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage response = await HttpClient.PostAsync(apiUrl, content);

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
        public static async Task<List<ScheduleItem>> GetSchedulesAsync()
        {
            try
            {
                // Thay thế URL bằng endpoint API thực tế của bạn
                string apiUrl = "http://localhost:8080/v1/plans";

                string accessToken = GlobalState.Instance.AccessToken;

                // Add Bearer Token for Authorization
                HttpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                HttpResponseMessage response = await HttpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string stringResponse = await response.Content.ReadAsStringAsync();

                    var jsonResponse = JObject.Parse(stringResponse);
                    var schedules = new List<ScheduleItem>();
                    var data = jsonResponse["data"].ToList();
                    data.ForEach(x => 
                        schedules.Add(new ScheduleItem
                        {
                            Time = x["time"].ToString(),
                            Activity = x["activity"].ToString()
                        })
                    );

                    return schedules ?? [];
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
    }
}
