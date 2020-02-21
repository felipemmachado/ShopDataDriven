using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Context;
using Shop.Models;
using Shop.Services;

namespace Shop.Controllers
{
    [Route("users")]
    public class UserController : ControllerBase 
    {

        [HttpGet]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<List<User>>> Get([FromServices] DataContext context)
        {    
            var users = await context.Users.AsNoTracking().ToListAsync();
            return users;
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "manager")]
        public async Task<ActionResult<User>> Put(
            int id,
            [FromBody] User user, 
            [FromServices] DataContext context)
        {

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            if(id != user.Id)
                return NotFound(new { message = "Usuário não encontrado"});

            try 
            {
                context.Entry(user).State =  EntityState.Modified;
                await context.SaveChangesAsync();
                return user;
            } 
            catch(Exception ex)
            {
                return BadRequest( 
                    new { 
                        message = "Não foi possível atualizar o usuário", 
                        error = ex.Message
                    }
                );
            }
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Post(
            [FromBody] User user, 
            [FromServices] DataContext context)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try {

                context.Users.Add(user);
                await context.SaveChangesAsync();
                return user;

            } 
            catch (Exception ex)
            {
                return BadRequest(
                    new { 
                        message = "Não foi possível salvar o usuario", 
                        error = ex.Message
                    }
                );
            }
        }

        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<dynamic>> Authenticate(
            [FromBody] User user,
            [FromServices] DataContext context)
        {
            var userLogin = await context
                .Users
                .AsNoTracking()
                .Where(x => x.Username == user.Username && x.Password == user.Password)
                .FirstOrDefaultAsync();

            if(userLogin == null)
                return NotFound(new { message = "Usuário ou senha inválido"});

            var token = TokenService.GenerateToken(userLogin);

            return token;
        }
    }
}