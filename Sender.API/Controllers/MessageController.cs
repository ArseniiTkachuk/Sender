using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sender.API.DTO;
using Sender.Application;

namespace Sender.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessageController : BaseController
    {
        private readonly MessageCRUD _messageCRUD;

        public MessageController(MessageCRUD messageCRUD)
        {
            _messageCRUD = messageCRUD;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMessageRequest request)
        {
            var result = await _messageCRUD.CreateMessageAsync(request.Subject, request.Body, decodeToken.Id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return StatusCode(result.StatusCode, ApiResponse<object>.SuccessResult(new { message = result.Value }));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var result = await _messageCRUD.GetOneMessageAsync(decodeToken.Id, id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return StatusCode(result.StatusCode, ApiResponse<object>.SuccessResult(new { message = result.Value }));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _messageCRUD.GetAllMessageAsync(decodeToken.Id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return StatusCode(result.StatusCode, ApiResponse<object>.SuccessResult(new { allMessage = result.Value }));
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateMessageRequest request)
        {
            var result = await _messageCRUD.UpdateMessageAsync(request.Id, request.Subject, request.Body, decodeToken.Id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return StatusCode(result.StatusCode, ApiResponse<object>.SuccessResult(new { message = result.Value }));

        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _messageCRUD.DeleteMessageAsync(decodeToken.Id, id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return NoContent();
        }

        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageRequest request)
        {
            var result = await _messageCRUD.SendMessageAsync(decodeToken.Id, request.Contacts, request.Message, request.messageId);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return StatusCode(result.StatusCode, ApiResponse<object>.SuccessResult(result.Value));
        }

    }
}
