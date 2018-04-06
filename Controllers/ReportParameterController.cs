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
        private readonly IReportParameterServices _reportParameterService;

        public ReportParameterController (IReportParameterServices reportParameterService)
        {
            _reportParameterService = reportParameterService;
        }
        
        [HttpGet("ProductionOrder/{productionOrderId}")]
        public async Task<IActionResult> GetProductionOrder(int productionOrderId, [FromQuery]int thingId)
        {
            try
            {
                Report report;
                string erro;
                (report,erro) = await _reportParameterService.GetReportPerProductionOrderId(productionOrderId,thingId);

                if(report == null)
                    return StatusCode(500,erro);

                return Ok(report);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("RecipeCode/{recipeCode}")]
        public async Task<IActionResult> GetRecipeCode(string recipeCode, [FromQuery]int thingId,long startDate, long endDate)
        {
            try
            {
                Report report;
                string erro;
                (report,erro) = await _reportParameterService.GetReportPerRecipeCode(recipeCode,thingId,startDate,endDate);

                if(report == null)
                    return StatusCode(500,erro);

                return Ok(report);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("Date")]
        public async Task<IActionResult> GetDate([FromQuery]int thingId,long startDate, long endDate)
        {
            try
            {
                Report report;
                string erro;
                (report,erro) = await _reportParameterService.GetReportPerDate(thingId,startDate,endDate);

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