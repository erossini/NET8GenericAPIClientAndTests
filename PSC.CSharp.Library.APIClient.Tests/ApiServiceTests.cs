using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using PSC.CSharp.Library.DemoAPIClient;
using PSC.CSharp.Library.DemoAPIClient.Models.Person;
using PSC.CSharp.Library.DemoAPIClient.Models.Person.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace PSC.CSharp.Library.APIClient.Tests
{
    [TestClass()]
    public class ApiServiceTests
    {
        [TestMethod]
        public async Task Get_PersonById_Test_Valid()
        {
            // set variables for collecting the logs
            List<LogLevel> logLevels = new List<LogLevel>();
            List<string> logMessages = new List<string>();

            // set logger mock
            var loggerMock = new Mock<ILogger<PersonService>>();
            loggerMock.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var state = invocation.Arguments[2];
                    var exception = (Exception)invocation.Arguments[3];
                    var formatter = invocation.Arguments[4];

                    var invokeMethod = formatter.GetType().GetMethod("Invoke");
                    var logMessage = (string)invokeMethod?.Invoke(formatter, new[] { state, exception });

                    logLevels.Add(logLevel);
                    logMessages.Add(logMessage);
                }));

            // define the mock answer from the API
            var responseModel = new PersonModel()
            {
                FirstName = "Enrico",
                LastName = "Rossini"
            };

            // convert the mock answer and encode it
            string json = JsonSerializer.Serialize(responseModel);
            var contentResponse = new StringContent(json, Encoding.UTF8, "application/json");

            // define the response from the API
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Content = contentResponse
            };

            // set the HttpMessageHandler from the API
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            // create an instance of HttpClient with the logger mock
            var httpClient = new HttpClient(mockHandler.Object);
            httpClient.BaseAddress = new Uri("https://myurl");
            var client = new PersonService("test", httpClient, loggerMock.Object);

            // call the API implementation
            // the return is of ApiReponse<T>
            var actual = await client.GetPersonById("1");

            // validate the answer
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Success);

            // validate the HttpRequestMessage method response
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
                ItExpr.IsAny<CancellationToken>());

            // validate the logs
            Assert.IsTrue(logLevels.Contains(LogLevel.Information));
            Assert.IsTrue(!logLevels.Contains(LogLevel.Error));
            Assert.IsTrue(logMessages.Count() == 1);
            Assert.IsTrue(logMessages[0] == "[DBG][HttpVerb: GET][URL: /people/1][HttpCode: 200]");
        }

        [TestMethod]
        public async Task Post_Person_Test_Valid()
        {
            // set variables for collecting the logs
            List<LogLevel> logLevels = new List<LogLevel>();
            List<string> logMessages = new List<string>();

            // set logger mock
            var loggerMock = new Mock<ILogger<PersonService>>();
            loggerMock.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var state = invocation.Arguments[2];
                    var exception = (Exception)invocation.Arguments[3];
                    var formatter = invocation.Arguments[4];

                    var invokeMethod = formatter.GetType().GetMethod("Invoke");
                    var logMessage = (string)invokeMethod?.Invoke(formatter, new[] { state, exception });

                    logLevels.Add(logLevel);
                    logMessages.Add(logMessage);
                }));

            // define the mock answer from the API
            var responseModel = new CreatePersonResponse()
            {
                Id = 1
            };

            // convert the mock answer and encode it
            string json = JsonSerializer.Serialize(responseModel);
            var contentResponse = new StringContent(json, Encoding.UTF8, "application/json");

            // define the response from the API
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.Created,
                Content = contentResponse
            };

            // set the HttpMessageHandler from the API
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            // create an instance of HttpClient with the logger mock
            var httpClient = new HttpClient(mockHandler.Object);
            httpClient.BaseAddress = new Uri("https://myurl");
            var client = new PersonService("test", httpClient, loggerMock.Object);

            // call the API implementation
            // the return is of ApiReponse<T>
            var actual = await client.AddPerson(new PersonModel() {
                FirstName = "Enrico", LastName = "Rossini"
            });

            // validate the answer
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Success);

            // validate the HttpRequestMessage method response
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>());

            // validate the logs
            Assert.IsTrue(logLevels.Contains(LogLevel.Information));
            Assert.IsTrue(!logLevels.Contains(LogLevel.Error));
            Assert.IsTrue(logMessages.Count() == 1);
            Assert.IsTrue(logMessages[0] == "[DBG][HttpVerb: POST][URL: /people][HttpCode: 201]");
        }

        [TestMethod]
        public async Task Put_Person_Test_Valid()
        {
            // set variables for collecting the logs
            List<LogLevel> logLevels = new List<LogLevel>();
            List<string> logMessages = new List<string>();

            // set logger mock
            var loggerMock = new Mock<ILogger<PersonService>>();
            loggerMock.Setup(x => x.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<It.IsAnyType>(),
                    It.IsAny<Exception>(),
                    (Func<It.IsAnyType, Exception, string>)It.IsAny<object>()))
                .Callback(new InvocationAction(invocation =>
                {
                    var logLevel = (LogLevel)invocation.Arguments[0];
                    var eventId = (EventId)invocation.Arguments[1];
                    var state = invocation.Arguments[2];
                    var exception = (Exception)invocation.Arguments[3];
                    var formatter = invocation.Arguments[4];

                    var invokeMethod = formatter.GetType().GetMethod("Invoke");
                    var logMessage = (string)invokeMethod?.Invoke(formatter, new[] { state, exception });

                    logLevels.Add(logLevel);
                    logMessages.Add(logMessage);
                }));

            // define the response from the API
            var mockResponse = new HttpResponseMessage
            {
                StatusCode = System.Net.HttpStatusCode.NoContent
            };

            // set the HttpMessageHandler from the API
            var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mockHandler
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Put),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse);

            // create an instance of HttpClient with the logger mock
            var httpClient = new HttpClient(mockHandler.Object);
            httpClient.BaseAddress = new Uri("https://myurl");
            var client = new PersonService("test", httpClient, loggerMock.Object);

            // call the API implementation
            // the return is of ApiReponse<T>
            var actual = await client.UpdatePerson("1", new PersonModel()
            {
                FirstName = "Enrico",
                LastName = "Rossini"
            });

            // validate the answer
            Assert.IsNotNull(actual);
            Assert.IsTrue(actual.Success);

            // validate the HttpRequestMessage method response
            mockHandler.Protected().Verify(
                "SendAsync",
                Times.Exactly(1),
                ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Put),
                ItExpr.IsAny<CancellationToken>());

            // validate the logs
            Assert.IsTrue(logLevels.Contains(LogLevel.Information));
            Assert.IsTrue(logMessages.Count() == 2);

            var t = logMessages.Where(m => m == "[DBG][HttpVerb: PUT][URL: /people/1][HttpCode: 204]").ToList();
            Assert.IsTrue(logMessages.Where(m => m == "[DBG][HttpVerb: PUT][URL: /people/1][HttpCode: 204]").Count() > 0);
        }
    }
}