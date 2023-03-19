using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;

namespace StoriesWeb.Models
{
  public class EmailRecepient
  {
    [Key]
    public int Id { get; set; }

    public int EmailLogId { get; set; }
    public bool Sent { get; set; } = false;
    public DateTime? SentAt { get; set; } = DateTime.UtcNow;

    public string? Error { get; set; } = "";

    public EmailLog EmailLog { get; set; }
  }
}