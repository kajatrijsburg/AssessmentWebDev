using AssessmentWebDev.Database.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssessmentWebDev.Pages.Order;

[Authorize]
public class PayIndividually : PageModel
{
    public Database.Items.Order order { get; set; }

    public IActionResult OnGet()
    {
        int? OrderID = HttpContext.Session.GetInt32("OrderID");

        if (OrderID == null)
        {
            return RedirectToPage("/Order/ManageOrders");
        }

        order = new OrderRepository().Get((int) OrderID);

        return null;
    }

    public IActionResult OnPostPay(string paidItems)
    {
        int? OrderID = HttpContext.Session.GetInt32("OrderID");

        if (OrderID == null)
        {
            return RedirectToPage("/Order/ManageOrders");
        }

        paidItems = Filter(paidItems, ',');
        var dict = paidItems.Split("id").Select(x => x.Split('='))
            .Where(x => x.Length > 1 && !String.IsNullOrEmpty(x[0].Trim()) && !String.IsNullOrEmpty(x[1].Trim()))
            .ToDictionary(x => x[0].Trim(), x => x[1].Trim());
        
        order = new OrderRepository().Get((int) OrderID);

        foreach (var item in order.Items)
        {
            if (dict.ContainsKey(item.ProductID.ToString()))
            {
                item.Paid = Int32.Parse(dict[item.ProductID.ToString()]); 
            }
        }
        
        new OrderProductRepository().UpdatePaidFromOrder(order);
        
        return RedirectToPage("/Order/ManageOrders");
    }

    public string Filter(string str, char c)
    {
        if (str.Length == 0 || str == null) return str;
        string outputstr = "";
        foreach (var strc in str)
        {
            if (strc != c)
            {
                outputstr += strc;
            }
        }

        return outputstr;
    }
}