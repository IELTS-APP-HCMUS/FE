using Newtonsoft.Json;

public class ApiResponse<T>
{
	[JsonProperty("code")]
	public int Code { get; set; }

	[JsonProperty("message")]
	public string Message { get; set; }

	[JsonProperty("data")]
	public T Data { get; set; }
}