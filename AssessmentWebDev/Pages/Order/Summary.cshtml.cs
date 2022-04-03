using AssessmentWebDev.Database.Items;
using AssessmentWebDev.Database.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssessmentWebDev.Pages.Order;

[Authorize]
public class Summary : PageModel
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
    
    public IActionResult OnPostItemCountDecrease(int Count, int Paid, int ProductID, int OrderID)
    {
        var orderProduct = new OrderProduct();
        var orderProductRepository = new OrderProductRepository();
        
        orderProduct.OrderID = OrderID;
        orderProduct.Count = Count-1;
        orderProduct.Paid = Paid;
        orderProduct.ProductID = ProductID;
        
        if (Count <= 1)
        {
            orderProductRepository.Delete(orderProduct);
        }
        else
        {
            orderProductRepository.Update(orderProduct);
        }
        
        return RedirectToPage("/Order/Summary");
    }

    public IActionResult OnPostItemCountIncrease(int Count, int Paid, int ProductID, int OrderID)
    {
        var orderProduct = new OrderProduct();
        var orderProductRepository = new OrderProductRepository();
        
        orderProduct.OrderID = OrderID;
        orderProduct.Count = Count+1;
        orderProduct.Paid = Paid;
        orderProduct.ProductID = ProductID;
        
        orderProductRepository.Update(orderProduct);
        
        return RedirectToPage("/Order/Summary");
    }
}