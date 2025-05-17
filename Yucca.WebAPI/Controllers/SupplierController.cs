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
        public ActionResult<IEnumerable<Supplier>> GetSuppliers()
        {
            return Ok(supplierList.FilterByName(string.Empty));
        }

        [HttpPost]
        public ActionResult<Supplier> AddSupplier([FromBody] Supplier supplier)
        {
            if (supplier == null)
            {
                return BadRequest("Supplier data is required.");
            }

            supplierList.Save(supplier);
            return CreatedAtAction(nameof(GetSuppliers), new { id = supplier.Id }, supplier);
        }
    }
}
