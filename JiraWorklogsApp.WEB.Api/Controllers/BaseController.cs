using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace JiraWorklogsApp.WEB.Api.Controllers
{
    public class BaseController : ControllerBase
    {
        protected string GetAdUserId()
        {
            return User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
