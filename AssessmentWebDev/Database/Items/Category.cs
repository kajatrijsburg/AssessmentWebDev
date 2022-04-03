using System.ComponentModel.DataAnnotations;

namespace AssessmentWebDev.Database.Items;

public class Category
{
    public int CategoryID { get; set; }
    [Required]
    [DataType(DataType.Text)]
    public string CategoryName { get; set; }
}