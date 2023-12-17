using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.DTOs.InventoryMovement;
using Inventory.Entities;
using Inventory.Persistence.Interfaces;
using Inventory.Persistence.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InventoryMovementController : ControllerBase
    {
        private readonly IInventoryMovementRepository _inventoryMovementRepository;
        private readonly IMovementTypeRepository _movementTypeRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        public InventoryMovementController(IInventoryMovementRepository inventoryMovementRepository, IMovementTypeRepository movementTypeRepository, IProductRepository productRepository, IMapper mapper)
        {
            _mapper = mapper;
            _movementTypeRepository = movementTypeRepository;
            _productRepository = productRepository;
            _inventoryMovementRepository = inventoryMovementRepository;    
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(){
        var inventoryMovements = await _inventoryMovementRepository.GetAllAsync();
        foreach (var inventoryMovement in inventoryMovements)
        {
            inventoryMovement.Product = await _productRepository.GetByIdAsync(inventoryMovement.ProductId);
            inventoryMovement.MovementType = await _movementTypeRepository.GetByIdAsync(inventoryMovement.MovementTypeId);
        }
        return Ok(_mapper.Map<List<InventoryMovementToListDTO>>(inventoryMovements));
        
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id){
            var inventoryMovement = await _inventoryMovementRepository.GetByIdAsync(id);

            if(inventoryMovement is null) return NotFound("No se encontro tal movimiento de inventario");
            inventoryMovement.Product = await _productRepository.GetByIdAsync(inventoryMovement.ProductId);
            inventoryMovement.MovementType = await _movementTypeRepository.GetByIdAsync(inventoryMovement.MovementTypeId);
            var InventoryMovementListDTO = _mapper.Map<InventoryMovementToListDTO>(inventoryMovement);

            
            // var InventoryMovementList = new InventoryMovementToListDTO{
            //     Id = inventoryMovement.Id,
            //     ProductId =  inventoryMovement.ProductId,
            //     Date = inventoryMovement.Date,
            //     Quantity = inventoryMovement.Quantity,
            //     MovementTypeId = inventoryMovement.MovementTypeId,
            //     MovementTypeName = MovementType.Name,
            //     ProductName = Product.Name,
            // };


            return Ok(InventoryMovementListDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Post(InventoryMovementToCreateDTO inventoryMovementToCreateDTO){


            var MovementType = await _movementTypeRepository.GetByIdAsync(inventoryMovementToCreateDTO.MovementTypeId);
            if(MovementType is null) return BadRequest("MovementType no existe");
            var Product = await _productRepository.GetByIdAsync(inventoryMovementToCreateDTO.ProductId);
            if(Product is null) return BadRequest("Producto no existe"); 

            var inventoryMovementToCreate = _mapper.Map<InventoryMovement>(inventoryMovementToCreateDTO);

            
            var inventoryMovementCreated = await _inventoryMovementRepository.AddAsync(inventoryMovementToCreate);
            inventoryMovementCreated.MovementType = MovementType;
            inventoryMovementCreated.Product = Product;

            var inventoryMovementListDTO = _mapper.Map<InventoryMovementToListDTO>(inventoryMovementCreated);

            return Ok(inventoryMovementListDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, InventoryMovementToEditDTO inventoryMovementToEditDTO){
            if(id != inventoryMovementToEditDTO.Id) return BadRequest("Error en los datos de entrada");

            var inventoryMovementToUpdate= await _inventoryMovementRepository.GetByIdAsync(id);

            if(inventoryMovementToUpdate == null) return NotFound("Id no encontrado");

            inventoryMovementToUpdate.ProductId = inventoryMovementToEditDTO.ProductId;
            inventoryMovementToUpdate.Date = inventoryMovementToEditDTO.Date;
            inventoryMovementToUpdate.MovementTypeId = inventoryMovementToEditDTO.MovementTypeId;
            inventoryMovementToUpdate.Quantity = inventoryMovementToEditDTO.Quantity;

            var MovementType = await _movementTypeRepository.GetByIdAsync(inventoryMovementToUpdate.MovementTypeId);
            if(MovementType is null) return BadRequest("MovementType no existe");

            var Product = await _productRepository.GetByIdAsync(inventoryMovementToUpdate.ProductId);
            if(Product is null) return BadRequest("Producto no existe"); 
            var updated = await _inventoryMovementRepository.UpdateAsync(id, inventoryMovementToUpdate);

            if(!updated) return NoContent();

            var updatedInventoryMovement= await _inventoryMovementRepository.GetByIdAsync(id);

            updatedInventoryMovement.MovementType = MovementType;
            updatedInventoryMovement.Product = Product;
            var inventoryMovementListDTO = _mapper.Map<InventoryMovementToListDTO>(updatedInventoryMovement);

            // var inventoryMovementListDTO=  new InventoryMovementToListDTO{
            //     Id = updatedInventoryMovement.Id,
            //     ProductId =  updatedInventoryMovement.ProductId,
            //     Date = updatedInventoryMovement.Date,
            //     Quantity = updatedInventoryMovement.Quantity,
            //     MovementTypeId = updatedInventoryMovement.MovementTypeId,
            //     MovementTypeName = updatedInventoryMovement.MovementType.Name,
            //     ProductName = updatedInventoryMovement.Product.Name,
            // };  

            return Ok(inventoryMovementListDTO);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id){
            var inventoryMovementToDelete = await _inventoryMovementRepository.GetByIdAsync(id);
            if(inventoryMovementToDelete is null) NotFound("Registro no encontrado");
            var deleted = await _inventoryMovementRepository.DeleteAsync(id);
            if(!deleted) return Ok("Registrno no eliminado, vea los logs");
            return Ok("Registro eliminado");
        } 
    }
}