using System.Threading.Tasks;
using Circa.Actors.Application;
using Circa.Actors.Domain.Dtos;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using OpenTracing.Tag;
using Solari.Deimos;
using Solari.Vanth;

namespace Circa.Actors.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonApplication _application;
        private readonly IDeimosTracer _tracer;

        public PersonController(IPersonApplication application, IDeimosTracer tracer)
        {
            _application = application;
            _tracer = tracer;
        }

        [HttpPost]
        public async Task<IActionResult> Add(InsertPersonDto dto)
        {
            using (DeimosTracingDescriptor scope = _tracer.OpenScopeFromContext("inserting-person", HttpContext.Request.Headers))
            {
                scope.Enrich.Tag(Tags.HttpMethod, "POST");
                CommonResponse<string> rs = await _application.Insert(dto);
                if (rs.HasErrors)
                {
                    return BadRequest(rs);
                }
                return Ok(rs);    
            }
        }
    }
}