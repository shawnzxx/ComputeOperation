using Merlion.Core.Microservices.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Audit.Application.EventHandlers
{
    internal abstract class BaseEventHandler : IHostedService
    {
        protected readonly ILogger _logger;
        protected readonly ISubscriber _subscriber;
        protected readonly IServiceProvider _serviceProvider;

        protected BaseEventHandler(
            ILogger logger,
            ISubscriber subscriber,
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _subscriber = subscriber;
            _serviceProvider = serviceProvider;

            _subscriber.OnMessageReceived += ProcessMessageAsync;
        }

        Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            Task.Factory.StartNew(() =>
            {
                _subscriber.StartSubscriberAsync();
            },
            TaskCreationOptions.LongRunning);

            return Task.CompletedTask;
        }

        private async Task ProcessMessageAsync(IntegrationEvent integrationEvent)
        {
            using (var serviceScope = _serviceProvider.CreateScope())
            {
                string logEntry = $"Topic: {integrationEvent.TopicName} Message: {integrationEvent.Message} Key: {integrationEvent.Key}";
                _logger.LogInformation($"{nameof(ProcessMessageAsync)} {logEntry}");

                await HandleEventAsync(integrationEvent, serviceScope);
            }
        }

        protected internal abstract Task HandleEventAsync(IntegrationEvent receivedMessage, IServiceScope serviceScope);

        Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"{nameof(IHostedService.StopAsync)} Stop Subscriber");
            _subscriber.StopSubscriber();
            return Task.CompletedTask;
        }
    }
}
