using ChatKid.Api.Models;
using ChatKid.Api.Models.RequestModels;
using ChatKid.Common.CommandResult;
using ChatKid.Common.Providers;
using ChatKid.OpenAI.ChatCompletions;
using ChatKid.OpenAI.ImageGeneration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Chat;
using OpenAI_API.Images;
using System.Net;

namespace ChatKid.Api.Controllers
{
    [Route("api/gpt")]
    [ApiController]
    [Authorize]
    public class GPTController : ControllerBase
    {
        private readonly IChatCompletionService chatCompletionService;
        private readonly IImageGenerateService imageGenerateService;
        public GPTController(IChatCompletionService chatCompletionService,
            IImageGenerateService imageGenerateService)
        {
            this.chatCompletionService = chatCompletionService;
            this.imageGenerateService = imageGenerateService;
        }

        [HttpPost("chat")]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChatGpt([FromBody] BotChatCompletionRequest request)
        {
            var response = await this.chatCompletionService.GetChatCompletion(request.Message);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Ok(response);
        }

        [HttpPost("chat-stream")]
        [ProducesResponseType(typeof(IAsyncEnumerable<ChatResult>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChatStreamGpt([FromBody] BotChatCompletionRequest request)
        {
            var response = await this.chatCompletionService.GetChatStreamCompletion(request.Message);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Ok(response);
        }

        [HttpPost("image")]
        [ProducesResponseType(typeof(ImageResult), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GenerateImage([FromBody] BotImageGenerateRequest request)
        {
            var response = await this.imageGenerateService.GenerateImage(request.Promt, request.Quantity, request.Size);
            if (!response.Succeeded) return StatusCode(response.GetStatusCode(), response.GetData());
            return Ok(response);
        }
    }
}
