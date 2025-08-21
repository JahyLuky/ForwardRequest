using log4net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ForwardRequest.Controllers
{
    [ApiController]
    public class ForwardRequestController : ControllerBase
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ForwardRequestController));
        private readonly HttpClient _httpClient;

        public ForwardRequestController(ILogger<ForwardRequestController> logger, HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost]
        [Route("ForwardRequest")]
        public async Task<IActionResult> Post([FromQuery] string targetUrl, [FromBody] object content)
        {
            _logger.Info($"Starting ForwardRequest");
            _logger.Info($"Target URL for API Request forward: {targetUrl}");
            _logger.Info($"Content to send: {content}");

            try
            {
                var jsonContent = new StringContent(JsonConvert.SerializeObject(content), System.Text.Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(targetUrl, jsonContent);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                _logger.Info($"Received request: {responseBody}");

                return Content(responseBody, response.Content.Headers.ContentType?.ToString() ?? "application/json");
            }
            catch (HttpRequestException ex)
            {
                _logger.Error($"Received request: {ex.Message}");
                return StatusCode(500, "Error forwarding request to external service.");
            }
        }

        [HttpGet]
        [Route("ForwardRequestGet")]
        public async Task<IActionResult> Get([FromQuery] string targetUrl)
        {
            _logger.Info($"Starting ForwardRequestGet");
            _logger.Info($"Target URL for API Request forward: {targetUrl}");

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(targetUrl);

                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                _logger.Info($"Received request: {responseBody}");

                return Content(responseBody, response.Content.Headers.ContentType?.ToString() ?? "application/json");
            }
            catch (HttpRequestException ex)
            {
                _logger.Error($"Received request: {ex.Message}");
                return StatusCode(500, "Error forwarding request to external service.");
            }
        }
    }
}
