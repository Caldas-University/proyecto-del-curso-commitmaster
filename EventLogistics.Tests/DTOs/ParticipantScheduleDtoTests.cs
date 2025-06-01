using Xunit;

public class ParticipantScheduleDtoTests
{
    [Fact]
    public void Constructor_ShouldInitializeProperties()
    {
        var dto = new ParticipantScheduleDto("John Doe", "Event 1", "2023-10-01T10:00:00");

        Assert.NotNull(dto.ParticipantName);
        Assert.NotNull(dto.EventName);
        Assert.NotNull(dto.EventDate);
    }

    [Fact]
    public void Properties_ShouldNotAcceptNullValues()
    {
        var exception = Assert.Throws<ArgumentNullException>(() => new ParticipantScheduleDto(null, "Event 1", "2023-10-01T10:00:00"));
        Assert.Equal("participantName", exception.ParamName);

        exception = Assert.Throws<ArgumentNullException>(() => new ParticipantScheduleDto("John Doe", null, "2023-10-01T10:00:00"));
        Assert.Equal("eventName", exception.ParamName);

        exception = Assert.Throws<ArgumentNullException>(() => new ParticipantScheduleDto("John Doe", "Event 1", null));
        Assert.Equal("eventDate", exception.ParamName);
    }
}