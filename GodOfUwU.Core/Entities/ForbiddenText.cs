namespace GodOfUwU.Core.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class ForbiddenText
    {
#nullable disable

        [Key]
        public virtual string Text { get; set; }

        public virtual Guild Guild { get; set; }
#nullable enable
    }
}