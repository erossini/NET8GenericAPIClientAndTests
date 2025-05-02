using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PSC.CSharp.Library.DemoAPIClient.Models.Person.Responses
{
    public class CreatePersonResponse
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }
    }
}
