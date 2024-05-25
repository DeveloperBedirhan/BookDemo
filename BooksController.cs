using bookDemo.Data;
using bookDemo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.JsonPatch;

namespace bookDemo.Controllers
{
    [Route("api/books")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllBooks()
        {
            var books = ApplicationContext.Books;
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = ApplicationContext.Books.Where(b => b.Id == id)
                .SingleOrDefault();

            if (book == null)
                return NotFound(); //404

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book bookPost)
        {
            try
            {
                if (bookPost is null)
                    return BadRequest(); //400

                ApplicationContext.Books.Add(bookPost);
                return StatusCode(201, bookPost);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id,
            [FromBody] Book bookPut)
        {
            //check book?
            var entity = ApplicationContext.Books.Find(b => b.Id == id);

            if (entity == null)
                return NotFound();

            //check id
            if (id != bookPut.Id)
                return BadRequest();

            ApplicationContext.Books.Remove(entity);
            bookPut.Id = entity.Id;
            ApplicationContext.Books.Add(bookPut);
            return Ok(bookPut);
        }

        [HttpDelete]
        public IActionResult DeleteAllBooks()
        {
            ApplicationContext.Books.Clear();
            return NoContent(); //204
        }


        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute(Name = "id")] int id)
        {
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));

            if (entity == null)
                return NotFound(new
                {
                    statusCode = 404,
                    message = $"Book with id:{id} could not found."
                });

            ApplicationContext.Books.Remove(entity);
            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name ="id")] int id, 
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            //check entity
            var entity = ApplicationContext.Books.Find(b => b.Id.Equals(id));
            if (entity == null)
                return NotFound(); //404

            bookPatch.ApplyTo(entity);
            return NoContent(); //204
        }
    }
}
