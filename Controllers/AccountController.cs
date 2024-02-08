using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
using Tulip.Models;
using Tulip.Data;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;


namespace Tulip.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationDbContext _db;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IEmailSender emailSender,
            UrlEncoder urlEncoder, RoleManager<IdentityRole> roleManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _db = db;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AdminLogin(string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult LoginAS(string appuser)
        {
            if (appuser == "sysuser")
            {
                return View("Login");
            }
            else
            {
                return View("AdminView");
            }


        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult InitialPage(/*string returnurl = null*/)
        {
            //ViewData["ReturnUrl"] = returnurl;
            return View();
        }
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            returnUrl = returnUrl ?? Url.Content("/Home/Index");

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserId, model.Password, model.RememberMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    bool isPasswordPatternMatch = IsPasswordPatternMatch(model.Password);
                    TempData["IsPasswordPatternMatch"] = isPasswordPatternMatch;


                    // Pass the flag to the view via ViewData
                    ViewData["IsPasswordPatternMatch"] = isPasswordPatternMatch;
                    // Check if the password matches the pattern "Password@###"
                    if (IsPasswordPatternMatch(model.Password))
                    {
                        TempData["RequirePasswordChange"] = true;
                        //ModelState.AddModelError(string.Empty, "Please change your password according to some conditions.");
                        return LocalRedirect(Url.Content("/Account/ResetPassword"));
                    }
                    else
                    {
                        TempData["RequirePasswordChange"] = false;
                        return LocalRedirect(returnUrl);
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }

            return View(model);
        }

        private bool IsPasswordPatternMatch(string password)
        {
            string pattern = @"^Password@\d{3}$";
            return Regex.IsMatch(password, pattern);
        }

        /*
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdminLogin(LoginViewModel model, string returnurl = null)
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("/Account/ResetPassword");
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.UserId, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return LocalRedirect(returnurl);
                }

                //if (result.IsLockedOut)
                //{
                //    return View("Lockout");
                //}
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return View(model);
                }
            }


            return View(model);
        }
        */
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Register(string returnurl = null)
        {
            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                //create roles
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("User"));
            }

            List<SelectListItem> listItems = new List<SelectListItem>();
            listItems.Add(new SelectListItem()
            {
                Value = "Admin",
                Text = "Admin"
            });
            listItems.Add(new SelectListItem()
            {
                Value = "User",
                Text = "User"
            });



            ViewData["ReturnUrl"] = returnurl;
            RegisterViewModel registerViewModel = new RegisterViewModel()
            {
                RoleList = listItems
            };
            return View(registerViewModel);
        }

            /**
            @brief Handles HTTP POST requests to create multiple users.*
            This method is restricted to users with the "Admin" role.
            *
            @param startingIndex The starting index for generating user data.
            @param numberOfUsersToCreate The number of users to create.
            @param returnurl The URL to redirect to after creating users. Defaults to "/Account/GetUsers".
            *
            @return IActionResult representing the result of the operation.
            */
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUsers(int startingIndex, int numberOfUsersToCreate, string email, string returnurl = null)  
        {
            ViewData["ReturnUrl"] = returnurl;
            returnurl = returnurl ?? Url.Content("/Account/GetUsers");

            // Common data for all users
            var commonUserData = new RegisterViewModel
            {
                ApplicationServer = "CommonServer",
                RoleSelected = "User"
            };

            for (int i = startingIndex; i < startingIndex + numberOfUsersToCreate; i++)
            {
                // Generate unique data for each user
                var userData = new RegisterViewModel
                {
                    Username = $"LEARN-{i:000}",
                    //UserId = $"LEARN-{i:000}",
                    Password = $"Password@{i:000}",
                    Email = email, //takes user input email
                    Name = $"LEARN-{i:000}",
                    ClientId = 101,
                    //ApplicationServer = "trek.ucc.uwm.edu"
                };

                // Create the user
                if (ModelState.IsValid)
                {
                    var user = new ApplicationUser
                    {
                        UserName = userData.Username,
                        Email = userData.Email,
                        Name = userData.Name,
                        ApplicationServer = userData.ApplicationServer,
                        //ApplicationServer = commonUserData.ApplicationServer, // Use common data
                        ClientId = userData.ClientId, // Use unique data
                                                      //UserId = userData.UserId, // Use unique data
                    };

                    var result = await _userManager.CreateAsync(user, userData.Password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, commonUserData.RoleSelected); // Use common data
                    }
                    else
                    {
                        // Handle user creation errors for this specific user
                        // You can add error messages to ModelState or log them
                        // ModelState.AddModelError(string.Empty, "User creation failed.");
                    }
                }
                else
                {
                    // Handle invalid model state for this specific user
                    // You can add error messages to ModelState or log them
                    // ModelState.AddModelError(string.Empty, "Invalid registration data.");
                }
            }

            // Redirect to the return URL after creating all users
            return LocalRedirect(returnurl);
        }
        //used asynchronous method "await" to prevent potentially blocking operations from database queries. 

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers()
        {
            var userList = await _db.ApplicationUsers.ToListAsync();
            var userRole = await _db.UserRoles.ToListAsync();
            var roles = await _db.Roles.ToListAsync();
            foreach (var user in userList)
            {
                var role = userRole.FirstOrDefault(u => u.UserId == user.Id);
                if (role == null)
                {
                    user.Role = "None";
                }
                else
                {
                    user.Role = roles.FirstOrDefault(u => u.Id == role.RoleId).Name;
                }
            }

            return View(userList); ;
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(string userId)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            if (objFromDb == null)
            {
                return NotFound();
            }
            var userRole = _db.UserRoles.ToList();
            var roles = _db.Roles.ToList();
            var role = userRole.FirstOrDefault(u => u.UserId == objFromDb.Id);
            if (role != null)
            {
                objFromDb.RoleId = roles.FirstOrDefault(u => u.Id == role.RoleId).Id;
            }
            objFromDb.RoleList = _db.Roles.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = u.Name,
                Value = u.Id
            });
            return View(objFromDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(ApplicationUser user)
        {
            if (ModelState.IsValid)
            {
                var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == user.Id);
                if (objFromDb == null)
                {
                    return NotFound();
                }
                var userRole = _db.UserRoles.FirstOrDefault(u => u.UserId == objFromDb.Id);
                if (userRole != null)
                {
                    var previousRoleName = _db.Roles.Where(u => u.Id == userRole.RoleId).Select(e => e.Name).FirstOrDefault();
                    //removing the old role
                    await _userManager.RemoveFromRoleAsync(objFromDb, previousRoleName);

                }

                //add new role
                await _userManager.AddToRoleAsync(objFromDb, _db.Roles.FirstOrDefault(u => u.Id == user.RoleId).Name);
                objFromDb.Name = user.Name;
                _db.SaveChanges();
                TempData[SD.Success] = "User has been edited successfully.";
                return RedirectToAction(nameof(GetUsers));
                // return Ok();
            }


            user.RoleList = _db.Roles.Select(u => new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
            {
                Text = u.Name,
                Value = u.Id
            });
            return View(user);
        }



        //[HttpGet]
        //[Authorize(Roles = "Admin,User")]
        //public async Task<IActionResult> Logout()
        //{
        //    return View();
        //}

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            // return RedirectToAction(nameof(InitialPage));
            return RedirectToAction(nameof(Login));

        }



        [HttpGet]
        [AllowAnonymous]
        [Authorize(Roles = "Admin,User")]
        public IActionResult ResetPassword()
        {
            return View();
        }

        /**
        @brief Handles HTTP GET requests to display the reset password confirmation view.*
        This method is accessible to users with either the "Admin" or "User" role.*
        @return IActionResult representing the result of the operation, displaying the reset password confirmation view.
        */
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {

            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }

                var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, code, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("ResetPasswordConfirmation");
                }
                AddErrors(result);
            }

            return View();
        }

        [HttpPost]

        [Authorize(Roles = "Admin")]
        public IActionResult Delete(string userId)
        {
            var objFromDb = _db.ApplicationUsers.FirstOrDefault(u => u.Id == userId);
            if (objFromDb == null)
            {
                return NotFound();
            }
            _db.ApplicationUsers.Remove(objFromDb);
            _db.SaveChanges();
            TempData[SD.Success] = "User deleted successfully.";
            return RedirectToAction(nameof(GetUsers));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}
