using Data.Models;
using LibraryAPI.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;

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
        public IEnumerable<BookVM> GetBooks()
        { 
            var result = this.bookService.GetBooks().ToList();
            return result;
        } 

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book  = this.bookService.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(book);
        } 

        [HttpPost]
        public IActionResult Post([FromBody]BookVM book) // Models -> book
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentBook = this.bookService.Add(book);
            return Ok(currentBook);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                this.bookService.Delete(id);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody]BookVM book)
        {
            try
            {
                this.bookService.Update(id, book);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}