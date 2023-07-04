using AssessmentWebDev.Database.Items;
using Dapper;

namespace AssessmentWebDev.Database.Repository;

public class OrderRepository
{
    public int Add(Order order)
    {
        const string sql = @"INSERT INTO `stenden-cafe`.`order` (user_email, table_number)
                    VALUES (@UserEmail, @TableNumber)";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, order);
    }

    public int Delete(int OrderID)
    {
        const string sqlDeleteOrder = @"DELETE FROM `stenden-cafe`.`order` WHERE `order`.order_id = @OrderID";
        const string sqlDeleteOrderProduct = @"DELETE FROM `stenden-cafe`.order_product WHERE order_id = @OrderID";
        
        using var connection = new DbUtils().GetDbConnection();
        connection.Execute(sqlDeleteOrderProduct, new {OrderID});
        return connection.Execute(sqlDeleteOrder, new {OrderID});
    }

    public int Update(Order order)
    {
        const string sql = @"UPDATE `stenden-cafe`.`order` SET user_email = @UserEmail, table_number = @TableNumber WHERE order_id = @OrderID";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, order);
    }

    public List<Order>? Get(string userID)
    {
        const string sql = @"SELECT o.order_id AS OrderID, o.user_email AS UserEmail, o.table_number AS TableNumber, u.email AS Email, 
        u.first_name AS FirstName, u.last_name AS LastName, u.role AS Role 
        FROM `stenden-cafe`.`order` o left join `stenden-cafe`.users u ON o.user_email = u.email 
        WHERE u.email = @userID";

        using var connection = new DbUtils().GetDbConnection();
        var orders = connection.Query<Order, User, Order>(sql, (order, user) =>
        {
            order.User = user;
            return order;
        }, new {userID}, splitOn:"Email").ToList();

        foreach (var order in orders)
        {
            const string orderItemssql = @"SELECT c.category_id AS CategoryID, c.name AS CategoryName,
                                            p.product_id AS ProductID, p.product_price AS Price, p.product_name AS ProductName, op.paid AS Paid, op.count AS Count
                                            FROM `stenden-cafe`.order_product op 
                                            LEFT JOIN `stenden-cafe`.product p on p.product_id = op.product_id 
                                            LEFT JOIN `stenden-cafe`.category c on c.category_id = p.category_id 
                                            WHERE op.order_id = @OrderID";

            order.Items = connection.Query<Category, Product, Product>(orderItemssql, (category, product) =>
            {
                product.Category = category;
                return product;
            }, new{order.OrderID},splitOn:"ProductID").ToList();
        }

        return orders;
    }


    public Order Get(int OrderID)
    {
        const string sql = @"SELECT o.order_id AS OrderID, o.user_email AS UserEmail, o.table_number AS TableNumber, u.email AS Email, 
            u.first_name AS FirstName, u.last_name AS LastName, u.role AS Role FROM `stenden-cafe`.`order` o 
            left join `stenden-cafe`.users u ON o.user_email = u.email 
            WHERE o.order_id = @OrderID";

        using var connection = new DbUtils().GetDbConnection();
        var order = connection.Query<Order, User, Order>(sql, (order, user) =>
        {
            order.User = user;
            return order;
        }, new {OrderID}, splitOn:"Email").First();

        
        const string orderItemssql = @"SELECT c.category_id AS CategoryID, c.name AS CategoryName,
                                        p.product_id AS ProductID, p.product_price AS Price, p.product_name AS ProductName, op.paid AS Paid, op.count AS Count
                                        FROM `stenden-cafe`.order_product op 
                                        LEFT JOIN `stenden-cafe`.product p on p.product_id = op.product_id 
                                        LEFT JOIN `stenden-cafe`.category c on c.category_id = p.category_id 
                                        WHERE op.order_id = @OrderID";

        order.Items = connection.Query<Category, Product, Product>(orderItemssql, (category, product) =>
        {
            product.Category = category;
            return product;
        }, new{order.OrderID},splitOn:"ProductID").ToList();
        

        return order;
    }
}
