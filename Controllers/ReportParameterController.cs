using System;
using Microsoft.AspNetCore.Mvc;
using reportservice.Service.Interface;
using reportservice.Model;
using System.Threading.Tasks;

namespace reportservice.Controllers
{
    [Route("api/[controller]")]
    public class ReportParameterController : Controller
    {
        private readonly IReportOpService _reportOpService;

        public ReportController (IReportOpService reportOpService)
        {
            _reportOpService = reportOpService;
        }
        
         public async Task<IActionResult> Get([FromQuery]int productionOrderId, [FromQuery]int thingId)
        {
            try
            {
                Report report;
                string erro;
                (report,erro) = await _reportOpService.GetReport(productionOrderId,thingId);

                if(report == null)
                    return StatusCode(500,erro);

                return Ok(report);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

    }
}