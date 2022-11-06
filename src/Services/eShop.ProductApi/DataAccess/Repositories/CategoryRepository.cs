using eShop.ProductApi.DataAccess.Repositories.QueryModels;
using eShop.ProductApi.Entity;
using Microsoft.EntityFrameworkCore;

namespace eShop.ProductApi.DataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ProductDbContext _dbContext;

        public CategoryRepository(ProductDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CategoryEntity> CreateCategory(CategoryEntity entity)
        {
            return _dbContext.Category.Add(entity).Entity;
        }

        public async Task<bool> CategoryExists(string name)
        {
            return await _dbContext.Category
                .AsNoTracking()
                .AnyAsync(c => c.Name == name);
        }

        public async Task<GetCategoriesQueryResponse> GetCategories(int page = 1, int itemsPerPage = 10)
        {
            var skip = (page - 1) * itemsPerPage;

            var categories = await _dbContext.Category
                .AsNoTracking()
                .Skip(skip)
                .Take(itemsPerPage)
                .Select(c => new GetCategoriesQueryResponse.Category()
                {
                    Name = c.Name,
                    Description = c.Description,
                    TotalProducts = c.Products.Count()
                })
                .ToListAsync();

            return new GetCategoriesQueryResponse() { Categories = categories };
        }

        /// <summary>
        /// This method should be used when tracking the entity state is needed
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CategoryEntity> GetCategoryById(Guid id, bool includeProducts = false)
        {
            return includeProducts
                ? await _dbContext.Category.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id)
                : await _dbContext.Category.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task SaveChanges()
        {
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteCategory(CategoryEntity entity)
        {
            _dbContext.Category.Remove(entity);
        }
    }
}
