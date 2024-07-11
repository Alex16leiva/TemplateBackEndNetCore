using Infraestructura.Core.RestClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebServices.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestRestClient : Controller
    {
        [AllowAnonymous]
        [Route("")]
        [HttpGet]
        public principal getData()
        {
            var response = new principal();
            Task.Run(async () =>
            {
                response = await getExample();
            }).GetAwaiter().GetResult();

            var responsePost = new MethodPostResponse();

            Task.Run(async () =>
            {
                responsePost = await Post();
            }).GetAwaiter().GetResult();

            return response;
        }

        private async Task<principal> getExample()
        {
            string baseUri = "https://coderbyte.com";
            string uri = "/api/challenges/json/json-cleaning";
            return await RestClientFactory.CreateClient(baseUri).GetAsync<principal>(uri);
        }


        private async Task<MethodPostResponse> Post()
        {
            var baseUri = "https://postman-echo.com";
            var uri = "/post";
            var request = new MethodPostRequest { Test = "value"};

            return await RestClientFactory.CreateClient(baseUri).PostAsync<MethodPostRequest, MethodPostResponse>(uri, request);
        }
    }

    public class MethodPostRequest
    {
        public string Test { get; set; }
    }

    public class MethodPostResponse
    {
        public Args Args { get; set; }
        public Args Data { get; set; }
        public Args Files { get; set; }
    }

    public class Args 
    {
        public string Test { get; set; }
        public string host { get; set; }
        public string cookie { get; set; }
    }


    public class principal
    {
        public Name Name { get; set; }
        public int Age { get; set; }
    }

    public class Name
    {

        public string First { get; set; }
        public string middle { get; set; }
        public string last { get; set; }
        //{"name":{"first":"Robert","middle":"","last":"Smith"},"age":25,"DOB":"-","hobbies":["running","coding","-"],"education":{"highschool":"N\/A","college":"Yale"}}
    }
}
