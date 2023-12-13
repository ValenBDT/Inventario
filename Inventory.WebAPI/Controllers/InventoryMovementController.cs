using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public InventoryMovementController(IInventoryMovementRepository inventoryMovementRepository, IMovementTypeRepository movementTypeRepository, IProductRepository productRepository)
        {
            _movementTypeRepository = movementTypeRepository;
            _productRepository = productRepository;
            _inventoryMovementRepository = inventoryMovementRepository;    
        }
        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var inventoryMovements = await _inventoryMovementRepository.GetAllAsync();

            // Recorre la lista
            var inventoryMovementListTasks = inventoryMovements.Select(async im =>{
            
                var movementType = await _movementTypeRepository.GetByIdAsync(im.MovementTypeId);
                var product = await _productRepository.GetByIdAsync(im.ProductId);


                return new InventoryMovementToListDTO{
                    Id = im.Id,
                    ProductId = im.ProductId,
                    Date = im.Date,
                    Quantity = im.Quantity,
                    MovementTypeId = im.MovementTypeId,
                    MovementTypeName = movementType.Name ?? "Desconocido",
                    ProductName = product.Name ?? "Desconocido",
                };
                });

                var inventoryMovementList = await Task.WhenAll(inventoryMovementListTasks);

                return Ok(inventoryMovementList.ToList());
            }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id){
            var inventoryMovement = await _inventoryMovementRepository.GetByIdAsync(id);

            if(inventoryMovement is null) return NotFound("No se encontro tal movimiento de inventario");

            var MovementType = await _movementTypeRepository.GetByIdAsync(inventoryMovement.MovementTypeId);
            if(MovementType is null) return BadRequest("MovementType no existe");
            var Product = await _productRepository.GetByIdAsync(inventoryMovement.ProductId);
            if(Product is null) return BadRequest("Producto no existe"); 
            
            var InventoryMovementList = new InventoryMovementToListDTO{
                Id = inventoryMovement.Id,
                ProductId =  inventoryMovement.ProductId,
                Date = inventoryMovement.Date,
                Quantity = inventoryMovement.Quantity,
                MovementTypeId = inventoryMovement.MovementTypeId,
                MovementTypeName = MovementType.Name,
                ProductName = Product.Name,
            };


            return Ok(InventoryMovementList);
        }

        [HttpPost]
        public async Task<IActionResult> Post(InventoryMovementToCreateDTO inventoryMovementToCreateDTO){


            var MovementType = await _movementTypeRepository.GetByIdAsync(inventoryMovementToCreateDTO.MovementTypeId);
            if(MovementType is null) return BadRequest("MovementType no existe");
            var Product = await _productRepository.GetByIdAsync(inventoryMovementToCreateDTO.ProductId);
            if(Product is null) return BadRequest("Producto no existe"); 


            var inventoryMovementToCreate = new InventoryMovement{
                ProductId =  inventoryMovementToCreateDTO.ProductId,
                Date = inventoryMovementToCreateDTO.Date,
                Quantity = inventoryMovementToCreateDTO.Quantity,
                MovementTypeId = inventoryMovementToCreateDTO.MovementTypeId,
                MovementType = MovementType,
                Product= Product,
            };
            
            

            var inventoryMovementCreated = await _inventoryMovementRepository.AddAsync(inventoryMovementToCreate);


            var inventoryMovementListDTO=  new InventoryMovementToListDTO{
                Id = inventoryMovementCreated.Id,
                ProductId =  inventoryMovementCreated.ProductId,
                Date = inventoryMovementCreated.Date,
                Quantity = inventoryMovementCreated.Quantity,
                MovementTypeId = inventoryMovementCreated.MovementTypeId,
                MovementTypeName = inventoryMovementCreated.MovementType?.Name ?? "Desconocido",
                ProductName = inventoryMovementCreated.Product?.Name ?? "Desconocido",
            };  

            
            return Ok(inventoryMovementListDTO);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, InventoryMovementToEditDTO inventoryMovementToEditDTO){
            if(id != inventoryMovementToEditDTO.Id) return BadRequest("Error en los datos de entrada");

            var inventoryMovementToUpdate= await _inventoryMovementRepository.GetByIdAsync(id);

            if(inventoryMovementToUpdate == null) return NotFound("Id no encontrado");

            inventoryMovementToUpdate.ProductId = inventoryMovementToEditDTO.ProductId;
            inventoryMovementToUpdate.Date = inventoryMovementToEditDTO.Date;

            inventoryMovementToUpdate.Quantity = inventoryMovementToEditDTO.Quantity;
            inventoryMovementToUpdate.MovementTypeId = inventoryMovementToEditDTO.MovementTypeId;


            var updated = await _inventoryMovementRepository.UpdateAsync(id, inventoryMovementToUpdate);

            if(!updated) return NoContent();

            var updatedInventoryMovement= await _inventoryMovementRepository.GetByIdAsync(id);

            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            var inventoryMovementListDTO=  new InventoryMovementToListDTO{
                Id = updatedInventoryMovement.Id,
                ProductId =  updatedInventoryMovement.ProductId,
                Date = updatedInventoryMovement.Date,
                Quantity = updatedInventoryMovement.Quantity,
                MovementTypeId = updatedInventoryMovement.MovementTypeId,
                MovementTypeName = updatedInventoryMovement.MovementType.Name,
                ProductName = updatedInventoryMovement.Product.Name,
            };  
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
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