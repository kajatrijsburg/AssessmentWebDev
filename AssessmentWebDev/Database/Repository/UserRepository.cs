using AssessmentWebDev.Database.Items;
using Dapper;

namespace AssessmentWebDev.Database.Repository;

public class UserRepository
{
    public int Add(User user)
    {
        const string sql = @"INSERT INTO users (email, first_name, last_name, role, encrypted_password)
                    values (@Email, @FirstName, @LastName, @Role, @EncryptedPassword)";

        using var connection = new DbUtils().GetDbConnection();
        return connection.Execute(sql, user);
    }

    public User Get(string email)
    {
        const string sql = @"SELECT email AS Email, first_name AS FirstName, last_name AS LastName, role AS Role, encrypted_password AS EncryptedPassword, approved AS Approved FROM users WHERE email = @email";

        using var connection = new DbUtils().GetDbConnection();

        var users = connection.Query<User>(sql, new {email});

        if (users.Any())
            return users.First();

        return new User();
    }

    public int Update(User user)
    {
        string sql = @"UPDATE users SET email = @Email, first_name = @FirstName, last_name = @LastName, role = @Role, encrypted_password = @EncryptedPassword, approved = @Approved WHERE email = @Email";

        using var connection = new DbUtils().GetDbConnection();

        return connection.Execute(sql, user);
    }
}