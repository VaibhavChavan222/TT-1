using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using BusinessEntities;
using Core.Services.Users;
using WebApi.Models.Users;

namespace WebApi.Controllers
{
    [RoutePrefix("users")]
    public class UserController : BaseApiController
    {
        private readonly ICreateUserService _createUserService;
        private readonly IDeleteUserService _deleteUserService;
        private readonly IGetUserService _getUserService;
        private readonly IUpdateUserService _updateUserService;

        public UserController(ICreateUserService createUserService, IDeleteUserService deleteUserService, IGetUserService getUserService, IUpdateUserService updateUserService)
        {
            _createUserService = createUserService;
            _deleteUserService = deleteUserService;
            _getUserService = getUserService;
            _updateUserService = updateUserService;
        }

        [Route("{userId:guid}/create")]
        [HttpPost]
        public HttpResponseMessage CreateUser(Guid userId, [FromBody] UserModel model)
        {
            var existingUser = _getUserService.GetUser(userId);
            if (existingUser != null)
            {
                userId = Guid.NewGuid(); //Task 1 : Generate a new GUID if the user ID already exists
            }
            var user = _createUserService.Create(userId, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
            return Found(new UserData(user));
        }

        [Route("{userId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateUser(Guid userId, [FromBody] UserModel model)
        {
            //Task 1 : Validate the empty email address
            if (string.IsNullOrEmpty(model.Email))
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Email is empty for the user.");
            }

            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                return DoesNotExist();
            }
            _updateUserService.Update(user, model.Name, model.Email, model.Type, model.AnnualSalary, model.Tags);
            return Found(new UserData(user));
        }

        [Route("{userId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                return DoesNotExist();
            }
            _deleteUserService.Delete(user);
            return Found();
        }

        [Route("{userId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetUser(Guid userId)
        {
            var user = _getUserService.GetUser(userId);
            return Found(new UserData(user));
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetUsers(int skip, int take, UserTypes? type = null, string name = null, string email = null)
        {
            var users = _getUserService.GetUsers(type, name, email)
                                       .Skip(skip).Take(take)
                                       .Select(q => new UserData(q))
                                       .ToList();
            return Found(users);
        }

        [Route("clear")]
        [HttpDelete]
        public HttpResponseMessage DeleteAllUsers()
        {
            _deleteUserService.DeleteAll();
            return Found();
        }

        [Route("list/tag")]
        [HttpGet]
        public HttpResponseMessage GetUsersByTag(string tag)
        {
            // Example implementation
            var users = GetUsersFromDatabaseByTag(tag); // Assume this method fetches users from the database
            if (users == null || !users.Any())
            {
                return Request.CreateResponse(HttpStatusCode.NotFound, "No users found with the given tag.");
            }
            return Request.CreateResponse(HttpStatusCode.OK, users);
        }

        private IEnumerable<User> GetUsersFromDatabaseByTag(string tag)
        {
            return new List<User>();
        }
    }
}