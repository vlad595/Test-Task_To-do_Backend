using System;
using System.Security.Claims;
using DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service;

namespace Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = Guid.Parse(userIdClaim);

            var result = await _categoryService.GetAllCategories(userId);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(CategoryCreationDto dto)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = Guid.Parse(userIdClaim);

            var result = await _categoryService.CreateCategoryAsync(dto, userId);
            if (result == null)
            {
                return BadRequest("Category with this name already exists");
            }
            return Ok(result);
        }
        [HttpDelete]
        public async Task<IActionResult> RemoveCategory(int categoryId)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = Guid.Parse(userIdClaim);

            var result = await _categoryService.DeleteCategoryAsync(categoryId, userId);
            if (result == 1 || result == 2)
            {
                return NotFound("Category does not found");
            }
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> RemoveCategory(int categoryId, string newName)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userId = Guid.Parse(userIdClaim);

            var result = await _categoryService.RenameCategoryAsync(categoryId, newName, userId);
            if (result == null)
            {
                return Forbid();
            }
            return Ok(result);
        }
    }
}