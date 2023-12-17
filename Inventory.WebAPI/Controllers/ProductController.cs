using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.DTOs.Product;
using Inventory.Entities;
using Inventory.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ISupplierRepository _supplierRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IMapper mapper, ICategoryRepository categoryRepository, ISupplierRepository supplierRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;
            _supplierRepository = supplierRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepository.GetAllAsync();

            foreach (var product in products)
            {
                product.Category = await _categoryRepository.GetByIdAsync(product.CategoryId);  
                product.Supplier = await _supplierRepository.GetByIdAsync(product.SupplierId);  
            }

            return Ok(_mapper.Map<List<ProductToListDTO>>(products));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GEtById(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            product.Category = await _categoryRepository.GetByIdAsync(product.CategoryId);  
            product.Supplier = await _supplierRepository.GetByIdAsync(product.SupplierId);  

            return Ok(_mapper.Map<ProductToListDTO>(product));
        }


        [HttpPost]
        public async Task<IActionResult> Post(ProductToCreateDTO productToCreateDto)
        {
            var category = await _categoryRepository.GetByIdAsync(productToCreateDto.CategoryId);
            if(category is null) return BadRequest("Categoria no existe");
            var supplier = await _supplierRepository.GetByIdAsync(productToCreateDto.SupplierId);
            if(supplier is null) return BadRequest("Supplier no existe");

            var productToCreate = _mapper.Map<Product>(productToCreateDto);
            productToCreate.CreatedAt= DateTime.Now;
            var productCreated = await _productRepository.AddAsync(productToCreate);
            productCreated.Category = category;
            productCreated.Supplier = supplier;
            return Ok( _mapper.Map<ProductToListDTO>(productCreated));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, ProductToEditDTO productToEditDto)
        {
            if( id != productToEditDto.Id)
                return BadRequest();

            var productToUpdate = await _productRepository.GetByIdAsync(id);
            if(productToUpdate is null)
                return BadRequest("Id no encontrado");

            var category = await _categoryRepository.GetByIdAsync(productToEditDto.CategoryId);
            if(category is null) return BadRequest("Categoria no existe");
            var supplier = await _supplierRepository.GetByIdAsync(productToEditDto.SupplierId);
            if(supplier is null) return BadRequest("Supplier no existe");

            _mapper.Map(productToEditDto,productToUpdate);
            
            productToUpdate.UpdatedAt = DateTime.Now;
            var updated = await _productRepository.UpdateAsync(id,productToUpdate);
            if(!updated)
                return NoContent();

            var product = await _productRepository.GetByIdAsync(id);
            return Ok(_mapper.Map<ProductToListDTO>(product));

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete (int id)
        {
            var prodcutToDelete = await _productRepository.GetByIdAsync(id);

            if(prodcutToDelete is null)
            return NotFound("Producto no encontrado");

            var deleted = await _productRepository.DeleteAsync(prodcutToDelete);

            if(!deleted)
                return Ok("Producto no borrado contacte al administrador");
            
            return Ok("El producto fue borrado");
        }
    }
}