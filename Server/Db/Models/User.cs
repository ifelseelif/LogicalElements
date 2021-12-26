using System;
using System.ComponentModel.DataAnnotations;

namespace Server.Db.models
{
    public class User
    {
        [Key] public Guid Id { get; set; }
        [Required] public string Username { get; set; }
    }
}