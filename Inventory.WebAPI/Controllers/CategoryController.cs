using Inventory.DTOs.Category;
using Inventory.DTOs.Categoty;
using Inventory.Entities;
using Inventory.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;    
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(){
            var categories = await _categoryRepository.GetAllAsync();

            var categoriesList = categories.Select(c => new CategoryToListDTO{
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt
            }).ToList();

            return Ok(categoriesList);
        }

        [HttpPost]
        public async Task<IActionResult> Post(CategoryToCreateDTO categoryToCreateDTO){
            var categoryToCreate = new Category{
                Name = categoryToCreateDTO.Name,
                Description = categoryToCreateDTO.Description,
                CreatedAt = DateTime.Now,
            };
            var categoryCreated = await _categoryRepository.AddAsync(categoryToCreate);

            var categoryCreatedDTO =  new Category{
                Id = categoryCreated.Id,
                Name = categoryCreated.Name,
                Description = categoryCreated.Description,
                CreatedAt = DateTime.Now,
                UpdatedAt = categoryCreated.UpdatedAt,
            };
            return Ok(categoryCreatedDTO);
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

            var updatedCategoryDTO = new CategoryToListDTO{
                Id = updatedCategory.Id,
                Name = updatedCategory.Name,
                Description = updatedCategory.Description,
                CreatedAt = updatedCategory.CreatedAt,
                UpdatedAt = updatedCategory.UpdatedAt,
            };

            return Ok(updatedCategoryDTO);

            

        }
        
    }
}