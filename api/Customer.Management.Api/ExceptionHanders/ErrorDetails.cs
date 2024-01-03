namespace Customers.Management.Api.ExceptionHanders
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public override string ToString() => System.Text.Json.JsonSerializer.Serialize(this);
    }
}
