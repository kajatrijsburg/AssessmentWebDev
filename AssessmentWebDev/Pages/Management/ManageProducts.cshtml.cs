using System.Globalization;
using AssessmentWebDev.Database.Items;
using AssessmentWebDev.Database.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssessmentWebDev.Pages.Management;

[Authorize(Roles = "Manager")]
public class ManageProducts : PageModel
{
    [BindProperty] public List<Product>? Products { get; set; }
    public SelectList Categories;
    public void OnGet()
    {
        Products = new ProductRepository().Get();
        var categoryList = new CategoryRepository().Get();
        Categories = new SelectList(categoryList, nameof(Category.CategoryID), nameof(Category.CategoryName));
    }

    public IActionResult OnPostAdd(string ProductName, string Price, int CategoryID)
    {
        Product product = new();
        product.ProductName = ProductName;
        product.Price = Decimal.Parse(Price);
        product.CategoryID = CategoryID;

        new ProductRepository().Add(product);
        return RedirectToPage("/Management/ManageProducts");
    }
    
    public IActionResult OnPostDelete(int ProductID)
    {
        Product product = new();
        product.ProductID = ProductID;
        new ProductRepository().Delete(product);
        return RedirectToPage("/Management/ManageProducts");
    }
    
    public IActionResult OnPostUpdate(int ProductID, string ProductName, string Price, int CategoryID)
    {
        Product product = new();
        product.ProductID = ProductID;
        product.ProductName = ProductName;
        product.Price = Decimal.Parse(Price, CultureInfo.InvariantCulture);
        product.CategoryID = CategoryID;

        new ProductRepository().Update(product);
        return RedirectToPage("/Management/ManageProducts");
    }
}