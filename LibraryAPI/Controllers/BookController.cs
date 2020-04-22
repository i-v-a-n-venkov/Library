using LibraryAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService bookService;

        public BookController(IBookService bookService)
        {
            this.bookService = bookService;
        }
        
        [HttpGet]
        public async Task<IEnumerable<BookVM>> GetBooks()
        {
            var result = await this.bookService.GetBooks();
            return result;
        } 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book  = await this.bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        } 

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]BookVM book)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var currentBook = await this.bookService.Add(book);
                return Ok(currentBook);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await this.bookService.Delete(id);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody]BookVM book)
        {
            try
            {
               await this.bookService.Update(id, book);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}