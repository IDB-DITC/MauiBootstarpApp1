using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace MauiBoostarpApp1.Models
{
    //[Table(nameof(ProductCategory))]
    [Table("ProductCategory")]
	public class ProductCategory: BaseModel
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
  
        public int ProductCategoryID { get; set; }

        [Required, NotNull]
        public string Name { get; set; }

        public ICollection<Product> Products { get; set; } = [];


	}
}
