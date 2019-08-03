using Merlion.Core.HttpConnectionInfo;
using Merlion.Core.Microservices.EventBus;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Compute.Application.Services
{
    //old sync call class
    //public class AuditService : IAuditService
    //{
    //    private readonly HttpClient _httpClient;
    //    private readonly string _uri;

    //    //A Typed Client is, effectively, a transient object
    //    //meaning that a new instance is created each time one is needed
    //    //and it will receive a new HttpClient instance each time it's constructed. 
    //    public AuditService(HttpClient httpClient, IConfiguration configuration)
    //    {
    //        _httpClient = httpClient;
    //        _uri = configuration["AuditServiceUri"] + "/api/v1/audit";
    //    }

    //    public async Task SubmitForAuditAsync(double number)
    //    {
    //        var httpContent = new StringContent(JsonConvert.SerializeObject(number), System.Text.Encoding.UTF8, "application/json");
    //        var response = await _httpClient.PostAsync(_uri, httpContent);
    //    }
    //}

    //new async call class
    public class AuditService : IAuditService
    {
        private readonly IPublisher _publisher;
        private readonly IConnectionContextAccessor _connCtxAccessor;

        public AuditService(
            IConnectionContextAccessor connectionContextAccessor,
            IPublisher publisher)
        {
            _publisher = publisher;
            _connCtxAccessor = connectionContextAccessor;
        }

        public async Task SubmitForAuditAsync(double number)
        {
            var submitForAuditEvent = new IntegrationEvent(
                requestId: Guid.NewGuid(),
                correlationId: _connCtxAccessor.ConnectionContext.CorrelationId,
                topicName: "SubmitForAudit",
                message: number.ToString());

            await _publisher.PublishAsync(submitForAuditEvent);
        }
    }
}
