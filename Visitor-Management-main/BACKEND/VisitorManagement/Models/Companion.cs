namespace VisitorManagement.Models
{
    public class Companion
    {
        public int CompanionId { get; set; }
        public string FullName { get; set; } = null!;
        public int VisitorId { get; set; }
        public virtual Visitor Visitor { get; set; } = null!;
    }
}
