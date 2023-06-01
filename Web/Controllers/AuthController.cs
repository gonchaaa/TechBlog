using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.DTOs;
using Web.Models;

namespace Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;


        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
         public async Task<IActionResult> Login(LoginDTO loginDTO)
        {

            var finduser = await _userManager.FindByEmailAsync(loginDTO.Email);
            if (finduser == null)
            {
                return RedirectToAction("Login");
            }
            Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(finduser,loginDTO.Password,false,false);
            if (result.Succeeded)
            {
                return RedirectToAction("Index","Home");
            }
            
            return View(loginDTO);
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {

            User user = new()
            {
                FirstName = registerDTO.FirstName,
                LastName = registerDTO.LastName,
                PhotoUrl = "/image/avatar.png",
                UserName = registerDTO.Email,
                Email = registerDTO.Email
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            return View(registerDTO);
        }
    }
}
