using Microsoft.AspNetCore.Mvc;
using Sender.Domain;

public abstract class BaseController : ControllerBase
{
    protected Token decodeToken => HttpContext.Items["Token"] as Token
        ?? throw new UnauthorizedAccessException("Токен не був розшифрований або відсутній.");
}