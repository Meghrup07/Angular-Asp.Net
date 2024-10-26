using System;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
	public class AccountController(DataContext _context, ITokenService _tokenService, IMapper mapper) : BaseApiController
	{
		[HttpPost("register")]
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			if (await UserExixts(registerDto.Username)) return BadRequest("Username is taken");

			using var hmac = new HMACSHA512();

			var user = mapper.Map<AppUser>(registerDto);

			user.UserName = registerDto.Username.ToLower();
			user.passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
			user.passwordSalt = hmac.Key;

			_context.Users.Add(user);
			await _context.SaveChangesAsync();

			return new UserDto
			{
				Username = user.UserName,
				Token = _tokenService.CreateToken(user),
				KnownAs = user.KnownAs,
				Gender = user.Gender
			};

		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await _context.Users
			.Include(p => p.Photos)
			.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());

			if (user == null) return Unauthorized("invalid username");

			using var hmac = new HMACSHA512(user.passwordSalt);

			var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

			for (int i = 0; i < computedHash.Length; i++)
			{
				if (computedHash[i] != user.passwordHash[i]) return Unauthorized("Invalid password");
			}

			return new UserDto
			{
				Username = user.UserName,
				Token = _tokenService.CreateToken(user),
				PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
				KnownAs = user.KnownAs,
				Gender = user.Gender
			};
		}

		private async Task<bool> UserExixts(string username)
		{
			return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
		}

	}
}