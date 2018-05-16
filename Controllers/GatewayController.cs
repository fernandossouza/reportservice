using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using reportservice.Service.Interface;

namespace reportservice.Controllers {
    [Route ("")]
    public class GatewayController : Controller {
        private IConfiguration _configuration;
        private IThingService _thingService;
        private IProductionOrderService _productionOrderService;
        private IRecipeService _recipeService;

        public GatewayController (IConfiguration configuration, IThingService thingService, IProductionOrderService productionOrderService, IRecipeService recipeService) {
            _configuration = configuration;
            _thingService = thingService;
            _productionOrderService = productionOrderService;
            _recipeService = recipeService;
        }

        [HttpGet ("gateway/things/")]
        [Produces ("application/json")]
        public async Task<IActionResult> GetThing ([FromQuery] int? startat, [FromQuery] int? quantity, [FromQuery] string fieldFilter = null, [FromQuery] string fieldValue = null, [FromQuery] string orderField = null, [FromQuery] string order = null) {

            var (thing, resultCode) = await _thingService.getThing (startat, quantity, fieldFilter, fieldValue, orderField, order);
            switch (resultCode) {
                case HttpStatusCode.OK:
                    return Ok (thing);
                case HttpStatusCode.NotFound:
                    return NotFound ();
            }
            return StatusCode (StatusCodes.Status500InternalServerError);
        }

        [HttpGet ("gateway/things/{{id:int}}")]
        [Produces ("application/json")]
        public async Task<IActionResult> GetThing (int id) {

            var (thing, resultCode) = await _thingService.getThing (thingId);
            switch (resultCode) {
                case HttpStatusCode.OK:
                    return Ok (thing);
                case HttpStatusCode.NotFound:
                    return NotFound ();
            }
            return StatusCode (StatusCodes.Status500InternalServerError);
        }

        [HttpGet ("gateway/productionorder/")]
        [Produces ("application/json")]
        public async Task<IActionResult> GetProductionOrder ([FromQuery] int? startat, [FromQuery] int? quantity, [FromQuery] string fieldFilter = null, [FromQuery] string fieldValue = null, [FromQuery] string orderField = null, [FromQuery] string order = null) {

            var (productionOrder, resultCode) = await _productionOrderService.getProductionOrder (startat, quantity, fieldFilter, fieldValue, orderField, order);
            switch (resultCode) {
                case HttpStatusCode.OK:
                    return Ok (productionOrder);
                case HttpStatusCode.NotFound:
                    return NotFound ();
            }
            return StatusCode (StatusCodes.Status500InternalServerError);
        }

        [HttpGet ("gateway/recipe/")]
        [Produces ("application/json")]
        public async Task<IActionResult> GetRecipe ([FromQuery] int? startat, [FromQuery] int? quantity, [FromQuery] string fieldFilter = null, [FromQuery] string fieldValue = null, [FromQuery] string orderField = null, [FromQuery] string order = null) {

            var (recipe, resultCode) = await _recipeService.getRecipe (startat, quantity, fieldFilter, fieldValue, orderField, order);
            switch (resultCode) {
                case HttpStatusCode.OK:
                    return Ok (recipe);
                case HttpStatusCode.NotFound:
                    return NotFound ();
            }
            return StatusCode (StatusCodes.Status500InternalServerError);
        }
    }
}