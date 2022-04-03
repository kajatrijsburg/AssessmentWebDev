using AssessmentWebDev.Database.Items;
using AssessmentWebDev.Pages.Order;
using Dapper;
using Order = AssessmentWebDev.Database.Items.Order;

namespace AssessmentWebDev.Database.Repository;

public class OrderProductRepository
{
    public int Add(OrderProduct orderProduct)
    {
        const string sql = @"INSERT INTO order_product (product_id, order_id, count, paid)
                    VALUES (@ProductID, @OrderID, @Count, @Paid)";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, orderProduct);
    }

    public OrderProduct? Get(OrderProduct orderProduct)
    {
        const string sql = @"SELECT product_id AS ProductID, order_id AS OrderID, paid AS Paid, count AS Count FROM order_product WHERE order_id = @OrderID AND product_id = @ProductID";
        using var connection = new DbUtils().GetDbConnection();
        return connection.Query<OrderProduct>(sql, orderProduct).FirstOrDefault();
    }
    
    /*
    public List<OrderProduct> Get()
    {
        const string sql = @"SELECT product_id AS ProductID, order_id AS OrderID, paid AS Paid, count AS Count FROM order_product";
        using var connection = new DbUtils().GetDbConnection();
        return connection.Query<OrderProduct>(sql).ToList();
    } */

    public int PayAll(int OrderID)
    {
        const string sql = @"UPDATE order_product SET paid = count WHERE order_id = @OrderID";
        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, new{OrderID});
        
    }
    
    public int Update(OrderProduct orderProduct)
    {
        const string sql = @"UPDATE order_product SET count = @Count, paid = @Paid WHERE product_id = @ProductID AND order_id = @OrderID";
        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, orderProduct);
    }

    public void UpdatePaidFromOrder(Order order)
    {
        using var connection = new DbUtils().GetDbConnection();
        string sql = "";
        foreach (var item in order.Items)
        {
            sql = @"UPDATE order_product SET paid = @Paid WHERE product_id = @ProductID AND order_id = @OrderID";
            connection.Execute(sql, new {item.Paid, item.ProductID, order.OrderID});
        }
    }

    public int Delete(OrderProduct orderProduct)
    {
        const string sql = @"DELETE FROM order_product WHERE product_id = @ProductID AND order_id = @OrderID";
        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, orderProduct);
    }
    
}