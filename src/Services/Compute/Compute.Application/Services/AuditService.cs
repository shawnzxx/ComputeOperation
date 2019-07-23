using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Compute.Application.Services
{
    public class AuditService : IAuditService
    {
        private readonly HttpClient _httpClient;
        private readonly string _uri;

        public AuditService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _uri = configuration["AuditServiceUri"] + "/api/v1/audit";
        }

        public async Task SubmitForAuditAsync(double number)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(number), System.Text.Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(_uri, httpContent);
        }
    }
}
