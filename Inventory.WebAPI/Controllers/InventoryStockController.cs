
using AutoMapper;
using Inventory.DTOs.InventoryStock;
using Inventory.Entities;
using Inventory.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryStockController : ControllerBase
    {   
        private readonly IProductRepository _productRepository;
        private readonly IInventoryStockRepository _inventoryStockRepository;
        private readonly IMapper _mapper;
        public InventoryStockController(IInventoryStockRepository inventoryStockRepository, IMapper mapper, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _mapper = mapper;
            _inventoryStockRepository = inventoryStockRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var inventoryStocks = await _inventoryStockRepository.GetAllAsync();
            foreach (var inventoryStock in inventoryStocks)
            {
                inventoryStock.Product = await _productRepository.GetByIdAsync(inventoryStock.ProductId);
            }
            return Ok(_mapper.Map<List<InventoryStockToListDTO>>(inventoryStocks));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var inventoryStock = await _inventoryStockRepository.GetByIdAsync(id);
            inventoryStock.Product = await _productRepository.GetByIdAsync(inventoryStock.ProductId);
            return Ok(_mapper.Map<InventoryStockToListDTO>(inventoryStock));
        }

        [HttpPost]
        public async Task<IActionResult> Post(InventoryStockToCreateDTO inventoryStockToCreateDto)
        {
            var Product = await _productRepository.GetByIdAsync(inventoryStockToCreateDto.ProductId);
            if(Product is null) return BadRequest("Producto no existe"); 
            var inventoryStockToCreate = _mapper.Map<InventoryStock>(inventoryStockToCreateDto);
            inventoryStockToCreate.CreatedAt = DateTime.Now;

            var inventoryCreated = await _inventoryStockRepository.AddAsync(inventoryStockToCreate);
            inventoryCreated.Product = Product;
            var inventoryStockListDTO = _mapper.Map<InventoryStockToListDTO>(inventoryCreated);
            return Ok(inventoryStockListDTO);

        }

        

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, InventoryStockToEditDTO inventoryStockToEditDto)
        {
            var Product = await _productRepository.GetByIdAsync(inventoryStockToEditDto.ProductId);

            if(Product is null) return BadRequest("Producto no existe");

            if(id != inventoryStockToEditDto.Id)
                return BadRequest("Error en los datos de entrada");

            var inventoryStockToUpdate = await _inventoryStockRepository.GetByIdAsync(id);
            if(inventoryStockToUpdate is null)
                return BadRequest("Id no encontrado");
            
            _mapper.Map(inventoryStockToEditDto,inventoryStockToUpdate);
            inventoryStockToUpdate.UpdatedAt = DateTime.Now;
            var updated = await _inventoryStockRepository.UpdateAsync(id,inventoryStockToUpdate);

            if(!updated)
                return NoContent();
            
            var inventoryStock = await _inventoryStockRepository.GetByIdAsync(id);
            inventoryStock.Product = Product;
            return Ok(_mapper.Map<InventoryStockToListDTO>(inventoryStock));            

        }
    }
}