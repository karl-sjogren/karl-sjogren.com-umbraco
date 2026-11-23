using Asp.Versioning;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace UmbracoBlog.BackofficeExtension.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "UmbracoBlog.BackofficeExtension")]
public class UmbracoBlogBackofficeExtensionApiController : UmbracoBlogBackofficeExtensionApiControllerBase {
    [HttpGet("ping")]
    [ProducesResponseType<string>(StatusCodes.Status200OK)]
    public string Ping() => "Pong";
}
