using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eShop.ProductApi.Entity
{
    public abstract class BaseEntity : IEquatable<BaseEntity>
    {
        protected BaseEntity()
        {
            if (Id == Guid.Empty)
            {
                Id = Guid.NewGuid();
                CreateDate = DateTime.UtcNow;
            }
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; private set; }
        public DateTime CreateDate { get; private set; }

        public bool Equals(BaseEntity? other)
        {
            return Id == other?.Id;    
        }
    }
}
