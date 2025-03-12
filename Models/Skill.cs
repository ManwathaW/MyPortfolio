namespace Portfolio.Models
{
    public class Skill
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ProficiencyLevel { get; set; } // 1-5 scale
        public string Category { get; set; } // e.g., "Frontend", "Backend", "Database"
    }
}
