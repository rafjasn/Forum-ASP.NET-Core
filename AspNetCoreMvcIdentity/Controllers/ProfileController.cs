using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCoreMvcIdentity.Models;
using AspNetCoreMvcIdentity.Models.ApplicationUserViewModels;
using AspNetCoreMvcIdentity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvcIdentity.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IApplicationUser _user;
        private readonly IUpload _upload;

        public ProfileController(UserManager<ApplicationUser> userManager, IApplicationUser user, IUpload upload)
        {
            _userManager = userManager;
            _user = user;
            _upload = upload;
        }

        

        public IActionResult Details(long id)
        {
            var user = _user.GetById(id);
            var userRoles = _userManager.GetRolesAsync(user).Result;
            var model = new ProfileModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                UserRating = user.Rating.ToString(),
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                MemberSince = user.MemberSince,
                IsAdmin = userRoles.Contains("Admin")
            };
            return View(model);
        }
        //[HttpPost]
        //public async Task<IActionResult> UploadProfileImage(IFormFile file)
        //{
        //    var userId = _userManager.GetUserId(User);
        //}


        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            var profiles = _user.GetAll().Select(u => new ProfileModel
            {
                UserId = u.Id,
                Email = u.Email,
                UserName = u.UserName,
                ProfileImageUrl = u.ProfileImageUrl,
                UserRating = u.Rating.ToString(),
                MemberSince = u.MemberSince

            });

            var model = new ProfileListModel
            {
                Profiles = profiles
            };
            return View(model);
                
       }
    }
}