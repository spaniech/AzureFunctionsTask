using MediatR;

internal class ProcessQueueMessageCommand : IRequest
{
    public string? MessageId { get; set; }
    public BinaryData? Body { get; set; }
}
