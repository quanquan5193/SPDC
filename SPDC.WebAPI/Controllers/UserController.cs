using AutoMapper;
using SPDC.Common;
using SPDC.Model.ViewModels;
using SPDC.Service.Services;
using SPDC.WebAPI.Helpers;
using SPDC.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Cors;

namespace SPDC.WebAPI.Controllers
{

    [Authorize]
    [RoutePrefix("api/User")]
    [EnableCorsAttribute("*", "*", "*")]
    public class UserController : ApiControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("all-user")]
        //[Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetAllUser(int role = 0)
        {
            Log.Info($"Start Get All User - Role: {role}");
            var result = await _userService.GetAllUser(role);
            Log.Info($"Completed Get All User - Role: {role}");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, Mapper.Map<IEnumerable<UserDataViewModel>>(result)));
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("user-by-id")]
        //[Authorize(Roles = "Admin")]
        public async Task<IHttpActionResult> GetUserByID(int userId)
        {
            Log.Info($"Start Get User - User Id: {userId}");
            var result = await _userService.GetUserByID(userId);
            Log.Info($"Completed Get User - User Id: {userId}");

            return Content(HttpStatusCode.OK, new ActionResultModel(FileHelper.GetServerMessage("success","Common", GetLanguageCode().ToString()), true, Mapper.Map<UserDataViewModel>(result)));
        }
    }
}
