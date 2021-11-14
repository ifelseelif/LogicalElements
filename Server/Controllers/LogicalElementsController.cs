using Common;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("/logicalelements")]
    public class LogicalElementsController : Controller
    {
        private readonly ILogicalElementsService _logicalElementsService;

        public LogicalElementsController(ILogicalElementsService logicalElementsService)
        {
            _logicalElementsService = logicalElementsService;
        }

        [HttpPost("elements")]
        public string AddElement([FromQuery] ElemType elemType)
        {
            return _logicalElementsService.AddElement(elemType, HttpContext.Connection.RemoteIpAddress?.ToString());
        }

        [HttpPut("elements")]
        public string SetValueForElement([FromQuery] string name, [FromQuery] bool value)
        {
            return _logicalElementsService.SetValueForElement(name, value,
                HttpContext.Connection.RemoteIpAddress?.ToString());
        }

        [HttpPost("io")]
        public string AddIO([FromQuery] bool isInput, [FromQuery] string name)
        {
            return _logicalElementsService.AddIO(isInput, name, HttpContext.Session.Id);
        }

        [HttpPost("connection")]
        public string AddConnection([FromQuery] int idOfInput, [FromQuery] int idOfOutput)
        {
            return _logicalElementsService.AddConnection(idOfInput, idOfOutput,
                HttpContext.Connection.RemoteIpAddress?.ToString());
        }

        [HttpGet("{id}")]
        public string Show([FromRoute] int id)
        {
            return _logicalElementsService.Show(id, HttpContext.Connection.RemoteIpAddress?.ToString());
        }

        [HttpGet("result")]
        public string Print()
        {
            return _logicalElementsService.Print(HttpContext.Connection.RemoteIpAddress?.ToString());
        }
    }
}