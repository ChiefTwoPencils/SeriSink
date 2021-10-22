using Microsoft.Extensions.Logging;
using SeriSink.Interfaces;

namespace SeriSink.Services
{
    public class Service : IService
    {
        private readonly ILogger<Service> _logger;

        public Service(ILogger<Service> logger)
            => _logger = logger;

        public void Do()
        {
            _logger.LogInformation(
                "We're in {@Service} with a {@Type}.",
                nameof(Service), _logger.GetType()
            );
        }
    }
}
