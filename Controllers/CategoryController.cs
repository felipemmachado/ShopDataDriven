using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Context;
using Shop.Models;

namespace Shop.Controllers
{

    [Route("categories")]
    public class CategoryController : ControllerBase
    {

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Category>>> Get([FromServices]DataContext context)
        {
            var categoires = await context.Categories.AsNoTracking().ToListAsync();
            return Ok(categoires);
        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<Category>> GetById(int id, [FromServices]DataContext context)
        {
            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            return Ok(category);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Category>> Post([FromBody]Category category, [FromServices]DataContext context)
        {
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try {
                context.Categories.Add(category);
                await context.SaveChangesAsync();
                return  Ok(category);
            } catch (Exception e) {
                return BadRequest(new { message = "Não foi possível salvar a categoria", error = e.Message });
            }
        }

        [HttpPut("{id:int}")]
        [Authorize]
        public async Task<ActionResult<Category>> Put(int id, [FromBody]Category category, [FromServices]DataContext context)
        {
            if(id != category.Id)
                return NotFound(new { messag = "Categoria não encontrada" });

            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            try {

                context.Entry<Category>(category).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return Ok(category);
            } catch (DbUpdateConcurrencyException e){
                return BadRequest(new { message = "Não foi possível atualizar esse registro agora", error = e.Message });
            } catch (Exception e){
                return BadRequest(new { message = "Não foi possível atualizar a categoria", error = e.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id, [FromServices]DataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(category == null)
                return NotFound(new { message = "Categoria não encontrada"});

            try {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok();

            } catch (Exception e){
                return BadRequest(new { message = "Não foi possível excluír a categoria", error = e.Message });
            }
        }

    }

}