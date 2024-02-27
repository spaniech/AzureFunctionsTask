using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class ListBlobs(IMediator mediator)
{
    private readonly IMediator _mediator = mediator;

    [Function(nameof(ListBlobs))]
    public async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req)
    {
        ListBlobsQuery query = new();
        var res = await _mediator.Send(query);
        return new JsonResult(res);
    }
}
