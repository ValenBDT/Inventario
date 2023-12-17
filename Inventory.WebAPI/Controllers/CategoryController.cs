using AutoMapper;
using Inventory.DTOs.Category;
using Inventory.Entities;
using Inventory.Persistence.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _mapper = mapper;
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var categories = await _categoryRepository.GetAllAsync();
            var categoriesList = _mapper.Map<List<CategoryToListDTO>>(categories);
            // var categoriesList = categories.Select(c => new CategoryToListDTO{
            //     Id = c.Id,
            //     Name = c.Name,
            //     Description = c.Description,
            //     CreatedAt = c.CreatedAt,
            //     UpdatedAt = c.UpdatedAt
            // }).ToList();

            return Ok(categoriesList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id){
            var category = await _categoryRepository.GetByIdAsync(id);

            if(category is null) return NotFound("No se encontro tal categoria");

            var categoryListDTO = _mapper.Map<CategoryToListDTO>(category);


            return Ok(categoryListDTO);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryToCreateDTO categoryToCreateDTO){
            //  var categoryToCreate = new Category
            //  {
            //      Name = categoryToCreateDTO.Name,
            //      Description = categoryToCreateDTO.Description,
            //      CreatedAt = DateTime.Now
            //  };
            var categoryToCreate = _mapper.Map<Category>(categoryToCreateDTO);
            categoryToCreate.CreatedAt = DateTime.Now;
            var categoryCreated = await _categoryRepository.AddAsync(categoryToCreate);

            //  var categoryCreatedDto = new CategoryToListDTO
            //  {
            //      Id=categoryCreated.Id,
            //      Name = categoryCreated.Name,
            //      Description= categoryCreated.Description,
            //      CreatedAt = categoryCreated.CreatedAt,
            //      UpdatedAt = categoryCreated.UpdatedAt
            //  };
            var categoryCreatedDto = _mapper.Map<CategoryToListDTO>(categoryCreated);
            return Ok(categoryCreatedDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, CategoryToEditDTO categoryToEditDTO){
            if(id != categoryToEditDTO.Id) return BadRequest("Error en los datos de entrada");

            var categoryToUpdate = await _categoryRepository.GetByIdAsync(id);

            if(categoryToUpdate == null) return NotFound("Id no encontrado");

            categoryToUpdate.Name = categoryToEditDTO.Name;
            categoryToUpdate.Description = categoryToEditDTO.Description;

            categoryToUpdate.UpdatedAt = DateTime.Now;

            var updated = await _categoryRepository.UpdateAsync(id, categoryToUpdate);

            if(!updated) return NoContent();

            var updatedCategory = await _categoryRepository.GetByIdAsync(id);

            var updatedCategoryDTO = _mapper.Map<CategoryToListDTO>(updatedCategory);

            return Ok(updatedCategoryDTO);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteById(int id){
            var categoryToDelete = await _categoryRepository.GetByIdAsync(id);
            if(categoryToDelete is null) NotFound("Registro no encontrado");
            var deleted = await _categoryRepository.DeleteAsync(id);
            if(!deleted) return Ok("Registrno no eliminado, vea los logs");
            return Ok("Registro eliminado");
        }
    }
}