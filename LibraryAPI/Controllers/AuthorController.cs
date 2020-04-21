using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LibraryAPI.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService authorService;

        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpGet]
        public IEnumerable<AuthorVM> GetAuthors()
        {
            return this.authorService.GetAuthors();
        }

        [HttpGet("{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var author = this.authorService.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpPost]
        public IActionResult Post([FromBody]AuthorVM author)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentAuthor = this.authorService.Add(author);
            return Ok(currentAuthor);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                this.authorService.Delete(id);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]AuthorVM author)
        {
            try
            {
                this.authorService.Update(id, author);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}