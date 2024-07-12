using System;
using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

	[Authorize]

	public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController
	{

		[HttpGet]
		public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
		{
			var users = await userRepository.GetMembersAsync();
			return Ok(users);
		}


		[HttpGet("{username}")]
		public async Task<ActionResult<MemberDto>> GetUsers(string username)
		{

			var user = await userRepository.GetMemberAsync(username);

			if (user == null) return NotFound();

			return user;
		}

		[HttpPut]
		public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
		{

			var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

			if (user == null) return BadRequest("Could not find user");

			mapper.Map(memberUpdateDto, user);

			if (await userRepository.SaveAllAsync()) return NoContent();

			return BadRequest("Faild to update user");
		}

		[HttpPost("add-photo")]
		public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
		{
			var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
			if (user == null) return BadRequest("Cannot update user");
			var result = await photoService.AddPhotoAsync(file);
			if (result.Error != null) return BadRequest(result.Error.Message);

			var photo = new Photo
			{
				Url = result.SecureUrl.AbsoluteUri,
				PublicId = result.PublicId
			};
			user.Photos.Add(photo);
			if (await userRepository.SaveAllAsync()) return mapper.Map<PhotoDto>(photo);

			return BadRequest("Problem adding photo");
		}

	}
}

