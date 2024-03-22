using ClarivateApp.Authentication.Basic;
using ClarivateApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClarivateApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("getUser"), BasicAuthorization]       
        public async Task<IActionResult> GetRandomUser()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync("https://randomuser.me/api/");
                response.EnsureSuccessStatusCode();

                var result =await response.Content.ReadAsStringAsync();

                dynamic json = JsonConvert.DeserializeObject(result);

                return Ok(json);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
