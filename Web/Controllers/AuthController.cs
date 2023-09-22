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
        private readonly IHttpContextAccessor _httpContext;


        public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IHttpContextAccessor httpContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContext = httpContext;
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
                string c = _httpContext.HttpContext.Request.Query["controller"];
                string a = _httpContext.HttpContext.Request.Query["action"];
                string i = _httpContext.HttpContext.Request.Query["id"];
                if (!string.IsNullOrWhiteSpace(c))
                {
                    return RedirectToAction(a,c,new {id=i});
                }


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
            await _userManager.AddToRoleAsync(user,"User");
            
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            return View(registerDTO);
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
           await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }

    }
}
