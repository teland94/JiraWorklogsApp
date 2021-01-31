using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using JiraWorklogsApp.BLL.IServices;
using JiraWorklogsApp.Common.Models;
using JiraWorklogsApp.Common.Models.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JiraWorklogsApp.WEB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReportController : BaseController
    {
        private IReportService ReportService { get; }

        public ReportController(IReportService reportService)
        {
            ReportService = reportService;
        }

        [HttpPost(nameof(GetProjects))]
        public async Task<IActionResult> GetProjects([FromBody]ICollection<JiraConnectionShortInfo> jiraConnections)
        {
            try
            {
                return Ok(await ReportService.GetProjectsAsync(jiraConnections, GetAdUserId()));
            }
            catch (HttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost(nameof(GetAssignableUsers))]
        public async Task<IActionResult> GetAssignableUsers([FromBody]GetAssignableUsersParams getAssignableUsersParams)
        {
            try
            {
                return Ok(await ReportService.GetAssignableUsers(getAssignableUsersParams));
            }
            catch (HttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost(nameof(GetReportList))]
        public async Task<IActionResult> GetReportList([FromBody]GetReportListParams getReportListParams)
        {
            try
            {
                return Ok(await ReportService.GetReportListAsync(getReportListParams));
            }
            catch (HttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [HttpPost(nameof(GetReportExcelFile))]
        public async Task<IActionResult> GetReportExcelFile([FromBody]GetReportListParams getReportParams)
        {
            try
            {
                var data = await ReportService.GetReportExcelDataAsync(getReportParams);
                return File(data, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "Report.xlsx");
            }
            catch (HttpRequestException e)
            {
                return BadRequest(e.Message);
            }
            catch (InvalidOperationException e)
            {
                return UnprocessableEntity(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}