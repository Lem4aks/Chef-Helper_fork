using System;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Chef_Helper_API.Models;
using Microsoft.Data.SqlClient;

namespace Chef_Helper_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : Controller
    {
        public readonly ChefdbContext _dbContext;

        public WarehouseController(ChefdbContext dbContext)
        {
            _dbContext = dbContext;
        }
        // GET: api/Warehouse
        [HttpGet("/Warehouse")]
        public IActionResult Get()
        {
            var warehouses = _dbContext.Warehouse.ToList();
            return Ok(warehouses);
        }

        // GET: api/Warehouse/5
        [HttpGet("{ingredientName}")]
        public IActionResult Get(string ingredientName)
        {
            var warehouse = _dbContext.Warehouse.Where((box)=> box.IngredientName == ingredientName);
            if (warehouse == null)
            {
                return NotFound();
            }
            return Ok(warehouse);
        }

        // POST: api/Warehouse
        [HttpPost("{ingredientName}, {quantity}")]
        public IActionResult Post(int quantity, string ingredientName)
        {
            try
            {
                Warehouse newWarehouse = new Warehouse
                {
                    IngredientName = ingredientName,
                    WarehouseQuantity = quantity,
                };
                _dbContext.Warehouse.Add(newWarehouse);
                _dbContext.SaveChanges();
                return Ok(newWarehouse);
            }
            catch (Exception ex)
            {
                {
                    // Обработка ошибок при добавлении записи
                    return Content(ex.Message);
                }
            }
        }

        // PUT: api/Warehouse/5
        [HttpPut("{boxNumber}, {ingredientName}, {quantity}")]
        public IActionResult Put(int boxNumber, Warehouse updatedWarehouse, string ingredientName, int quantity)
        {
            var warehouseToUpdate = _dbContext.Warehouse.Find(boxNumber);
            if (warehouseToUpdate == null)
            {
                return NotFound();
            }
            updatedWarehouse.IngredientName = ingredientName;
            updatedWarehouse.WarehouseQuantity = quantity;
            warehouseToUpdate.IngredientName = updatedWarehouse.IngredientName;
            warehouseToUpdate.WarehouseQuantity = updatedWarehouse.WarehouseQuantity;

            _dbContext.SaveChanges();
            return Ok();
        }

        // DELETE: api/Warehouse/5
        [HttpDelete("{boxNumber}")]
        public IActionResult Delete(int boxNumber)
        {
            var warehouseToDelete = _dbContext.Warehouse.Find(boxNumber);
            if (warehouseToDelete == null)
            {
                return NotFound();
            }

            _dbContext.Warehouse.Remove(warehouseToDelete);
            _dbContext.SaveChanges();
            return Ok();
        }

    }
}
