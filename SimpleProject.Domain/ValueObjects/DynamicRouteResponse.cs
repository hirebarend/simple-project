namespace SimpleProject.Domain.ValueObjects
{
    public class DynamicRouteResponse
    {
        public dynamic? Payload { get; set; }

        public bool Success { get; set; }
    }
}
