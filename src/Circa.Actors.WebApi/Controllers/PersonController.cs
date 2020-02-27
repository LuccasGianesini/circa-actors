using System.Threading.Tasks;
using Circa.Actors.Application;
using Circa.Actors.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Solari.Vanth;

namespace Circa.Actors.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonApplication _application;

        public PersonController(IPersonApplication application) { _application = application; }

        [HttpPost]
        public async Task<IActionResult> Add(InsertPersonDto dto)
        {
            CommonResponse<ObjectId> rs = await _application.Insert(dto);
            if (rs.HasErrors)
            {
                return BadRequest(rs);
            }
            return Ok(rs.Result);
        }
    }
}