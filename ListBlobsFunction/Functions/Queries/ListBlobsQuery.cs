using MediatR;

namespace ListBlobsFunction;

internal class ListBlobsQuery : IRequest<IList<string>>
{
}