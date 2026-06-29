using Microsoft.AspNetCore.Mvc;
using Shop.App.Filters;
using Shop.Domain.Models;

namespace ShopApp.Controllers
{
    // http://localhost:port/api/product
    [ApiController]
    [Route("api/[controller]")]
    [LogActionFilter]
    public class ProductsController : ControllerBase
    {
        private List<Product2> _products = new()
{
    new Product2
    {
        Id = 1,
        Name = "Laptop",
        Price = 25000
    },
    new Product2
    {
        Id = 2,
        Name = "Mouse",
        Price = 800
    },
    new Product2
    {
        Id = 3,
        Name = "Keyboard",
        Price = 1500
    }
};
        [HttpGet]
        public ActionResult<List<Product2>> GetProducts()
        {
            return Ok(_products);
        }

        // GET: api/products/1
        [HttpGet("{id:int}")]
        public ActionResult<Product2> GetByIdProducts(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
            {
                return NotFound("Product not found");
            }

            return Ok(product);
        }
        [HttpPost]
        public ActionResult<Product2> CreateProduct([FromBody] CreateUpdateProductDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = new Product2
            {
                Id = _products.Any() ? _products.Max(x => x.Id) + 1 : 1,
                Name = dto.Name,
                Price = dto.Price
            };

            _products.Add(product);

            return CreatedAtAction(
                nameof(GetByIdProducts),
                new { id = product.Id },
                product);
        }

        // PUT: api/products/1
        [HttpPut("{id:int}")]
        public ActionResult<Product2> UpdateProduct(int id, [FromBody] CreateProductDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            product.Name = dto.Name;
            product.Price = dto.Price;

            return Ok(product);
        }
        [HttpDelete("{id:int}")]
        public ActionResult DeleteProduct(int id)
        {
            var product = _products.FirstOrDefault(p => p.Id == id);

            if (product == null)
                return NotFound("Product not found");

            _products.Remove(product);

            return NoContent(); // 204
        }
        [HttpGet("search")]
        public ActionResult<List<Product2>> Search([FromQuery] string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return BadRequest("Query parameter 'name' is required");

            var result = _products
                .Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(result);
        }
    }
}
