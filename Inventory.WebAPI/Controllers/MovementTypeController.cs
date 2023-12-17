using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Inventory.DTOs.MovementType;
using Inventory.Entities;
using Inventory.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovementTypeController : ControllerBase
    {
        private readonly IMovementTypeRepository _movementTypeRepository;
        private readonly IMapper _mapper;
        public MovementTypeController(IMovementTypeRepository movementTypeRepository, IMapper mapper)
        {
            _movementTypeRepository = movementTypeRepository;
 
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var movementTypes = await _movementTypeRepository.GetAllAsync();
            return Ok(_mapper.Map<List<MovementTypeToListDTO>>(movementTypes));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var moventType = await _movementTypeRepository.GetByIdAsync(id);
            return Ok(_mapper.Map<MovementTypeToListDTO>(moventType));
        }

        [HttpPost]
        public async Task<IActionResult> Post(MovementTypeToCreateDTO movementTypeToCreateDto)
        {
            var movementeToCreate = _mapper.Map<MovementType>(movementTypeToCreateDto);
            movementeToCreate.CreatedAt = DateTime.Now;
            var movementCreated = await _movementTypeRepository.AddAsync(movementeToCreate);

            return Ok(_mapper.Map<MovementTypeToListDTO>(movementCreated));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, MovementTypeToEditDTO movementTypeToEditDto)
        {
            if(id != movementTypeToEditDto.Id)
                 return BadRequest("Error en los datos de entrada");

            var movementToUpdate = await _movementTypeRepository.GetByIdAsync(id);
            if(movementToUpdate is null)
                return BadRequest("Id no encontrado");
            
            _mapper.Map(movementTypeToEditDto,movementToUpdate);

            movementToUpdate.UpdatedAt = DateTime.Now;

            var updated = await _movementTypeRepository.UpdateAsync(id, movementToUpdate);

            if(!updated)
                return NoContent();
            
            var movementType = await _movementTypeRepository.GetByIdAsync(id);

            return Ok(_mapper.Map<MovementTypeToListDTO>(movementType));
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var movementTypeToDelete = await _movementTypeRepository.GetByIdAsync(id);
            if(movementTypeToDelete is null)
              return NotFound("IdNo ecnonctrado");

            var deleted = await _movementTypeRepository.DeleteAsync(movementTypeToDelete);
            if(!deleted)
                return Ok("Registro no borrado consulte al administrador");

            return Ok("Registro borrado");
        }


    }
}