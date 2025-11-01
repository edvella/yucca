using Microsoft.AspNetCore.Mvc;
using Yucca.Volatile;
using Yucca.Inventory;

namespace Yucca.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private static readonly InMemorySupplierList supplierList = new();
        
        [HttpGet]
        public ActionResult<IEnumerable<Supplier>> GetSuppliers([FromQuery] string name = "")
        {
            return Ok(supplierList.FilterByName(name));
        }

        [HttpGet("{id}")]
        public ActionResult<Supplier> GetSupplier(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Supplier ID is required.");
            }
            
            var supplier = supplierList.Get(id);
            
            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(supplier);
        }

        [HttpPost]
        public ActionResult<Supplier> PostSupplier([FromBody] Supplier supplier)
        {
            if (supplier == null)
            {
                return BadRequest("Supplier data is required.");
            }

            supplierList.Save(supplier);
            return CreatedAtAction(nameof(GetSuppliers), new { id = supplier.Id }, supplier);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSupplier(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Supplier ID is required.");
            }

            var existing = supplierList.Get(id);
            if (existing == null)
            {
                return NotFound();
            }

            supplierList.Remove(id);
            return NoContent();
        }
    }
}
