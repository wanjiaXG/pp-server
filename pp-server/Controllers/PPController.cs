using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PerformanceCalculator.Simulate;

namespace pp_server.Controllers
{
    [ApiController]
    public class PPController : ControllerBase
    {
        private readonly ILogger<PPController> _logger;

        public PPController(ILogger<PPController> logger)
        {
            _logger = logger;
        }

        [HttpGet("/")]
        public IEnumerable<PPModel> index(int b, int m, string mods)
        {
            OsuSimulateCommand command = new OsuSimulateCommand();

            _logger.LogInformation("???????????????",null);

            List<PPModel> p = new List<PPModel>();
            p.Add(new PPModel() { BeatmapId = b });
            return p;

        }
    }
}
