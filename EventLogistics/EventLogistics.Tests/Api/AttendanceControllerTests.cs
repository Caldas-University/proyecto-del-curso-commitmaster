using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EventLogistics.Application.DTOs;
using EventLogistics.Application.Interfaces;

namespace EventLogistics.EventLogistics.Tests.Api
{
    // Implementación fake de IAttendanceService
    public class FakeAttendanceService : IAttendanceService
    {
        public Task<(bool success, string message, int participantId, string participantName, string accessType)>
            RegisterAttendanceAsync(string? qrCode = null, string? document = null)
        {
            if (qrCode == "OK")
                return Task.FromResult((true, "OK", 1, "Juan Perez", "General"));
            return Task.FromResult((false, "Error", 0, "", ""));
        }

        public Task<IEnumerable<AttendanceResponseDto>> GetAttendanceByParticipantAsync(int participantId)
            => Task.FromResult<IEnumerable<AttendanceResponseDto>>(new List<AttendanceResponseDto>());

        public Task<IEnumerable<AttendanceResponseDto>> GetAttendanceByEventAsync(int eventId)
            => Task.FromResult<IEnumerable<AttendanceResponseDto>>(new List<AttendanceResponseDto>());
    }

    // Implementación fake de ICredentialService
    public class FakeCredentialService : ICredentialService
    {
        public Task<byte[]> GenerateCredentialAsync(int participantId)
            => Task.FromResult(new byte[] { 1, 2, 3 });

        public Task<PersonalizedScheduleDto?> GetPersonalizedScheduleAsync(int participantId)
            => Task.FromResult<PersonalizedScheduleDto?>(new PersonalizedScheduleDto());
    }

    public class AttendanceControllerTests
    {
        private readonly AttendanceController _controller;

        public AttendanceControllerTests()
        {
            _controller = new AttendanceController(new FakeAttendanceService(), new FakeCredentialService());
        }

        [Fact]
        public async Task CheckIn_ReturnsOk_WhenSuccess()
        {
            var dto = new AttendanceRegisterDto { QrCode = "OK", Document = "12345678" };

            var result = await _controller.CheckIn(dto);

            var okResult = Assert.IsType<OkObjectResult>(result);
            var response = Assert.IsType<CredentialResponseDto>(okResult.Value);
            Assert.Equal("Juan Perez", response.ParticipantName);
        }

        [Fact]
        public async Task CheckIn_ReturnsBadRequest_WhenNotSuccess()
        {
            var dto = new AttendanceRegisterDto { QrCode = "FAIL", Document = "12345678" };

            var result = await _controller.CheckIn(dto);

            Assert.IsType<BadRequestObjectResult>(result);
        }
    }
}