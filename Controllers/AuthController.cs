using AuthenticationDemo.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace AuthenticationDemo.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        //private readonly ApplicationDbContext _context;
        public AuthController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config) {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = config;

        }
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterModel model) {
            var userExist = await _userManager.FindByNameAsync(model.UserName);
            if (userExist != null) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "User already exist!"

                });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName,
            };



            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response
                {
                    Status = "Error",
                    Message = "User Creation Failed! Please check the user details and try again"
                });
            }
            if (model.Role == "user") {
                if (!await _roleManager.RoleExistsAsync(UserRoles.User)) {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
                }

                if (await _roleManager.RoleExistsAsync(UserRoles.User)) {

                    await _userManager.AddToRoleAsync(user, UserRoles.User);

                }
            }

            if (model.Role == "admin") {
                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin)) {
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                }
                if (await _roleManager.RoleExistsAsync(UserRoles.Admin)) {
                    await _userManager.AddToRoleAsync(user, UserRoles.Admin);

                }
            }
            return Ok(new Response { Status = "Success", Message = "User created successfully" });
        }

        [HttpPost("login")]
        public  async Task<IActionResult> Login(LoginModel login) {
            var user = await _userManager.FindByNameAsync(login.UserName);
            if (user != null && await _userManager.CheckPasswordAsync(user, login.Password)) {
                // 1 user can have many roles
                var userRoles = await _userManager.GetRolesAsync(user);
                var authclaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                };
                foreach (var userRole in userRoles) {
                    authclaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                var authSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));

                var token = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddMinutes(20),
                claims: authclaims,
                signingCredentials: new SigningCredentials(authSigninKey, SecurityAlgorithms.HmacSha256));

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            else return Unauthorized();

        }

        //private string GenerateToken(User user) {

        //    var userRoles = _context.Roles.Where(r => r.RoleId == user.RoleId);
        //    var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
        //    var credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);
        //    var authclaims = new List<Claim>
        //        {
        //            new Claim(ClaimTypes.Name,user.UserName),
        //            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
        //        };
        //    var token = new JwtSecurityToken(
        //        issuer: _configuration["JWT:Issuer"],
        //        audience: _configuration["JWT:Audience"],
        //        claims: authclaims,
        //        expires: DateTime.Now.AddMinutes(15),
        //        signingCredentials: credentials);

        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}


    }
}

// repository pattern is not implemented by mam, u have to use repo pattern
