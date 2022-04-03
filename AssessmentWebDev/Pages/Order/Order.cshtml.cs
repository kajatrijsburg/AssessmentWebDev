using AssessmentWebDev.Database.Items;
using AssessmentWebDev.Database.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AssessmentWebDev.Pages.Order;

[Authorize]
public class Order : PageModel
{
    [BindProperty] public List<Product>? Products { get; set; }
    public List<Category> Categories { get; set; }
    public Category SelectedCategory { get; set; }
    
    public int? OrderID { get; set; }
    
    public Database.Items.Order order { get; set; }

    
    
    
    
    public IActionResult OnGet(int CategoryID)
    {
        OrderID = HttpContext.Session.GetInt32("OrderID");

        if (OrderID == null)
        {
            return RedirectToPage("/Order/ManageOrders");
        }
        
        order = new OrderRepository().Get((int)OrderID);
        
        Categories = new CategoryRepository().Get();
        
        if (CategoryID == 0)
        {
            CategoryID = Categories.First().CategoryID;
        }
        
        
        SelectedCategory = Categories.Find(x => x.CategoryID == CategoryID);
        Products = new ProductRepository().Get(SelectedCategory.CategoryID);

        return null;
    }

    public IActionResult OnPostSelectCategory(int CategoryID)
    {
        return RedirectToPage("/Order/Order", new {CategoryID});
    }

    public IActionResult OnPostAddToOrder(int ProductID)
    {
        OrderID = HttpContext.Session.GetInt32("OrderID");
        
        if (OrderID == null)
        {
            return RedirectToPage("/Order/ManageOrders");
        }
        
        OrderProductRepository orderProductRepository = new();
        OrderProduct? orderProduct = new()
        {
            OrderID = (int)OrderID,
            ProductID = ProductID
        };

        //check if there's already an order of this type in the system
        orderProduct = orderProductRepository.Get(orderProduct);
        if (orderProduct == null)
        {
            //if there isn't make one and add it to the system
            orderProduct = new();
            orderProduct.OrderID = (int) HttpContext.Session.GetInt32("OrderID"); //this will never be null because we check for that at the start of the function
            orderProduct.ProductID = ProductID;
            orderProduct.Paid = 0;
            orderProduct.Count = 1;
            orderProductRepository.Add(orderProduct);
        }
        else
        {
            orderProduct.Count++;
            orderProductRepository.Update(orderProduct);
        }
        return RedirectToPage("/Order/Order");
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
        
        return RedirectToPage("/Order/Order");
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
        
        return RedirectToPage("/Order/Order");
    }

    public IActionResult OnPostItemDelete(int ProductID, int OrderID)
    {
        var orderProduct = new OrderProduct();
        var orderProductRepository = new OrderProductRepository();
        
        orderProduct.ProductID = ProductID;
        orderProduct.OrderID = OrderID;
        orderProductRepository.Delete(orderProduct);
        
        return RedirectToPage("/Order/Order");
    }
}