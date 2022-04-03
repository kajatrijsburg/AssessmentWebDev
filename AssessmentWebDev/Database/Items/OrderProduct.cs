using System.ComponentModel.DataAnnotations;

namespace AssessmentWebDev.Database.Items;

public class OrderProduct
{
    [Required]
    public int ProductID { get; set; }
    [Required]
    public int OrderID { get; set; }
    
    [Required]
    public int Count { get; set; }
    
    
    [Required]
    public int Paid { get; set; }
    
    public Product Product { get; set; }
    public Order Order { get; set; }
}