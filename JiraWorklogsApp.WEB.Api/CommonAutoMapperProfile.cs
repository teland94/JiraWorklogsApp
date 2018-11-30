using AutoMapper;
using JiraWorklogsApp.DAL.Entities.Models;
using JiraWorklogsApp.WEB.Api.ViewModels;

namespace JiraWorklogsApp.WEB.Api
{
    public class CommonAutoMapperProfile : Profile
    {
        public CommonAutoMapperProfile()
        {
            CreateMap<JiraConnection, JiraConnectionViewModel>();
            CreateMap<JiraConnectionViewModel, JiraConnection>();
        }
    }
}
