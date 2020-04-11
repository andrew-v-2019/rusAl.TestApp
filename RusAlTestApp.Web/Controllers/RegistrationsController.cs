using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RusAlTestApp.Services;
using RusAlTestApp.ViewModels;

namespace RusAlTestApp.Web.Controllers
{
    [Route("api/[controller]")]
    public class RegistrationsController : Controller
    {
        private readonly IRegistrationsService _registrationsService;

        public RegistrationsController(IRegistrationsService registrationsService)
        {
            _registrationsService = registrationsService;
        }

        [HttpGet]
        [Route("selectBoxValues")]
        public async Task<RegistrationPageData> GetSelectBoxValuesAsync()
        {
            var model = await _registrationsService.GetRegistrationPageDataAsync();
            return model;
        }

        [Authorize(Roles = "admin")]
        [HttpGet]
        public async Task<IEnumerable<RegistrationViewModel>> Get(FilterViewModel filter)
        {
            var result = await _registrationsService.GetByFilter(filter);
            return result;
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync([FromBody] RegistrationViewModel registrationViewModel)
        {
            await _registrationsService.CreateRegistration(registrationViewModel);
            return Ok();
        }
    }
}
