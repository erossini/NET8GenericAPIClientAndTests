using PSC.CSharp.Library.APIClient;
using PSC.CSharp.Library.DemoAPIClient.Models.Person;
using PSC.CSharp.Library.DemoAPIClient.Models.Person.Responses;

namespace PSC.CSharp.Library.DemoAPIClient
{
    public interface IPersonService
    {
        Task<ApiResponse<UpdatePersonResponse>>? AddPerson(PersonModel person);
        Task<ApiResponse<PersonModel>>? GetPersonById(string id);
        Task<ApiResponse<UpdatePersonResponse>>? UpdatePerson(string id, PersonModel person);
    }
}