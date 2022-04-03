using System.ComponentModel.DataAnnotations;

namespace AssessmentWebDev.Database.Items;

public class Product
{
    
    public int ProductID { get; set; }
    [Required]
    [DataType(DataType.Text)]
    public string ProductName { get; set; }
    [Required]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }
    [Required]
    public int CategoryID { get; set; }
    
    public int Paid { get; set; }
    
    public int Count { get; set; }
    
    public Category Category { get; set; }

    public decimal TotalPrice()
    {
        return Count * Price;
    }

    public int LeftToPay()
    {
        return Count - Paid;
    }

    public decimal TotalPriceLeft()
    {
        return (Count - Paid) * Price;
    }
    
    
}