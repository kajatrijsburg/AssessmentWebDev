using AssessmentWebDev.Database.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssessmentWebDev.Pages.Order;

[Authorize]
public class PayAll : PageModel
{
    public Database.Items.Order order { get; set; }
    
    public IActionResult OnGet()
    {
        int? OrderID = HttpContext.Session.GetInt32("OrderID");

        if (OrderID == null)
        {
            return RedirectToPage("/Order/ManageOrders");
        }
        
        order = new OrderRepository().Get((int)OrderID);
        
        return null;
    }

    public IActionResult OnPostPay()
    {
        int? OrderID = HttpContext.Session.GetInt32("OrderID");

        if (OrderID == null)
        {
            return RedirectToPage("/Order/ManageOrders");
        }

        new OrderProductRepository().PayAll((int)OrderID);

        return RedirectToPage("/Order/ManageOrders");
    }
}