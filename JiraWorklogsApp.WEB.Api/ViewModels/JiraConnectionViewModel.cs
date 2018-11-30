using System.ComponentModel.DataAnnotations;

namespace JiraWorklogsApp.WEB.Api.ViewModels
{
    public class JiraConnectionViewModel : BaseViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string InstanceUrl { get; set; }
        [Required]
        public string UserName { get; set; }
        public string AuthToken { get; set; }
        public string TempoAuthToken { get; set; }
    }
}
