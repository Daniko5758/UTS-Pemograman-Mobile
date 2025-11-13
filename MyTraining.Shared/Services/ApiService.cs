using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using MyTraining.Shared.Models;
using Microsoft.AspNetCore.Components.Forms; 
using System.Text.Json;               

namespace MyTraining.Shared.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;

        public string BaseUrl { get; }

        public ApiService(HttpClient http)
        {
            _http = http;

            BaseUrl = _http.BaseAddress?.ToString() ?? "https://fallback.url.com/";
        }


        public Task<List<Trainer>?> GetTrainers(string? search = null)
        {
            var url = "api/trainers" + (string.IsNullOrWhiteSpace(search) ? "" : $"?search={Uri.EscapeDataString(search)}");
            return _http.GetFromJsonAsync<List<Trainer>>(url);
        }
        public Task<Trainer?> GetTrainer(int id) => _http.GetFromJsonAsync<Trainer>($"api/trainers/{id}");
        public Task<HttpResponseMessage> CreateTrainer(Trainer trainer) => _http.PostAsJsonAsync("api/trainers", trainer);
        public Task<HttpResponseMessage> UpdateTrainer(int id, Trainer trainer) => _http.PutAsJsonAsync($"api/trainers/{id}", trainer);
        public Task<HttpResponseMessage> DeleteTrainer(int id) => _http.DeleteAsync($"api/trainers/{id}");


        public Task<List<Course>?> GetCourses(string? search = null)
        {
            var url = "api/courses" + (string.IsNullOrWhiteSpace(search) ? "" : $"?search={Uri.EscapeDataString(search)}");
            return _http.GetFromJsonAsync<List<Course>>(url);
        }
        public Task<Course?> GetCourse(int id) => _http.GetFromJsonAsync<Course>($"api/courses/{id}");
        public Task<HttpResponseMessage> CreateCourse(Course course) => _http.PostAsJsonAsync("api/courses", course);
        public Task<HttpResponseMessage> UpdateCourse(int id, Course course) => _http.PutAsJsonAsync($"api/courses/{id}", course);
        public Task<HttpResponseMessage> DeleteCourse(int id) => _http.DeleteAsync($"api/courses/{id}");

        public async Task<string?> UploadCourseImageAsync(IBrowserFile file, long maxFileSize = 1024 * 1024 * 5)
        {
            try
            {
                using var content = new MultipartFormDataContent();
                using var streamContent = new StreamContent(file.OpenReadStream(maxFileSize));
                content.Add(streamContent, "file", file.Name);

                var response = await _http.PostAsync("api/courses/upload", content);
                if (!response.IsSuccessStatusCode) return null;

                var jsonResponse = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<UploadResult>(jsonResponse, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return result?.FilePath;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Image upload failed: {ex.Message}");
                return null;
            }
        }
        private class UploadResult { public string? FilePath { get; set; } }
    }
}