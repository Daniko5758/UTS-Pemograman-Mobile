using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;
using MyTraining.Shared.Models;

namespace MyTraining.Shared.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;  
        public ApiService(HttpClient http) => _http = http;

        // Trainers
        public Task<List<Trainer>> GetTrainers(string? search = null)
        {
            var url = "api/trainers" + (string.IsNullOrWhiteSpace(search) ? "" : $"?search={Uri.EscapeDataString(search)}");
            return _http.GetFromJsonAsync<List<Trainer>>(url) ?? Task.FromResult(new List<Trainer>());
        }
        public Task<Trainer?> GetTrainer(int id) => _http.GetFromJsonAsync<Trainer>($"api/trainers/{id}");
        public Task<HttpResponseMessage> CreateTrainer(Trainer trainer) => _http.PostAsJsonAsync("api/trainers", trainer);
        public Task<HttpResponseMessage> UpdateTrainer(int id, Trainer trainer) => _http.PutAsJsonAsync($"api/trainers/{id}", trainer);
        public Task<HttpResponseMessage> DeleteTrainer(int id) => _http.DeleteAsync($"api/trainers/{id}");

        // Courses
        public Task<List<Course>> GetCourses(string? search = null)
        {
            var url = "api/courses" + (string.IsNullOrWhiteSpace(search) ? "" : $"?search={Uri.EscapeDataString(search)}");
            return _http.GetFromJsonAsync<List<Course>>(url) ?? Task.FromResult(new List<Course>());
        }
        public Task<Course?> GetCourse(int id) => _http.GetFromJsonAsync<Course>($"api/courses/{id}");
        public Task<HttpResponseMessage> CreateCourse(Course course) => _http.PostAsJsonAsync("api/courses", course);
        public Task<HttpResponseMessage> UpdateCourse(int id, Course course) => _http.PutAsJsonAsync($"api/courses/{id}", course);
        public Task<HttpResponseMessage> DeleteCourse(int id) => _http.DeleteAsync($"api/courses/{id}");
    }
}
