namespace Portfolio.Models
{
    public class ProjectsViewModel
    {
        public List<Project> Projects { get; set; }
        public List<string> Technologies { get; set; }
        public string SelectedTechnology { get; set; }
        public string ViewMode { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
