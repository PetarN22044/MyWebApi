using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyWebApi.Data;
using MyWebApi.Dtos.User;

namespace MyWebApi.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        public AuthController(IAuthRepository authRepo){
              
              _authRepo = authRepo;
        }
        
        [HttpPost("Register")]
        public async Task<ActionResult<ServiceResponce<int>>> Register (UserRegisterDto request){
             var response = await _authRepo.Register(
                new User {UserName = request.UserName},request.Password
             );

             if(!response.Success){
                return BadRequest(response);
             }
                return Ok(response);
            
        }
         [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponce<int>>>Login (UserLoginDto request){
             var response = await _authRepo.Login(
                request.UserName,request.Password
             );

             if(!response.Success){
                return BadRequest(response);
             }
                return Ok(response);
            
        }

        
        
    }
}//.