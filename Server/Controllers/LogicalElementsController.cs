using Common;
using Microsoft.AspNetCore.Mvc;
using Server.Services;

namespace Server.Controllers
{
    [ApiController]
    [Route("/logicalelements")]
    public class LogicalElementsController : ControllerBase
    {
        private readonly ILogicalElementsService _logicalElementsService;

        public LogicalElementsController(ILogicalElementsService logicalElementsService)
        {
            _logicalElementsService = logicalElementsService;
        }

        [HttpPost("elements")]
        public string AddElement([FromQuery] ElemType elemType)
        {
            return _logicalElementsService.AddElement(elemType);
        }

        [HttpPut("elements")]
        public string SetValueForElement([FromQuery] string name, [FromQuery] bool value)
        {
            return _logicalElementsService.SetValueForElement(name, value);
        }

        [HttpPost("io")]
        public string AddIO([FromQuery] bool isInput, [FromQuery] string name)
        {
            return _logicalElementsService.AddIO(isInput, name);
        }

        [HttpPost("connection")]
        public string AddConnection([FromQuery] int idOfInput, [FromQuery] int idOfOutput)
        {
            return _logicalElementsService.AddConnection(idOfInput, idOfOutput);
        }

        [HttpGet("{id}")]
        public string Show([FromRoute] int id)
        {
            return _logicalElementsService.Show(id);
        }

        [HttpGet("result")]
        public string Print()
        {
            return _logicalElementsService.Print();
        }
    }
}