using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using School.Data;
using School.Options;
using System.Security.Claims;
using School.Contracts;
using School.Contracts.Responses;

namespace School.Services.Impl
{
    public class IdentityService : IIdentityService
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SchoolContext _entity;

        public IdentityService(UserManager<ApplicationUser> userManager, JwtSettings jwtSettings, RoleManager<IdentityRole> roleManager, SchoolContext entity)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _roleManager = roleManager;
            _entity = entity;
        }

        public async Task<AuthenticationResult> RegisterAsync(UserRegistrationRequest model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"User with this email address already exists."}
                };
            }

            var person = new Person
            {
                FirstName = model.FirstName,
                LastName = model.LastName
            };

            var res = _entity.Person.Add(person);
            _entity.SaveChanges();

            var newUser = new ApplicationUser
            { 
                Email = model.Email,
                UserName = model.Email,
                PersonId = person.Id,
                PhoneNumber = model.PhoneNumber
                
            };

            var createdUser = await _userManager.CreateAsync(newUser, model.Password);

            if (!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
                };
            }
            else 
            {
                return new AuthenticationResult
                {
                    Success = true
                };
            }


        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] {"User/password combination is wrong"}
                };
            }

            //token stuff come into picture
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            //pomocu ovo descriptora govorimo sta trebao da ima token koji vracamo useru 
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                //claims su neki properties u tokenu koji name govore ciji je token i neki stvari o odredjenom useru(stvari koje mi treba da znaom je njegov email, 
                //id, username, some permissions..) i dobra stvar oko ovoga je da znamo da je server to izdao zato sto mi to potpisujemo i koristimo
                //signing key  da proverimo da li je token validan pa mozemo da verujemo tome
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),  //unique ID for this JWT i to koristimo zbog token validacije
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }

        public async Task<string> CreateRole(string roleName)
        {
            try
            {
                IdentityRole role = new IdentityRole
                {
                    Name = roleName
                };

                IdentityResult result = await _roleManager.CreateAsync(role);

                return "Ok";
            }
            catch (Exception ex)
            {
                return "Error";
            }
        }

        public Response<UserModel> GetPerson(long personId)
        {
            var response = new Response<UserModel>();
            try
            {
                var p = _entity.Person.Find(personId);
                var person = new UserModel()
                {
                    FirstAndLastName = p.FirstName + " " + p.LastName,
                    ImageUrl = p.ImageUrl,
                };
                response.Value =person;
                
            }
            catch (Exception e)
            {

            }
            return response;
        }
    }
}
