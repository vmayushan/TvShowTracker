using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TvShowTracker.DataAccess.Storage.CollectionsSetup;

namespace TvShowTracker.WebApi.HostedServices
{
    public class MongoSetupHostedService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<MongoSetupHostedService> _logger;
        public MongoSetupHostedService(IServiceProvider serviceProvider, 
            ILogger<MongoSetupHostedService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var collectionsSetupObjects = (IEnumerable<IMongoCollectionSetup>) _serviceProvider
                .GetService(typeof(IEnumerable<IMongoCollectionSetup>));

            try
            {
                foreach (var collectionSetup in collectionsSetupObjects)
                {
                    _logger.LogInformation("Running mongo collection setup: {collectionName}", collectionSetup.GetType().Name);
                    await collectionSetup.Setup();
                }
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
                throw;
            }
        }
        
        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}