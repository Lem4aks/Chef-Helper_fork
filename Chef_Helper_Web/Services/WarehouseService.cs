using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using System.Threading.Tasks;
using Chef_Helper_API;
using Microsoft.AspNetCore.Components;
using Chef_Helper_API.Models;
using System.Text.Json;
using System.Text;

namespace Chef_Helper_Web.Services
{
    public class WarehouseService : IWarehouseService
    {
        private readonly HttpClient _httpClient;
        public WarehouseService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7114/");
        }
        public async Task<List<Warehouse>> GetWarehouses()
        {
            return await _httpClient.GetFromJsonAsync<List<Warehouse>>("/Warehouse");

        }
        public async Task<Warehouse> GetWarehouse(string ingridientName)
        {
            return await _httpClient.GetFromJsonAsync<Warehouse>($"api/Warehouse/{ingridientName}");
        }

        public async Task<Warehouse> Update(int boxToUpdate, Warehouse updatedWarehouse)
        {
            await _httpClient.PutAsJsonAsync($"api/Warehouse/{boxToUpdate}", updatedWarehouse);
            return updatedWarehouse;
        }
        public async Task<Warehouse> Post(string ingredientName, int quantity)
        {
            var response = await _httpClient.PostAsJsonAsync($"api/Warehouse/{ingredientName}, {quantity}" ,false);

            return await response.Content.ReadFromJsonAsync<Warehouse>();
        }
        public async Task Delete(int boxNumber)
        {
            await _httpClient.DeleteAsync($"api/Warehouse/{boxNumber}");
        }

    }
}
