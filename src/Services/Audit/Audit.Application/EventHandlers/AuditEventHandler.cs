using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Audit.Domain.Models;
using Merlion.Core.Microservices.EventBus;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Audit.Application.EventHandlers
{
    internal class AuditEventHandler : BaseEventHandler
    {
        public AuditEventHandler(
            ILogger<AuditEventHandler> logger,
            ISubscriber subscriber,
            IServiceProvider serviceProvider)
            : base(logger, subscriber, serviceProvider)
        {
        }

        protected internal override async Task HandleEventAsync(IntegrationEvent receivedMessage, IServiceScope serviceScope)
        {
            var runningTotalRepo = serviceScope.ServiceProvider.GetRequiredService<IRunningTotalRepository>();

            double number = Convert.ToDouble(receivedMessage.Message);
            var previousTotal = await runningTotalRepo.GetPrevioudTotal();
            var runningTotal = new RunningTotal(previousTotal, number);
            runningTotalRepo.CreateRunningTotal(runningTotal);
            await runningTotalRepo.SaveChangesAsync();

        }
    }
}
