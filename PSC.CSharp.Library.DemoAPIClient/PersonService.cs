using Microsoft.Extensions.Logging;
using PSC.CSharp.Library.APIClient;
using PSC.CSharp.Library.DemoAPIClient.Models.Person;
using PSC.CSharp.Library.DemoAPIClient.Models.Person.Responses;

namespace PSC.CSharp.Library.DemoAPIClient
{
    public class PersonService : ApiService, IPersonService
    {
        public PersonService(HttpClient clientFactory, ILogger logger) :
            base("/people", clientFactory, logger)
        {
        }

        public PersonService(string apiKey, HttpClient clientFactory, ILogger logger) :
            base("/people", apiKey, clientFactory, logger)
        {
        }

        public async Task<ApiResponse<PersonModel>>? GetPersonById(string id)
        {
            return await Get<PersonModel>($"{id}", null, null);
        }

        public async Task<ApiResponse<UpdatePersonResponse>>? AddPerson(PersonModel person)
        {
            return await Post<PersonModel, UpdatePersonResponse>(person);
        }

        public async Task<ApiResponse<UpdatePersonResponse>>? UpdatePerson(string id, PersonModel person)
        {
            return await Put<PersonModel, UpdatePersonResponse>(person, $"/{id}");
        }
    }
}
