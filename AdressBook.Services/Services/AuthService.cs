using AdressBook.Domain.Helpers;
using AdressBook.Domain.Models;
using AdressBook.Domain.ViewModels;
using AdressBook.Domain.ViewModels.User;
using AdressBook.Infrastructure.Data;
using AdressBook.Services.IServices;
using AdressBook.Services.IServices;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AdressBook.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> userManager,
                      RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt, AppDbContext context, IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _mapper = mapper;
            _jwt = jwt.Value;
            _roleManager = roleManager;

        }
        public async Task<ApiResponse<AuthModel>> Login(LoginModel model)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return new ApiResponse<AuthModel>("البريد الإلكتروني غير صحيح.");
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return new ApiResponse<AuthModel>("كلمة السر غير صحيحة.");
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);


            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.UserId = user.Id;
            authModel.Username = user.UserName;
            authModel.FirstName = user.FirstName;
            authModel.LastName = user.LastName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();

            return new ApiResponse<AuthModel>(authModel);
        }

        public async Task<ApiResponse<AuthModel>> RegisterAsync(RegisterModel model)
        {
            // check Email
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new ApiResponse<AuthModel> { Message = "الإيميل موجود من قبل", Succeeded = false };

            //check User Name
            if (await _userManager.FindByEmailAsync(model.UserName) is not null)
                return new ApiResponse<AuthModel> { Message = "إسم المستخدم موجود من قبل", Succeeded = false };


            var existingUser = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == model.PhoneNumber);

            if (existingUser != null)
            {
                return new ApiResponse<AuthModel>
                {
                    Message = "رقم الهاتف مسجل من قبل",
                    Succeeded = false
                };
            }


            var user = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            #region check if regiseration Done
            // check id creation done successful
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description} ,";
                }
                return new ApiResponse<AuthModel> { Message = errors, Succeeded = false };
            }
            #endregion

            var jwtSecurityToken = await CreateJwtToken(user);

            // Get the roles for the user
            var roles = await _userManager.GetRolesAsync(user);

            var authModel = new AuthModel();

            authModel.Email = user.Email;
            authModel.Username = user.UserName;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Roles = roles.ToList();

            return new ApiResponse<AuthModel>
            {
                Data = authModel,
                Message = " تم إنشاء مستخدم جديد بنجاح",
                Succeeded = true
            };
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddHours(_jwt.DurationInHours),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;

        }
    }
}