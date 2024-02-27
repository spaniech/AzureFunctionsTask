using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal class ListBlobsQuery : IRequest<IList<string>>
{
}