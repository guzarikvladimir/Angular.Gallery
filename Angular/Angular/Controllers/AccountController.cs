using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using System.Web.Security;
using Angular.Infrastructure;
using Angular.Models;
using Angular.Providers;
using BLL.Interfaces.Services;

namespace Angular.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        [AllowAnonymous]
        public JsonResult Login(string email, string password)
        {
            if (Membership.ValidateUser(email, password))
            {
                var user = userService.GetByEmail(email);
                return Json(new User()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Password = user.Password,
                    RoleId = user.Id,
                    Role = user.Role.Name
                });
            }
            Response.StatusCode = 401;
            return Json(false);
        }

        [AllowAnonymous]
        public JsonResult Register(string email, string password)
        {
            var anyUser = userService.GetByEmail(email);

            if (anyUser != null)
            {
                return Json(false);
            }

            var membershipUser = ((CustomMembershipProvider)Membership.Provider).CreateUser(email,
                password);

            if (membershipUser != null)
            {
                return Json(true);
            }
            return Json(false);
        }
    }
}