using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using BlendedAPI.DTOs;
using BlendedAPI.Models;
using BlendedAPI.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BlendedAPI.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly IConfiguration _configuration;

        public AuthController(IAuthRepository authRepository, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            userRegisterDto.Username = userRegisterDto.Username.ToLower();

            if (await _authRepository.UserExists(userRegisterDto.Username))
                return BadRequest("Username already exists");

            var user = new User { Username = userRegisterDto.Username };
            var newUser = await _authRepository.Register(user, userRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserRegisterDto userRegisterDto)
        {
            var user = await _authRepository.Login(userRegisterDto.Username.ToLower(), userRegisterDto.Password);

            if (user == null) 
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };

            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            var Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = Credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new { token = tokenHandler.WriteToken(token) });
        }
    }
}
