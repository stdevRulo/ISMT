﻿using BizData.Entities;
using BizDbAccess.GenericInterfaces;
using BizDbAccess.Utils;
using BizLogic.Authentication;
using DataLayer.EfCode;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceLayer.AccountServices;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TripManager2._0.ViewModels;

namespace TripManager2._0.Controllers
{
    //Example for how to use policy:    [Authorize("VisaManager")]
    [Authorize]
    [ResponseCache(Location = ResponseCacheLocation.None, NoStore = true)]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _context;
        private readonly UserManager<Usuario> _userManager;
        private readonly SignInManager<Usuario> _signInManager;
        private readonly IConfiguration _config;

        public AccountController(IUnitOfWork context,
            SignInManager<Usuario> signInManager,
            UserManager<Usuario> userManager,
            IConfiguration config)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
            _config = config;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            var cmd = new RegisterUsuarioCommand();
            cmd.SetViewModel(user);
            return View(cmd);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit(RegisterUsuarioCommand cmd)
        {
            if (ModelState.IsValid)
            {
                var log = new LoginService(_context, _signInManager, _userManager);
                var userToUpd = await _userManager.GetUserAsync(User);
                var user = cmd.ToUsuario();

                user = await log.EditUserAsync(user, userToUpd);
                await _signInManager.RefreshSignInAsync(user);
                
                var uvm = new UserViewModel();
                uvm.SetProperties(cmd);
                uvm.Email = User.Identity.Name;
                uvm.SetPermissions(User.Claims);

                if (Request.Query.Keys.Contains("ReturnUrl"))
                {
                    return Redirect(Request.Query["ReturnUrl"].First());
                }
                else
                {
                    return RedirectToAction("Welcome", "User", uvm);
                }
                
            }

            ModelState.AddModelError(string.Empty, "An error occured trying to register the user");

            //If we got to here, something went wrong
            return View(cmd);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterUsuarioCommand cmd)
        {  
            if (ModelState.IsValid)
            {
                var _registerService = new RegisterService(_context, _signInManager, _userManager);
                var user = cmd.ToUsuario();
                var result = await _registerService.RegisterUsuarioAsync(user, cmd.Password);

                if (result.Succeeded)
                {
                    var claim = new Claim("Permission", "Normal");
                    await _userManager.AddClaimAsync(user, claim);
                    await _userManager.AddClaimAsync(user, new Claim("Pending", "true"));

                    await _signInManager.SignInAsync(user, false);

                    if (Request.Query.Keys.Contains("ReturnUrl"))
                    {
                        return Redirect(Request.Query["ReturnUrl"].First());
                    }
                    else
                    {
                        return RedirectToAction("Pending", "Home");
                    }
                }
                AddErrors(result);
            }

            ModelState.AddModelError(string.Empty, "An error occured trying to register the user");
            
            //If we got to here, something went wrong
            return View(cmd);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel lvm)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(lvm.Email,
                                                                lvm.Password,
                                                                lvm.RememberMe,
                                                                false);

                if (result.Succeeded)
                {
                    return RedirectToAction("LoginHelper", "Home");
                }
            }

            ModelState.AddModelError(string.Empty, "An error occured trying to login");

            //If we got to here, something went wrong
            return View(lvm);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        [HttpPost]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);

                if (user != null)
                {
                    var result = await _signInManager.CheckPasswordSignInAsync(user, model.Password, false);

                    if (result.Succeeded)
                    {
                        // Create the token
                        var claims = new[]
                        {
                            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
                        };
                        
                        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Tokens:Key"]));
                        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                        var token = new JwtSecurityToken(
                            _config["Tokens:Issuer"],
                            _config["Tokens:Audience"],
                            claims,
                            expires: DateTime.Now.AddMinutes(30),
                            signingCredentials: creds);

                        var results = new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(token),
                            expiration = token.ValidTo
                        };

                        return Created("", results);
                    }
                }
            }

            return BadRequest();
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
