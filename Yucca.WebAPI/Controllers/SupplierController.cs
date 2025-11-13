using Microsoft.AspNetCore.Mvc;
using Yucca.Inventory;

namespace Yucca.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierList _supplierList;

        public SupplierController(ISupplierList supplierList)
        {
            _supplierList = supplierList;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Supplier>>> GetSuppliers([FromQuery] string name = "")
        {
            var suppliers = await _supplierList.FilterByName(name);
            return Ok(suppliers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Supplier>> GetSupplier(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Supplier ID is required.");
            }
            
            var supplier = await _supplierList.Get(id);
            
            if (supplier == null)
            {
                return NotFound();
            }

            return Ok(supplier);
        }

        [HttpPost]
        public async Task<ActionResult<Supplier>> PostSupplier([FromBody] Supplier supplier)
        {
            if (supplier == null)
            {
                return BadRequest("Supplier data is required.");
            }

            await _supplierList.Save(supplier);
            return CreatedAtAction(nameof(GetSuppliers), new { id = supplier.Id }, supplier);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSupplier(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest("Supplier ID is required.");
            }

            var existing = await _supplierList.Get(id);
            if (existing == null)
            {
                return NotFound();
            }

            await _supplierList.Remove(id);
            return NoContent();
        }
    }
}
