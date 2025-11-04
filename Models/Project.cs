using System.ComponentModel.DataAnnotations;

namespace Portfolio.Models
{
    // Project.cs
    public class Project
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot be longer than 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        [Display(Name = "Project URL")]
        public string ProjectUrl { get; set; }

        [Display(Name = "GitHub URL")]
        public string GitHubUrl { get; set; }

        private List<string> _technologies = new List<string>();

        public List<string> Technologies
        {
            get { return _technologies ?? new List<string>(); }
            set { _technologies = value ?? new List<string>(); }
        }

        [Display(Name = "Completion Date")]
        [DataType(DataType.Date)]
        public DateTime CompletionDate { get; set; }

        [Display(Name = "Featured Project")]
        public bool IsFeatured { get; set; }
    }
}
