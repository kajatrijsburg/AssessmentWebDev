using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using AssessmentWebDev.Database.Items;
using AssessmentWebDev.Database.Repository;
using AssessmentWebDev.utils;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AssessmentWebDev.Pages.Account;

public class Login : PageModel
{
    [BindProperty] public Credential credential { get; set; } = new Credential();
        
        public void OnGet()
        {
        }
        
        public IActionResult OnPost()
        {
            //check if the data is valid
            if (!ModelState.IsValid)
            {
                return Page();
            }
            
            //make a db connection and get the user from db
            UserRepository userRepository = new UserRepository();
            User user = userRepository.Get(credential.UserName);
            
            //check if the user exists
            if (user.Email != credential.UserName)
            {
                ModelState.AddModelError("LogOnError", "The user name or password provided is incorrect.");
                return Page();
            }
            //check if the password is correct
            switch (new PasswordHasher<object?>().VerifyHashedPassword(null, user.EncryptedPassword, credential.Password) ){
                //if it's correct sign the user in
                case PasswordVerificationResult.Success:
                    SignInUser(user);
                    return RedirectToPage("/Index");
                //if it isn't keep the user on the page
                case PasswordVerificationResult.Failed:
                    ModelState.AddModelError("LogOnError", "The user name or password provided is incorrect.");
                    return null;
                //if it's correct but the password needs to be rehashed, rehash the password and sign the user in
                case PasswordVerificationResult.SuccessRehashNeeded:
                    SignInUser(user);
                    user.EncryptedPassword =
                        new PasswordHasher<object?>().HashPassword(null, credential.Password);
                {
                    userRepository.Update(user);
                }
                    
                    return RedirectToPage("/Index");
            }

            return null;
        }

        private async void SignInUser(User user)
        {
            
            //create a new security context
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.FirstName + " " + user.LastName),
                new Claim(ClaimTypes.Role, user.Role),
            };
            //create a new identity with the appropriate claims
            var identity = new ClaimsIdentity(claims, Auth.AuthenticationSchemeName);
            
            //create a claimsPrincipal and associate it with the identity
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
            //add it to the http context ass a cookie
            await HttpContext.SignInAsync(Auth.AuthenticationSchemeName, claimsPrincipal);
        }
        
        public class Credential
        {
            [Required]
            [DataType(DataType.EmailAddress)]
            [Display(Name = "Email Address")]
            
            public string UserName { get; set; }
            [Required]
            [DataType(DataType.Password)]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [Display(Name = "Password")]  
            public string Password { get; set; }
        }
}