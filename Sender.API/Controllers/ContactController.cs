using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sender.API.DTO;
using Sender.Application;

namespace Sender.API.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ContactController: BaseController
    {

        private readonly ContactCRUD _contactCRUD;

        public ContactController(ContactCRUD contactCRUD)
        {
            _contactCRUD = contactCRUD;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContactRequest request)
        {
            var result = await _contactCRUD.CreateContactAsync(decodeToken.Id, request.Name, request.Email, request.TelegramUsername);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return StatusCode(result.StatusCode, ApiResponse<object>.SuccessResult(new { contact = result.Value }));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(Guid id)
        {
            var result = await _contactCRUD.GetOneContactAsync(decodeToken.Id, id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return StatusCode(result.StatusCode, ApiResponse<object>.SuccessResult(new { contact = result.Value }));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _contactCRUD.GetAllContactAsync(decodeToken.Id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return StatusCode(result.StatusCode, ApiResponse<object>.SuccessResult(new { allContact = result.Value }));
        }

        [Authorize]
        [HttpPatch]
        public async Task<IActionResult> Update([FromBody] UpdateContactRequest request)
        {
            var result = await _contactCRUD.UpdateContactAsync(request.Id, decodeToken.Id, request.Name, request.Email, request.TelegramUsername);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return StatusCode(result.StatusCode, ApiResponse<object>.SuccessResult(new { contact = result.Value }));
        }

        [Authorize]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _contactCRUD.DeleteContactAsync(decodeToken.Id, id);

            if (!result.IsSuccess)
            {
                return StatusCode(result.StatusCode, ApiResponse<object>.FailureResult(result.Error));
            }

            return NoContent();
        }
    }
}
