using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RealEstateApi.Data;
using System.Security.Claims;

namespace RealEstateApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PropertiesControllerV1 : ControllerBase
    {
        private readonly DBContext _dBContext;
        public PropertiesControllerV1()
        {
            _dBContext = new();
        }

        [HttpGet]
        [Route("Properties/{categoryId}")]
        public async Task<IActionResult> GetPropertiesList(int categoryId)
        {
            var propertiesResult = _dBContext.Properties.Where(c => c.CategoryId == categoryId);
            if (propertiesResult == null)
            {
                return Ok(new CommonResponse()
                {
                    Errors = new List<Errors>() { new Errors() { ErrorCode = ErrorCodes.NotFound, ErrorMessage = ErrorCodes.NotFound.ToString() } }
                });
            }
            return await Task.FromResult(Ok(new CommonResponse() { Data = propertiesResult, Success = true, Message = Message.Success.ToString() }));


        }

        [HttpGet]
        [Route("PropertyDetail/{propertyId}")]
        public async Task<IActionResult> GetPropertyDetail(int propertyId)
        {
            var userEmail = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = _dBContext.Users.FirstOrDefault(u => u.Email == userEmail);

            if (user == null) return NotFound();

            var propertyResult = _dBContext.Properties.Find(propertyId);

            if (propertyResult != null)
            {
                var result = _dBContext.Properties
                 .Include(p => p.Bookmarks).Where(p => p.Id == propertyId)
                 .Select(p => new
                 {
                     p.Id,
                     p.Name,
                     p.Detail,
                     p.Address,
                     p.Price,
                     p.ImageUrl,
                     p.User.Phone,
                     Bookmark = p.Bookmarks.FirstOrDefault(u => u.User_Id == user.Id)
                 }).FirstOrDefault();
                return await Task.FromResult(Ok(new CommonResponse() { Data = result, Success = true, Message = Message.Success.ToString() }));
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet]
        [Route("TrendingProperties")]
        public async Task<IActionResult> GetTrendingProperties()
        {
            var propertiesResult = _dBContext.Properties.Where(c => c.IsTrending == true);
            if (propertiesResult == null)
            {
                return NotFound();
            }
            return await Task.FromResult(Ok(propertiesResult));
        }

        [HttpGet]
        [Route("SearchProperties/{address}")]

        public async Task<IActionResult> SearchProperties(string address)
        {
            var propertiesResult = _dBContext.Properties.Where(p => p.Address.Contains(address));
            if (propertiesResult == null)
            {
                return NotFound();
            }
            return await Task.FromResult(Ok(propertiesResult));
        }
    }
}
