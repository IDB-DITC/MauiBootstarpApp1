using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MauiBoostarpApp1.Models
{
    [Table(nameof(Product))]
    public class Product: BaseModel
	{
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string ProductNumber { get; set; }

        public string Color { get; set; }

        [Required]
        public decimal StandardCost { get; set; }

        [Required]
        public decimal ListPrice { get; set;}

        [Required]
        public int Size { get; set;}

        [Required]
        public decimal Weight { get; set;}

        [ForeignKey(nameof(ProductCategory.ProductCategoryID))]
        public int ProductCategoryID { get; set; }

        public ProductCategory? ProductCategory { get; set; }

		
	}



    public abstract class BaseModel
    {
		public string CreatedBy { get; set; }
		public DateTime CreatedOn { get; set; } = DateTime.Now;

		public string ModifiedBy { get; set; }
		public DateTime? ModifiedOn { get; set; }
	}
}
