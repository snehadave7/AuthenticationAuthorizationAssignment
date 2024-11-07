using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AuthenticationDemo.Repository;
using AuthenticationDemo.Models;
using AuthenticationDemo.Authentication;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
namespace AuthenticationDemo.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase {
        private readonly IProductService _service;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProductsController(IProductService service, ApplicationDbContext context, IMapper mapper) {
            _service = service;
            _context = context;
            _mapper = mapper;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult GetAllProduct() {
            List<Products> products = _service.GetAllProductss();
            return Ok(products);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult CreateProduct(Products product) {
            int result = _service.AddNewProducts(product);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        public IActionResult UpdateProduct(Products product) {
            string result = _service.UpdateProducts(product);
            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public IActionResult DeleteProduct(int id) {
            string result = _service.DeleteProducts(id);
            return Ok(result);
        }


        [Authorize(Roles = "User")]
        //[AllowAnonymous]
        [HttpGet("DTO")]
        public async Task<ActionResult<List<ProductDTO>>> GetProductByDTO() {
            List<Products> products = _context.Products.ToList();
            if (products != null) {
                var productDTO = _mapper.Map<List<ProductDTO>>(products);
                return productDTO;
            }
            return NotFound();
        }
        
    }
}
