namespace StoriesWeb.Models.Helpers
{
  public class ApiResponse<T>
  {
    public bool Successful { get; set; } = true;
    public T? Data { get; set; }
    public string? ErrorMessage { get; set; }
  }
}