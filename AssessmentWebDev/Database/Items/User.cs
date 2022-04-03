using System.ComponentModel.DataAnnotations;
using Ubiety.Dns.Core.Common;

namespace AssessmentWebDev.Database.Items;

public class User
{
    [Required]
    [DataType(DataType.EmailAddress)]
    [StringLength(255)]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(63)]
    public string FirstName { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(63)]
    public string LastName { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(63)]
    public string Role { get; set; }
    
    [Required]
    [DataType(DataType.Text)]
    [StringLength(255)]
    public string EncryptedPassword { get; set; }
    
    [Required]
    public bool Approved { get; set; }
    
}

public static class UserRoles
{
    public static string Server = "Server";
    public static string Manager = "Manager";
}