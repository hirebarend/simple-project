namespace SimpleProject.Domain.ValueObjects
{
    public class DynamicRouteRequest
    {
        public string Method { get; set; }

        public dynamic? Payload { get; set; }

        public string Url { get; set; }
    }
}
