namespace GodOfUwU.Core.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Warning
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Warning()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
        }

        public Warning(string message)
        {
            Message = message;
            Timestamp = DateTime.UtcNow;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity), Key()]
        public virtual ulong Id { get; set; }

        public virtual string Message { get; set; }

        public virtual DateTime Timestamp { get; set; }
    }
}