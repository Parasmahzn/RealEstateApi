using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Data;
using System.Security.Claims;

namespace RealEstateApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookmarksController : ControllerBase
    {
        private readonly DBContext _dBContext;
        public BookmarksController()
        {
            _dBContext = new();
        }

        [HttpGet]
        [Route("GetBookMarks")]
        public async Task<IActionResult> GetBookMarks()
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dBContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return Ok(new CommonResponse()
            {
                Message = Message.Failed.ToString(),
                Errors = new List<Errors>()
                {
                    new Errors()
                    {
                        ErrorCode = ErrorCodes.NotFound
                        , ErrorMessage = ErrorCodes.NotFound.ToString()
                    }
                }
            });

            var bookmarks = from b in _dBContext.Bookmarks.Where(b => b.User_Id == user.Id)
                            join p in _dBContext.Properties on b.PropertyId equals p.Id
                            where b.Status == true
                            select new
                            {
                                b.Id,
                                p.Name,
                                p.Price,
                                p.ImageUrl,
                                p.Address,
                                b.Status,
                                b.User_Id,
                                b.PropertyId

                            };
            return await Task.FromResult(Ok(bookmarks));
        }

        [HttpPost]
        [Route("AddBookmark")]
        public async Task<IActionResult> AddBookmark([FromBody] Bookmark bookmarkitem)
        {
            bookmarkitem.Status = true;
            _dBContext.Bookmarks.Add(bookmarkitem);
            _dBContext.SaveChanges();
            return await Task.FromResult(Ok("Bookmark added"));
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public async Task<IActionResult> DeleteBookmark(int id)
        {
            var bookmarkResult = _dBContext.Bookmarks.FirstOrDefault(b => b.Id == id);
            if (bookmarkResult == null)
            {
                return NotFound();
            }
            else
            {
                var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                var user = _dBContext.Users.FirstOrDefault(u => u.Email == userEmail);
                if (user == null) return NotFound();
                if (bookmarkResult.User_Id == user.Id)
                {
                    _dBContext.Bookmarks.Remove(bookmarkResult);
                    await _dBContext.SaveChangesAsync();
                    return Ok("Bookmark deleted successfully");
                }
                return BadRequest();
            }
        }
    }
}
