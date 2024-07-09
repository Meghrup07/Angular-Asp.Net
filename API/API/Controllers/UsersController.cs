using System;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

	[Authorize]

	public class UsersController(IUserRepository userRepository) : BaseApiController
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

	}
}

