using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Compute.Domain.Models;
using Compute.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Compute.Application.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ComputeController : ControllerBase
    {
        private readonly IOperationRepository _operationRepository;

        public ComputeController(IOperationRepository operationRepository)
        {
            _operationRepository = operationRepository;
        }
        // GET api/v1/compute/add/7/8
        [HttpGet("add/{x}/{y}")]
        public ActionResult<double> Add(double x, double y)
        {
            return Ok();
        }

        // GET api/v1/compute/sub/7/8
        [HttpGet("sub/{x}/{y}")]
        public ActionResult<double> Sub(double x, double y)
        {
            var result = x - y;
            return result;
        }

        // GET api/v1/compute/mul/2/5
        [HttpGet("mul/{x}/{y}")]
        public ActionResult<double> Mul(double x, double y)
        {
            var result = x * y;
            return result;
        }

        // GET api/v1/compute/div/10/5
        [HttpGet("div/{x}/{y}")]
        public ActionResult<double> Div(double x, double y)
        {
            var result = x / y;
            return result;
        }
    }
}
