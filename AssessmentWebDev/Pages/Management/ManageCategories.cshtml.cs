using AssessmentWebDev.Database.Items;
using AssessmentWebDev.Database.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssessmentWebDev.Pages.Management;

[Authorize(Roles = "Manager")]
public class ManageCategories : PageModel
{
    [BindProperty] public List<Category>? Categories { get; set; }
    [BindProperty] public Category Category { get; set; }
    public void OnGet()
    {
        Categories = new CategoryRepository().Get();
    }

    public IActionResult OnPostAdd(string Name)
    {
        var category = new Category();
        category.CategoryName = Name;

        new CategoryRepository().Add(category);
        return RedirectToPage("/Management/ManageCategories");
    }

    public IActionResult OnPostDelete(int CategoryID)
    {
        var category = new Category();
        category.CategoryID = CategoryID;
        
        new CategoryRepository().Delete(category);
        return RedirectToPage("/Management/ManageCategories");
    }

    public IActionResult OnPostUpdate(int CategoryID, string Name)
    {
        var category = new Category();
        category.CategoryID = CategoryID;
        category.CategoryName = Name;
        
        new CategoryRepository().Update(Category);
        return RedirectToPage("/Management/ManageCategories");
    }
}