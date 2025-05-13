// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Supermarket.Data;
using Supermarket.Models;

namespace Supermarket.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;


        public IndexModel(
      UserManager<ApplicationUser> userManager,
      SignInManager<ApplicationUser> signInManager,
      AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>

            [Required]
            [DisplayName("Name")]
            public string Name { get; set; }

            [Required]
            [DisplayName("Street Address")]
            public string StreetAdress { get; set; }
            [Required]
            [DisplayName("City")]
            public string City { get; set; }
            [Required]
            [DisplayName("State")]
            public string State { get; set; }

            [DisplayName("Profile picture")]
            public byte[] ProfilePic { get; set; }

            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                Name = user.Name,
                StreetAdress = user.StreetAdress,
                City = user.City,
                PhoneNumber = phoneNumber,
                ProfilePic = user.profilepic
            };
                
            
           

        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            var name = user.Name;
            var street = user.StreetAdress;
            var city = user.City;

            // Track modifications
            bool isUpdated = false;

            // Update properties
            if (Input.Name != name)
            {
                user.Name = Input.Name;
                isUpdated = true;
            }

            if (Input.StreetAdress != street)
            {
                user.StreetAdress = Input.StreetAdress;
                isUpdated = true;
            }

            if (Input.City != city)
            {
                user.City = Input.City;
                isUpdated = true;
            }

            // Update phone number
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
                isUpdated = true;
            }

            // Handle file upload and profile picture update
            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files.FirstOrDefault();
                if (file != null && file.Length > 0)
                {
                    using (var datastream = new MemoryStream())
                    {
                        await file.CopyToAsync(datastream);
                        user.profilepic = datastream.ToArray();
                        isUpdated = true;
                    }
                }
            }

            // Only update the user if any changes have been made
            if (isUpdated)
            {
                // Attach the user entity to the context for tracking
                _context.Attach(user);  // Ensures entity is being tracked
                _context.Entry(user).State = EntityState.Modified;  // Mark entity as modified
                await _context.SaveChangesAsync();  // Save changes to the database
            }

            await _signInManager.RefreshSignInAsync(user);  // Refresh sign-in
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }


    }
}
