using System;
using Azure.Storage.Queues.Models;
using MediatR;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

public class ProcessQueueMessage(ILogger<ProcessQueueMessage> logger, IMediator mediator)
{
    private readonly ILogger<ProcessQueueMessage> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [Function(nameof(ProcessQueueMessage))]
    public async Task Run(
        [QueueTrigger("%QueueName%", Connection = "AzureWebJobsStorage")] QueueMessage message)
    {
        _logger.LogInformation($"Queue trigger function processed: {message.MessageText}");

        ProcessQueueMessageCommand command = new()
        {
            Body = message.Body,
            MessageId = message.MessageId
        };
        await _mediator.Send(command);
    }
}
