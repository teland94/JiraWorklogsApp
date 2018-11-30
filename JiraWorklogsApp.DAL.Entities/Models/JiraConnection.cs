using System.ComponentModel.DataAnnotations;
using JiraWorklogsApp.DAL.Entities.Base;

namespace JiraWorklogsApp.DAL.Entities.Models
{
    public class JiraConnection : EntityBase
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string InstanceUrl { get; set; }
        [Required]
        public string UserName { get; set; }
        public string AuthToken { get; set; }
        public string TempoAuthToken { get; set; }

        public string UserId { get; set; }
    }
}
