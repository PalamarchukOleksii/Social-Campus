using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Abstractions
{
    [ApiController]
    public abstract class ApiController(ISender sender) : ControllerBase
    {
        protected readonly ISender _sender = sender;
    }
}
