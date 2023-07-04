using AssessmentWebDev.Database.Items;
using Dapper;
using AssessmentWebDev.Database.Items;
namespace AssessmentWebDev.Database.Repository;

public class ProductRepository
{
    public int Add(Product product)
    {
        const string sql = @"INSERT INTO `stenden-cafe`.product (product_name, product_price, category_id)
                    VALUES (@ProductName, @Price, @CategoryID)";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, product);
    }

    public int Delete(Product product)
    {
        const string sql = @"DELETE FROM `stenden-cafe`.product WHERE product_id = @ProductID";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, product);
    }

    public int Update(Product product)
    {
        const string sql = @"UPDATE `stenden-cafe`.product SET product_name = @ProductName, product_price = @Price, category_id = @CategoryID WHERE product_id = @ProductID";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, product);
    }

    public List<Product>? Get()
    {
        const string sql = @"SELECT p.product_id as ProductID, p.product_name as ProductName, p.product_price as Price, p.category_id AS CategoryID, 
       c.category_id AS CategoryID, c.name as CategoryName
        FROM `stenden-cafe`.product p left join `stenden-cafe`.category c on c.category_id = p.category_id";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Query<Product, Category, Product>(sql, (product, category) =>
        {
            product.Category = category;
            return product;
        }, splitOn:"CategoryID").ToList();
    }
    
    public List<Product>? Get(int categoryID)
    {
        const string sql = @"SELECT p.product_id as ProductID, p.product_name as ProductName, p.product_price as Price, p.category_id AS CategoryID, 
       c.category_id AS CategoryID, c.name as CategoryName
        FROM `stenden-cafe`.product p left join `stenden-cafe`.category c on c.category_id = p.category_id WHERE p.category_id = @categoryID";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Query<Product, Category, Product>(sql, (product, category) =>
        {
            product.Category = category;
            return product;
        }, new {categoryID},
            splitOn:"CategoryID").ToList();
    }
    
}