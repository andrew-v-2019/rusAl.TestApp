using System.Collections.Generic;
using System.Threading.Tasks;
using RusAlTestApp.ViewModels;


namespace RusAlTestApp.Services
{
    public interface IRegistrationsService
    {
        Task<RegistrationPageData> GetRegistrationPageDataAsync();
        Task<IEnumerable<RegistrationViewModel>> GetByFilter(FilterViewModel filter);
        Task CreateRegistration(RegistrationViewModel registrationViewModel);
    }
}
