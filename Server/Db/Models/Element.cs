using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common;

namespace Server.Db.models
{
    public class Element
    {
        [Key] public int Id { get; set; }

        public ElemType ElemType { get; set; }
        public bool Value { get; set; }
        public string Name { get; set; }
        public bool IsInput { get; set; }

        [ForeignKey("UserId")] public User User { get; set; }
        public Guid UserId { get; set; }
    }
}