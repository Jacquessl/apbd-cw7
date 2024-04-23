using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.DTO_s;

public class AddProduct
{
    public int IdProduct { get; set; }
    public int IdWarehouse { get; set; }
    public int Amount { get; set; }
    //[MaxLength(200)]
    public DateTime CreatedAt { get; set; }
}