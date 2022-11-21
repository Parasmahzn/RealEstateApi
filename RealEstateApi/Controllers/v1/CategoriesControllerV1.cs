using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RealEstateApi.Data;

namespace RealEstateApi.Controllers.v1
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesControllerV1 : ControllerBase
    {
        private readonly DBContext _dBContext;
        public CategoriesControllerV1()
        {
            _dBContext = new DBContext();
        }

        [HttpGet]
        [Route("Categories")]
        public async Task<IActionResult> GetCategoriesList()
        {
            return await Task.FromResult(Ok(_dBContext.Categories));
        }
    }
}
