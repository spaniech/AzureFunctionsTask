using Azure.Storage.Blobs;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

internal class ProcessQueueMessageCommandHandler(
    ILogger<ProcessQueueMessageCommandHandler> logger,
    BlobServiceClient blobServiceClient,
    IConfiguration configuration
        ) : IRequestHandler<ProcessQueueMessageCommand>
{
    private readonly ILogger<ProcessQueueMessageCommandHandler> _logger = logger;
    private readonly BlobServiceClient _blobServiceClient = blobServiceClient;
    private readonly IConfiguration _configuration = configuration;

    public async Task Handle(
        ProcessQueueMessageCommand request, 
        CancellationToken cancellationToken)
    {
        var req = request ?? throw new ArgumentNullException(nameof(request));
        var body = req.Body ?? throw new ArgumentNullException(nameof(req.Body));

        ChechIsJson(body.ToString());

        var container = await GetContainerClientAsync(cancellationToken);

        var dir = GetBlobPath();
        Guid guid = Guid.TryParse(req.MessageId, out guid) ? guid : Guid.NewGuid();
        await container.UploadBlobAsync($"{dir}/{guid}.json", body, cancellationToken);
    }

    private string GetBlobPath() => DateTime.Now.ToString("yyyy'/'MM'/'dd'/'HH'/'mm");

    private async Task<BlobContainerClient> GetContainerClientAsync(
        CancellationToken cancellationToken)
    {
        var name = _configuration.GetValue<string>("QueueName");
        var container = _blobServiceClient.GetBlobContainerClient(name);
        _ = await container.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        return container;
    }

    private void ChechIsJson(string str)
    {
        try
        {
            _ = JsonConvert.DeserializeObject(str);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deserializing the file. Verify that the file is a valid JSON file.");
            throw;
        }
    }
}