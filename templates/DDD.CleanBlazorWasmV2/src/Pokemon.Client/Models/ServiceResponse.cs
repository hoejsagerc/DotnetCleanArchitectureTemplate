namespace Pokemon.Client.Models;

public class ServiceResponse<T>
{
    public string? Message { get; set; } = string.Empty;
    public int StatusCode { get; set; }
    public T? Data { get; set; }
}