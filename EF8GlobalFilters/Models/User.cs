using System.ComponentModel.DataAnnotations.Schema;

namespace EF8GlobalFilters.Models;

[Table("Users")]
public class User
{
    public int Id { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
}