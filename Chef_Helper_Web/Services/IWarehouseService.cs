using Chef_Helper_API;
using Chef_Helper_API.Models;
namespace Chef_Helper_Web.Services
{
    public interface IWarehouseService
    {
        Task<List<Warehouse>> GetWarehouses();
        Task<Warehouse> GetWarehouse(string ingridientName);
        Task<Warehouse> Update(int boxToUpdate, Warehouse updatedWarehouse);

        Task<Warehouse> Post(string ingredientName, int quantity);

        Task Delete(int boxNumber);
    }
}
