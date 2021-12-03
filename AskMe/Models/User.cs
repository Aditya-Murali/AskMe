using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AskMe.Models
{
    public class User
    {
        [Key]
        public string UserId { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

    }
}