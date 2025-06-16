using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace IzzyShop.Models
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        // Vulnerable: No proper validation on price
        public decimal Price { get; set; }

        // Vulnerable: No proper validation on description (XSS vulnerability)
        public string Description { get; set; }

        // Vulnerable: No proper validation on image path (Path Traversal vulnerability)
        public string ImagePath { get; set; }

        // Vulnerable: No proper validation on SKU
        public string SKU { get; set; }

        // Vulnerable: No proper validation on stock
        public int Stock { get; set; }

        // Vulnerable: No proper validation on category
        public string Category { get; set; }

        // Vulnerable: No proper validation on metadata (XXE vulnerability)
        public string Metadata { get; set; }

        // Vulnerable: No proper validation on supplier info
        public string SupplierInfo { get; set; }

        // Vulnerable: No proper validation on discount
        public decimal Discount { get; set; }

        public List<Review> Reviews { get; set; }
    }
} 