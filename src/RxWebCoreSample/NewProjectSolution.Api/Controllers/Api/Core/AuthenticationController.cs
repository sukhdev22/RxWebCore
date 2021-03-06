using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NewProjectSolution.Infrastructure.Security;
using NewProjectSolution.Models.Main;
using NewProjectSolution.Models.ViewModels;
using NewProjectSolution.UnitOfWork.Main;
using RxWeb.Core.AspNetCore.Filters;
using RxWeb.Core.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace NewProjectSolution.Api.Controllers
{
	[ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private ILoginUow LoginUow { get; set; }
        private IApplicationTokenProvider ApplicationTokenProvider { get; set; }
        private IPasswordHash PasswordHash { get; set; }

        public AuthenticationController(ILoginUow loginUow, IApplicationTokenProvider tokenProvider, IPasswordHash passwordHash)
        {
            this.LoginUow = loginUow;
            ApplicationTokenProvider = tokenProvider;
            PasswordHash = passwordHash;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get() {
            var token = await ApplicationTokenProvider.GetTokenAsync(new vUser { UserId=0,ApplicationTimeZoneName=string.Empty,LanguageCode=string.Empty });
            return Ok(token);
        }

        [HttpPost]
        [AllowAnonymousUser]
        public async Task<IActionResult> Post(AuthenticationModel authentication)
        {
            //var inactive = await LoginUow.Repository<InactiveUser>().AllAsync();
            int a = 1, b = 0, c = a / b;
            var users = await LoginUow.Repository<User>().AllAsync();
            var user = await LoginUow.Repository<vUser>().SingleOrDefaultAsync(t => t.UserName == authentication.UserName && !t.LoginBlocked);
            if (user != null && PasswordHash.VerifySignature(authentication.Password, user.Password, user.Salt))
            {
                var token = await ApplicationTokenProvider.GetTokenAsync(user);
                return Ok(token);
            }
            else
                return BadRequest();
        }
    }
}

