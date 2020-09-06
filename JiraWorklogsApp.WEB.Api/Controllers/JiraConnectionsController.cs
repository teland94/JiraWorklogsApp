using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AutoMapper;
using JiraWorklogsApp.BLL.IServices;
using JiraWorklogsApp.DAL.Entities.Models;
using JiraWorklogsApp.WEB.Api.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JiraWorklogsApp.WEB.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class JiraConnectionsController : BaseController
    {
        private IJiraConnectionsService JiraConnectionsService { get; }
        private IMapper Mapper { get; }

        public JiraConnectionsController(IJiraConnectionsService jiraConnectionsService,
            IMapper mapper)
        {
            JiraConnectionsService = jiraConnectionsService;
            Mapper = mapper;
        }

        // GET: api/JiraConnections
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(Mapper.Map<IEnumerable<JiraConnection>,
                IEnumerable<JiraConnectionViewModel>>(await JiraConnectionsService.GetAsync(GetAdUserId(), false)));
        }

        [HttpGet(nameof(GetShortInfo))]
        public async Task<IActionResult> GetShortInfo()
        {
            return Ok(await JiraConnectionsService.GetShortInfoAsync());
        }

        // GET: api/JiraConnections/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<IActionResult> Get(int id)
        {
            return Ok(Mapper.Map<JiraConnection, JiraConnectionViewModel>(await JiraConnectionsService.GetAsync(id, false)));
        }

        // POST: api/JiraConnections
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]JiraConnectionViewModel value)
        {
            var connection = Mapper.Map<JiraConnectionViewModel, JiraConnection>(value);
            connection.UserId = GetAdUserId();
            var id = await JiraConnectionsService.CreateAsync(connection);
            return CreatedAtAction(nameof(Post), id);
        }

        // PUT: api/JiraConnections
        [HttpPut]
        public async Task<IActionResult> Put([FromBody]JiraConnectionViewModel value)
        {
            var connection = Mapper.Map<JiraConnectionViewModel, JiraConnection>(value);
            connection.UserId = GetAdUserId();
            await JiraConnectionsService.UpdateAsync(connection);
            return NoContent();
        }

        // DELETE: api/JiraConnections/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await JiraConnectionsService.DeleteAsync(id);
            return NoContent();
        }

        [HttpPost(nameof(Test))]
        public async Task<IActionResult> Test([FromBody]JiraConnectionViewModel value)
        {
            try
            {
                var connection = Mapper.Map<JiraConnectionViewModel, JiraConnection>(value);
                connection.UserId = GetAdUserId();
                await JiraConnectionsService.TestAsync(connection);

                return NoContent();
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
    }
}
