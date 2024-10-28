using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using WebsiteApi.Controllers;

namespace WebsiteApi.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    //public int GitHubId { get; set; }

    //[MaxLength(255)]
    //public required string GitHubUsername { get; set; }

    [MaxLength(255)]
    public required string Name { get; set; }

    [MaxLength(255)]
    [EmailAddress]
    public required string Email { get; set; }


    //public DateTime TokenExpiration { get; set; }

    //public DateTime CreatedAt { get; set; } = DateTime.Now;

    //public DateTime? LastLogin { get; set; }

    //public virtual ICollection<User> Friends { get; set; } = new List<User>();
    //public virtual ICollection<User> FriendOf { get; set; } = new List<User>();
}