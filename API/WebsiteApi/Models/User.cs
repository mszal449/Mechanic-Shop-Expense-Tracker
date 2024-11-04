using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;
using WebsiteApi.Controllers;

namespace WebsiteApi.Models;

public class User
{
    [Key]
    public int UserId { get; set; }

    [MaxLength(255)]
    public required string Name { get; set; }

    [MaxLength(255)]
    [EmailAddress]
    public required string Email { get; set; }
}