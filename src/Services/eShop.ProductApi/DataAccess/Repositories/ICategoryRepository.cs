using eShop.ProductApi.DataAccess.Repositories.QueryModels;
using eShop.ProductApi.Entity;

namespace eShop.ProductApi.DataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<CategoryEntity> GetCategoryById(Guid id, bool includeProcuts = false);
        Task<GetCategoriesQueryResponse> GetCategories(int page, int itemsPerPage);
        Task<CategoryEntity> CreateCategory(CategoryEntity entity);
        Task<bool> CategoryExists(string name);
        Task DeleteCategory(CategoryEntity entity);
        Task SaveChanges();
    }
}
