using AssessmentWebDev.Database.Items;
using Dapper;

namespace AssessmentWebDev.Database.Repository;

public class CategoryRepository
{
    public int Add(Category category)
    {
        const string sql = @"INSERT INTO `stenden-cafe`.category (name)
                    VALUES (@CategoryName)";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, category);
    }

    public int Delete(Category category)
    {
        const string sql = @"DELETE FROM `stenden-cafe`.category WHERE category_id = @CategoryID";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, category);
    }

    public int Update(Category category)
    {
        const string sql = @"UPDATE `stenden-cafe`.category SET name = @CategoryName WHERE category_id = @CategoryID";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, category);
    }

    public List<Category>? Get()
    {
        const string sql = @"SELECT category_id AS CategoryID, name AS CategoryName FROM `stenden-cafe`.category";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Query<Category>(sql).ToList();
    }
}