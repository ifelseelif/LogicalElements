using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Db.models
{
    public class Connection
    {
        public Guid Id { get; set; }

        public int ElementIdIn { get; set; }
        public int ElementIdOut { get; set; }

        [ForeignKey("ElementIdIn")] public Element ElementIn { get; set; }
        [ForeignKey("ElementIdOut")] public Element ElementOut { get; set; }

        [ForeignKey("UserId")] public User User { get; set; }
        public Guid UserId { get; set; }
    }
}