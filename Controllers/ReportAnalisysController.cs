using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using reportservice.Model;
using reportservice.Service.Interface;

namespace reportservice.Controllers
{
    [Route ("api/[controller]")]
    public class ReportAnalisysController : Controller
    {
        private readonly IReportAnalysisService _reportAnalysisService;

        public ReportAnalisysController (IReportAnalysisService reportAnalysisService) 
        {
            _reportAnalysisService = reportAnalysisService;
        }

        [HttpGet("ProductionOrder/{productionOrderId}")]
        public async Task<IActionResult> GetProductionOrder(int productionOrderId)
        {
            try
            {
                ReportQuality report;
                string erro;
                (report,erro) = await _reportAnalysisService.GetReportQualityPerProductionOrderId(productionOrderId);

                if(report == null)
                {
                    if(erro.ToLower().IndexOf("not found")>=0)
                        return NotFound();
                    else
                        return StatusCode(500,erro);
                }

                return Ok(report);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("RecipeCode/{recipeCode}")]
        public async Task<IActionResult> GetRecipeCode(string recipeCode,[FromQuery] long startDate,[FromQuery] long endDate)
        {
            try
            {
                if(endDate<=0)
                    endDate = DateTime.Now.Ticks;

                ReportQuality report;
                string erro;
                (report,erro) = await _reportAnalysisService.GetReportQualityPerRecipeCodeAndDate(startDate,endDate,recipeCode);

                if(report == null)
                {
                    if(erro.ToLower().IndexOf("not found")>=0)
                        return NotFound();
                    else
                        return StatusCode(500,erro);
                }

                return Ok(report);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpGet("Date")]
        public async Task<IActionResult> GetDate([FromQuery] long startDate,[FromQuery] long endDate)
        {
            try
            {
                if(endDate<=0)
                    endDate = DateTime.Now.Ticks;

                ReportQuality report;
                string erro;
                (report,erro) = await _reportAnalysisService.GetReportQualityPerRecipeCodeAndDate(startDate,endDate);

                if(report == null)
                {
                    if(erro.ToLower().IndexOf("not found")>=0)
                        return NotFound();
                    else
                        return StatusCode(500,erro);
                }

                return Ok(report);
            }
            catch(Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        
    }
}