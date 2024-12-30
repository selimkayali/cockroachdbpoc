using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CockroachDb.Models;

public class Product
{
    [Key]
    public long Id { get; set; }  // Changed from int to long

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }
}
