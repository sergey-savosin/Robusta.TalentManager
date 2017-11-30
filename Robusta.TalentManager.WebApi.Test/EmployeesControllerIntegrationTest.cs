using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Http;
using Robusta.TalentManager.WebApi.Core.Configuration;
using System.Threading;
using System.Security.Principal;
using System.Net.Http;
using System.Net;
using Robusta.TalentManager.WebApi.Dto;

namespace Robusta.TalentManager.WebApi.Test
{
    [TestClass]
    public class EmployeesControllerIntegrationTest
    {
        private HttpServer server = null;
        private static bool IsMapperInit = false;

        [TestInitialize]
        public void Initialize()
        {
            var configuration = new HttpConfiguration();

            if (!IsMapperInit)
            {
                DtoMapperConfig.CreateMaps();
                IsMapperInit = true;
            }

            IocConfig.RegisterDependencyResolver(configuration);
            WebApiConfig.Register(configuration);

            server = new HttpServer(configuration);

            // This test runs under the context of my user
            Thread.CurrentPrincipal = new GenericPrincipal(
                new GenericIdentity(String.Empty),
                null);
        }

        [TestMethod]
        public void MustReturn401WhenNoCredentialsInRequest()
        {
            using (var invoker = new HttpMessageInvoker(server))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get,
                    "http://localhost/api/employees/1"))
                {
                    using (var response = invoker.SendAsync(request, CancellationToken.None).Result)
                    {
                        Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
                    }
                }
            }
        }

        [TestMethod]
        public void MustReturn200AndEmployeeWhenCredentialsAreSupplied()
        {
            using (var invoker = new HttpMessageInvoker(server))
            {
                using (var request = new HttpRequestMessage(HttpMethod.Get,
                    "http://localhost/api/employees/1"))
                {
                    request.Headers.Add("X-PSK", "somekey"); // Credentials
                    using (var response = invoker.SendAsync(request, CancellationToken.None).Result)
                    {
                        Assert.IsNotNull(response);
                        Assert.IsNotNull(response.Content);
                        Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                        Assert.IsInstanceOfType(response.Content, typeof(ObjectContent<EmployeeDto>));

                        var content = (response.Content as ObjectContent<EmployeeDto>);
                        var result = content.Value as EmployeeDto;

                        Assert.AreEqual(1, result.Id);
                        Assert.AreEqual("test", result.FirstName);
                        Assert.AreEqual("test", result.LastName);
                    }
                }
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (server != null)
            {
                server.Dispose();
            }
            
        }
    }
}
