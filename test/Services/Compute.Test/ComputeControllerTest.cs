using Compute.Application.Controllers;
using Compute.Application.Services;
using Compute.Domain.Models;
using FakeItEasy;
using System;
using Xunit;

namespace Compute.Test
{
    public class ComputeControllerTest
    {
        [Fact]
        public void the_audit_service_should_be_callled_from_operation_post_request()
        {
            // Arange
            var fakeIOperationRepository = A.Fake<IOperationRepository>();
            var fakeIAuditService = A.Fake<IAuditService>();
            var computeController = new ComputeController(fakeIOperationRepository, fakeIAuditService);
            
            // Act
            computeController.Cal(4, 5, 2);

            // Assert
            A.CallTo(
                () => fakeIAuditService.SubmitForAuditAsync(A<double>.Ignored)).MustHaveHappened(Repeated.Exactly.Once);

        }
    }
}
