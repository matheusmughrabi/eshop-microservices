using eShop.ProductApi.Domain;
using eShop.ProductApi.Exceptions;

namespace eShop.ProductApi.Entity;

public class CategoryEntity : BaseEntity
{
    protected CategoryEntity()
    {
    }

    public CategoryEntity(string name, string? description = null)
    {
        ValidateIfNullOrEmptyName(name);
        ValidateIfNameIsTooLong(name);

        Name = name;
        Description = description;
    }

    public string Name { get; private set; }
    public string? Description { get; private set; }
    public List<ProductEntity> Products { get; set; }
    public Guid? CategoryGroupId { get; private set; }
    public CategoryGroupEntity? CategoryGroup { get; }

    public void Update(string name, string? description = null)
    {
        ValidateIfNullOrEmptyName(name);
        ValidateIfNameIsTooLong(name);

        Name = name;
        Description = description;
    }

    public void RemoveProducts()
    {
        Products.Clear();
    }

    private void ValidateIfNullOrEmptyName(string name)
    {
        if (string.IsNullOrEmpty(name))
            throw new InvalidPropertyValueException("'Name' cannot be null or empty.");
    }

    private void ValidateIfNameIsTooLong(string name)
    {
        if (name.Length > 100)
            throw new InvalidPropertyValueException("'Name' length is limited to 100 characters.");
    }
}
