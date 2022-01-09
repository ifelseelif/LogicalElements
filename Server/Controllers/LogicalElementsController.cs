using System;
using System.Linq;
using System.Security.Claims;
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

            var userId = GetIdentifier();
            return _logicalElementsService.AddElement(elemType, userId);
        }

        [HttpPut("elements")]
        public Task<string> SetValueForElement([FromQuery] string name, [FromQuery] bool value)
        {

            var userId = GetIdentifier();
            return _logicalElementsService.SetValueForElement(name, value, userId);
        }

        [HttpPost("io")]
        public Task<string> AddIO([FromQuery] bool isInput, [FromQuery] string name)
        {

            var userId = GetIdentifier();
            return _logicalElementsService.AddIO(isInput, name, userId);
        }

        [HttpPost("connection")]
        public Task<string> AddConnection([FromQuery] int idOfInput, [FromQuery] int idOfOutput)
        {

            var userId = GetIdentifier();
            return _logicalElementsService.AddConnection(idOfInput, idOfOutput, userId);
        }

        [HttpGet("{id}")]
        public Task<string> Show([FromRoute] int id)
        {

            var userId = GetIdentifier();
            return _logicalElementsService.Show(id, userId);
        }

        [HttpGet("result")]
        public Task<string> Print()
        {
            var userId = GetIdentifier();
            return _logicalElementsService.Print(GetIdentifier());
        }

        private Guid GetIdentifier()
        {
            var id = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
            Guid.TryParse(id, out var userId);
            return userId;
        }
    }
}