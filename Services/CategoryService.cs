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
        Task<CategoryItemResponseDto> GetCategoryWithTasks(Guid userId, string categoryName);
        Task<List<CategoryResponseDto>> GetAllCategoryNames(Guid userId);
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
                Color = dto.color,
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
                    color = category.Color,
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
                    Color = category.Color,
                    Tasks = category.Tasks.ToList() 
                })
                .ToListAsync();

            return categoryItems;
        }
        public async Task<CategoryItemResponseDto>? GetCategoryWithTasks(Guid userId, string categoryName)
        {
            var category = await _context.Categories.Where(c => c.UserId == userId && c.Name == categoryName)
                .Select(category => new CategoryItemResponseDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    Color = category.Color,
                    Tasks = category.Tasks.ToList()
                }).FirstOrDefaultAsync();
            if (category != null)
            {
                return category;
            }
            return null;
            
        }
        public async Task<List<CategoryResponseDto>> GetAllCategoryNames(Guid userId)
        {
            var categoryItems = await _context.Categories
                .Where(c => c.UserId == userId)
                .OrderBy(c => c.Id)
                .Select(category => new CategoryResponseDto
                {
                    id = category.Id,
                    name = category.Name,
                    color = category.Color
                })
                .ToListAsync();

            return categoryItems;
        }
    }
}