namespace VisitorManagement.Models
{
    public class Companion
    {
        public int CompanionId { get; set; }
        public string FullName { get; set; } = null!;
        //ithink need to add more fields here like email,contact or age or gender etc.
        public int VisitorId { get; set; }
        public virtual Visitor Visitor { get; set; } = null!;
    }
}
