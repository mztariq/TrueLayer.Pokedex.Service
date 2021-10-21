namespace TrueLayer.Pokedex.Domain.Models
{
    public class SampleSettings
    {
        public string ServiceName { get; set; }
        public string ApiBaseUri { get; set; }
        public string ApiRelativeUri { get; set; }
        public int DefaultMaxRetryAttempts { get; set; }
        public int DefaultWaitTimeInSecondsBetweenAttempts { get; set; }
        public int DefaultCircuitBreakerTimeInSeconds { get; set; }
    }
}