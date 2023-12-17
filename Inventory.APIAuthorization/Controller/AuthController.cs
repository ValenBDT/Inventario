using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Inventory.APIAuthorization.Services;
using Inventory.APIAuthorization.Services.Interfaces;
using Inventory.DTOs.Auth;
using Inventory.Entities;
using Inventory.Persistence.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.APIAuthorization.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        private readonly ITokenService _tokenService;
        public AuthController(IAuthRepository authRepository, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _authRepository = authRepository;
        }   

        [HttpPost("register")]

        public async Task<IActionResult> Register(UserToRegisterDTO userToRegisterDTO){
            if(await _authRepository.UserExist(userToRegisterDTO.Email.ToLower())) return BadRequest("Ya existe un usuario registrado con ese correo");
            var userToCreate = new User
            (
                userToRegisterDTO.Email.ToLower(),
                userToRegisterDTO.Name
            );


            var userCreated = await _authRepository.Register(userToCreate, userToRegisterDTO.Password);

            UserToListDTO userToReturn = new (userCreated.Id, userCreated.Email, userCreated.Name, "");
            return Ok(userToReturn);
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(UserToLoginDTO userToLoginDTO){
            var userFromRepo = await _authRepository.Login(userToLoginDTO.Email.ToLower(), userToLoginDTO.Password);
            if(userFromRepo is null) return Unauthorized();
            var token = _tokenService.CreateToken(userFromRepo);
            UserToListDTO userToReturn = new (userFromRepo.Id, userFromRepo.Email, userFromRepo.Name, token);
            return Ok(userToReturn);
        }
    }
}