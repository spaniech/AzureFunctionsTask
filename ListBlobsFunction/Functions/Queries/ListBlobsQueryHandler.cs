using Azure.Storage.Blobs;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ListBlobsFunction;

internal class ListBlobsQueryHandler(
    ILogger<ListBlobsQueryHandler> logger,
    BlobServiceClient blobServiceClient,
    IConfiguration configuration
        ) : IRequestHandler<ListBlobsQuery, IList<string>>
{
    private readonly ILogger<ListBlobsQueryHandler> _logger = logger;
    private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
    private readonly IConfiguration _configuration = configuration;

    public async Task<IList<string>> Handle(ListBlobsQuery request, CancellationToken cancellationToken)
    {
        var container = await GetContainerClientAsync(cancellationToken);
        List<string> result = [];

        if (!await container.ExistsAsync(cancellationToken: cancellationToken))
            return result;

        var blobs = container.GetBlobsAsync(cancellationToken: cancellationToken);
        await foreach (var blob in blobs)
        {
            result.Add(blob.Name);
        }

        return result;
    }

    private async Task<BlobContainerClient> GetContainerClientAsync(
        CancellationToken cancellationToken)
    {
        var name = _configuration.GetValue<string>("QueueName");
        var container = _blobServiceClient.GetBlobContainerClient(name);
        return await Task.FromResult(container);
    }
}
