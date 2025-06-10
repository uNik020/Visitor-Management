namespace VisitorManagement.Models
{
    public class Designation
    {
        public int DesignationId { get; set; }

        public string DesignationName { get; set; }

        public virtual ICollection<Hosts> Hosts { get; set; } = new List<Hosts>();
    }
}
