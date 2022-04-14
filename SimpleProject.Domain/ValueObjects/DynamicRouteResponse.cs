namespace SimpleProject.Domain.ValueObjects
{
    public class DynamicRouteResponse
    {
        public object? Payload { get; set; }

        public bool Success { get; set; }
    }
}
