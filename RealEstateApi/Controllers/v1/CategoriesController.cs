using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Data;

namespace RealEstateApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly DBContext _dBContext;
        public CategoriesController()
        {
            _dBContext = new DBContext();
        }

        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategoriesList()
        {
            return await Task.FromResult(Ok(_dBContext.Categories));
        }
    }
}
