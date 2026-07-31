using System;
using Data;
using DTO;
using Microsoft.EntityFrameworkCore;
using Models;

namespace Service
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreationDto dto, Guid userId);
        Task<int> DeleteCategoryAsync(int categoryId, Guid userId);
        Task<CategoryResponseDto> RenameCategoryAsync(int categoryId, string newName, Guid userId);
        Task<List<CategoryItemResponseDto>> GetAllCategories(Guid userId);
    }
    public class CategoryService : ICategoryService
    {
        private readonly AppDBContext _context;
        public CategoryService(AppDBContext context)
        {
            _context = context;
        }
        public async Task<CategoryResponseDto> CreateCategoryAsync(CategoryCreationDto dto, Guid userId)
        {
            if (await _context.Categories.FirstOrDefaultAsync(c => c.Name == dto.name && c.UserId == userId) != null)
            {
                return null;
            }
            
            var newCategory = new Category
            {
                Name = dto.name,
                UserId = userId
            };

            _context.Categories.Add(newCategory);
            await _context.SaveChangesAsync();
            return new CategoryResponseDto
            {
                id = newCategory.Id,
                name = newCategory.Name  
            };
        }
        public async Task<int> DeleteCategoryAsync(int categoryId, Guid userId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category == null)
            {
                return 1;
            }
            else if (category.UserId == userId)
            {
                _context.Categories.Remove(category);
                await _context.SaveChangesAsync();
                return 0;
            }
            return 2;
        }
        public async Task<CategoryResponseDto> RenameCategoryAsync(int categoryId, string newName, Guid userId)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            var nameExists = await _context.Categories.FirstOrDefaultAsync(c => c.Name == newName && c.UserId == userId);
            if (category == null || nameExists != null)
            {
                return null;
            }
            if (category.UserId == userId)
            {
                category.Name = newName;
                await _context.SaveChangesAsync();
                return new CategoryResponseDto
                {
                    id = category.Id,
                    name = category.Name
                };
            }
            return null;
        }
        public async Task<List<CategoryItemResponseDto>> GetAllCategories(Guid userId)
        {
            var categoryItems = await _context.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Id)
                .Select(category => new CategoryItemResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    
                    Tasks = category.Tasks.ToList() 
                })
                .ToListAsync();

            return categoryItems;
        }
    }
}