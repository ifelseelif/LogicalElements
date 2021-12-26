using System;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.Services.Interfaces;

namespace Server.Controllers
{
    [ApiController]
    [Authorize]
    [Route("/logicalelements")]
    public class LogicalElementsController : Controller
    {
        private readonly ILogicalElementsService _logicalElementsService;

        public LogicalElementsController(ILogicalElementsService logicalElementsService)
        {
            _logicalElementsService = logicalElementsService;
        }

        [HttpPost("elements")]
        public Task<string> AddElement([FromQuery] ElemType elemType)
        {
            Guid.TryParse(User.Identity?.Name, out var id);
            return _logicalElementsService.AddElement(elemType, id);
        }

        [HttpPut("elements")]
        public Task<string> SetValueForElement([FromQuery] string name, [FromQuery] bool value)
        {
            Guid.TryParse(User.Identity?.Name, out var id);
            return _logicalElementsService.SetValueForElement(name, value, id);
        }

        [HttpPost("io")]
        public Task<string> AddIO([FromQuery] bool isInput, [FromQuery] string name)
        {
            Guid.TryParse(User.Identity?.Name, out var id);
            return _logicalElementsService.AddIO(isInput, name, id);
        }

        [HttpPost("connection")]
        public Task<string> AddConnection([FromQuery] int idOfInput, [FromQuery] int idOfOutput)
        {
            Guid.TryParse(User.Identity?.Name, out var id);
            return _logicalElementsService.AddConnection(idOfInput, idOfOutput
                , id);
        }

        [HttpGet("{id}")]
        public Task<string> Show([FromRoute] int id)
        {
            Guid.TryParse(User.Identity?.Name, out var userId);
            return _logicalElementsService.Show(id, userId);
        }

        [HttpGet("result")]
        public Task<string> Print()
        {
            Guid.TryParse(User.Identity?.Name, out var id);
            return _logicalElementsService.Print(id);
        }
    }
}