using System.Security.Claims;
using AssessmentWebDev.Database.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using AssessmentWebDev.Database.Items;
using Microsoft.AspNetCore.Mvc;

namespace AssessmentWebDev.Pages.Order;

[Authorize]
public class ManageOrders : PageModel
{
    public int? SelectedOrderID { get; set; }
    public List<Database.Items.Order>? orders { get; set; } 
    public void OnGet()
    {
        SelectedOrderID = HttpContext.Session.GetInt32("OrderID");
        orders = new OrderRepository().Get(GetUserEmail());
    }

    public IActionResult OnPostNewOrder(int TableNumber)
    {
        Database.Items.Order order = new();
        order.TableNumber = TableNumber;
        order.UserEmail = GetUserEmail();
        new OrderRepository().Add(order);
        return RedirectToPage("/Order/ManageOrders");
    }

    public IActionResult OnPostSelectOrder(int OrderID)
    {
        HttpContext.Session.SetInt32("OrderID", OrderID);
        return RedirectToPage("/Order/ManageOrders");
    }

    public IActionResult OnPostDeleteOrder(int OrderID)
    {
        new OrderRepository().Delete(OrderID);
        return RedirectToPage("/Order/ManageOrders");
    }
    
    private string? GetUserEmail()
    {
        var claims = User.Claims;
        return (from claim in claims where claim.Type == ClaimTypes.Email select claim.Value).FirstOrDefault();
    }
}