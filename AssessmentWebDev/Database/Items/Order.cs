using System.ComponentModel.DataAnnotations;

namespace AssessmentWebDev.Database.Items;

public class Order
{
    public int? OrderID { get; set; }
    [Required]
    [DataType(DataType.EmailAddress)]
    public string UserEmail { get; set; }
    [Required]
    public int TableNumber { get; set; }
    
    public User User { get; set; }
    
    public List<Product> Items { get; set; }

    public decimal TotalPrice()
    {
        decimal total = 0;
        foreach (var item in Items)
        {
            total += item.TotalPrice();
        }

        return total;
    }
    
    public decimal TotalPriceLeft()
    {
        decimal total = 0;
        
        foreach (var item in Items)
        {
            total += item.TotalPriceLeft();
        }

        return total;
    }
}