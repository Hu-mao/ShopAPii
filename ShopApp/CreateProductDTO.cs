using System.ComponentModel.DataAnnotations;

public class CreateProductDTO
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; } = string.Empty;

    public decimal Price { get; set; }
}