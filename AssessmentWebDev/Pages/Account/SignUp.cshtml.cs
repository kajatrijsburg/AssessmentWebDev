using System.ComponentModel.DataAnnotations;
using AssessmentWebDev.Database.Items;
using AssessmentWebDev.Database.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssessmentWebDev.Pages.Account;

public class SignUp : PageModel
{
    [BindProperty] public NewAccountInfo newAccountInfo { get; set; }



    public void OnGet()
    {
        
    }

    public IActionResult OnPost()
    {
        if (!ModelState.IsValid) return Page();

        UserRepository userRepository = new UserRepository();
        userRepository.Add(newAccountInfo.ToUser());
        return RedirectToPage("/Account/Login");
    }

    public class  NewAccountInfo
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
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "Het {0} moet ten minste {2} en maximaal {1} tekens lang zijn.", MinimumLength = 6)]
        [Display(Name = "Password")]  
        public string Password { get; set;}
    
        [Required]
        [Display(Name = "Confirm password")]
        [Compare(nameof(Password), ErrorMessage = "Het wachtwoord en bevestiging van wachtwoord komen niet overeen")]
        public string RepeatPassword { get; set; }

        public User ToUser()
        {
            User user = new User();
            user.Email = Email;
            user.Role = UserRoles.Server;
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.EncryptedPassword = new PasswordHasher<object?>().HashPassword(null, Password);
                
            return user;
        }
    }
}