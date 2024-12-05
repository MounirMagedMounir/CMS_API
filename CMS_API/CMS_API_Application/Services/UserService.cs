using CMS_API_Application.Dto;
using CMS_API_Application.Dto.Admin;
using CMS_API_Application.Interfaces.Servises;
using CMS_API_Core.DomainModels;
using CMS_API_Core.DomainModels.Authorization;
using CMS_API_Core.FilterModels;
using CMS_API_Core.helper.Response;
using CMS_API_Core.helper.Utils;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Core.Interfaces.Repository.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Security.Claims;


namespace CMS_API.Services
{
    public class UserService(IConfiguration configuration
        , IHttpContextAccessor _httpContextAccessor
         , IUserRepository _userRepository
          , IRoleRepository _RoleRepository
        ) : IUserService
    {

        readonly EmailSender emailSender = new(configuration);


        public ApiResponse<object?> Create(CreateUserDto UserRequest)
        {

            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }

            var NewUserId = Guid.NewGuid().ToString();

            var Role = _RoleRepository.GetRoleByName(UserRequest.Role);

            if (Role is null)
            {
                return new ApiResponse<object?>(
                   data: null,
                   status: StatusCodes.Status404NotFound,
                   message: [" Role dosen`t exist"]);
            }

            User NewUser = new()
            {
                Id = NewUserId,
                Name = UserRequest.Name,
                Email = UserRequest.Email,
                UserName = UserRequest.UserName,
                Phone = UserRequest.Phone,
                RoleId = Role.Id,
                IsActive = UserRequest.IsActive,
                LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                LastUpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                CreatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value
            };

            var Password = Guid.NewGuid().ToString().Substring(0, 10).Insert(1, "@mM1");

            NewUser.Password = Password;

            if (_userRepository.IsEmailExists(UserRequest.Email))
            {
                return new ApiResponse<object?>(
              data: null,
              status: StatusCodes.Status400BadRequest,
              message: [" Email Aleardy exist"]);
            }


            _userRepository.AddUser(NewUser);
            _userRepository.SaveChanges();

            return emailSender.EmailMessage(NewUser.Email, "registration Email", $"Hi {NewUser.Name} " +
                  $"  {NewUser.Email}   " +
                  "                                                                " + $"  {NewUser.Password}   " +
                  " is your New Password and Email  .");


        }

        public ApiResponse<object?> Update(UpdateUserDto UserRequest)
        {
            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }

            var NewUserId = Guid.NewGuid();


            var user = _userRepository.GetUserById(UserRequest.Id);
            if (user is null)
            {
                return new ApiResponse<object?>(
                      data: null,
                      status: StatusCodes.Status404NotFound,
                      message: [" User Id dosen`t exist"]);
            }

            if (user.Email != UserRequest.Email)
                if (_userRepository.IsEmailExists(UserRequest.Email))
                {
                    return new ApiResponse<object?>(
                           data: null,
                           status: StatusCodes.Status400BadRequest,
                           message: [" Email Aleardy exist"]);
                }



            var Role = _RoleRepository.GetRoleByName(UserRequest.Role);

            if (Role is null)
            {
                return new ApiResponse<object?>(
                 data: null,
                 status: StatusCodes.Status404NotFound,
                 message: [" Role dosen`t exist"]);
            }

            user.Name = string.IsNullOrEmpty(UserRequest.Name) ? user.Name : UserRequest.Name;

            user.Email = string.IsNullOrEmpty(UserRequest.Email) ? user.Email : UserRequest.Email;

            user.UserName = string.IsNullOrEmpty(UserRequest.UserName) ? user.UserName : UserRequest.UserName;

            user.Phone = string.IsNullOrEmpty(UserRequest.Phone) ? user.Phone : UserRequest.Phone;

            user.RoleId = string.IsNullOrEmpty(Role.Id) ? user.RoleId : Role.Id;

            user.IsActive = UserRequest.IsActive;
            user.Password = string.IsNullOrEmpty(UserRequest.Password) ? user.Password : UserRequest.Password;
            user.ProfileImage = string.IsNullOrEmpty(UserRequest.ProfileImage) ? user.ProfileImage : UserRequest.ProfileImage;

            user.LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            user.LastUpdatedDate = DateTime.Now;



            _userRepository.UpdateUser(user);
            _userRepository.SaveChanges();

            return new ApiResponse<object?>(
                      data: null,
                      status: StatusCodes.Status200OK,
                      message: new List<string> { "You have Updated the user Successfuly" });
        }

        public ApiResponse<object?> DeletePermanent(string UserId)
        {

            var user = _userRepository.GetUserById(UserId);

            if (user is null)
            {
                return new ApiResponse<object?>(
                            data: null,
                            status: StatusCodes.Status404NotFound,
                            message: [" user id dosen`t exist"]);
            }

            _userRepository.DeleteUser(user.Id);
            _userRepository.SaveChanges();


            return new ApiResponse<object?>(
              data: null,
              status: StatusCodes.Status200OK,
              message: new List<string> { $"You have removed user with id :{UserId} successfly" });

        }

        public ApiResponse<object?> Delete(string UserId)
        {

            var user = _userRepository.GetUserById(UserId);

            if (user is null)
            {
                return new ApiResponse<object?>(
                     data: null,
                     status: StatusCodes.Status400BadRequest,
                     message: [" user id dosen`t exist"]);
            }
            user.IsActive = false;
            user.LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            user.LastUpdatedDate = DateTime.Now;
            _userRepository.SaveChanges();


            return new ApiResponse<object?>(
                   data: null,
                   status: StatusCodes.Status200OK,
                   message: new List<string> { $"You have removed user with id :{UserId} successfly" });

        }

        public ApiResponse<object?> GetList(UsersFilter? filterParameter, int skip, int take, string? sortBy = "Name", string? sortOrder = "asc")
        {


            var validationErrors = filterParameter.Validation();

            if (validationErrors.Count > 0)
            {
                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status404NotFound,
                        message: ["No users found with the given parameters." + validationErrors.ToList()]);
            }

            var users = _userRepository.GetUsers(filterParameter, sortBy, sortOrder).Select(user => new GetUserDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Password = user.Password,
                ProfileImage = user.ProfileImage,
                IsActive = user.IsActive,
                Role = user.Role.Name.ToString(),
                CreatedDate = user.CreatedDate,
                LastUpdatedDate = user.LastUpdatedDate,
                CreatedbyId = user.CreatedbyId,
                CreatedByName = user.CreatedBy.Name,
                LastUpdatedbyId = user.LastUpdatedbyId,
                LastUpdatedByName = user.LastUpdatedBy.Name
            });

            var totalRecords = users.Count();

            if (totalRecords == 0)
            {
                return new ApiResponse<object?>(
                       data: null,
                       status: StatusCodes.Status404NotFound,
                       message: ["No users found with the given parameters."]);
            }

            var userList = users.Skip((skip - 1) * take).Take(take).ToList();

            var metaData = new MetaData<UsersFilter>(_page: skip, _PerPage: take, _TotalItems: totalRecords, _Filters: filterParameter, _SortBy: sortBy, sortOrder);

            return new ApiResponse<object?>(
                    data: [userList, metaData],
                    status: StatusCodes.Status200OK,
                    message: ["Users fetched successfully."])
            { };
        }

        public ApiResponse<object?> Search(string parameter, int skip, int take)
        {


            var user = _userRepository.GetUsersByParameter(parameter).Select(user => new GetUserDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Password = user.Password,
                ProfileImage = user.ProfileImage,
                IsActive = user.IsActive,
                Role = user.Role.Name.ToString(),
                CreatedDate = user.CreatedDate,
                LastUpdatedDate = user.LastUpdatedDate,
                CreatedbyId = user.CreatedbyId,
                CreatedByName = user.CreatedBy.Name,
                LastUpdatedbyId = user.LastUpdatedbyId,
                LastUpdatedByName = user.LastUpdatedBy.Name
            });


            var totalRecords = user.Count();

            if (totalRecords <= 0)
            {
                return new ApiResponse<object?>(
                            data: null,
                            status: StatusCodes.Status400BadRequest,
                            message: [$"user parameter: {parameter} dosen`t exist"]);
            }

            var userList = user.Skip((skip - 1) * take).Take(take).ToList();
            var metaData = new MetaData<string>(_page: skip, _PerPage: take, _TotalItems: totalRecords, _Filters: parameter);
            return new ApiResponse<object?>(
            data: [userList, metaData],
                    status: StatusCodes.Status200OK,
                    message: ["Users fetched successfully."])
            { };

        }

        public ApiResponse<object?> GetCurrent()
        {


            var userToken = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userToken == null)
            {
                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status404NotFound,
                        message: ["User is not authenticated"]);
            }

            var user = _userRepository.GetUserById(userToken.Value);
            var userDto = new GetUserDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Password = user.Password,
                ProfileImage = user.ProfileImage,
                IsActive = user.IsActive,
                Role = user.Role.Name.ToString(),
                CreatedDate = user.CreatedDate,
                LastUpdatedDate = user.LastUpdatedDate,
                CreatedbyId = user.CreatedbyId,
                CreatedByName = user.CreatedBy.Name,
                LastUpdatedbyId = user.LastUpdatedbyId,
                LastUpdatedByName = user.LastUpdatedBy.Name
            };


            return new ApiResponse<object?>(
                           data: new List<GetUserDto> { userDto },
                           status: StatusCodes.Status200OK,
                           message: ["User data fetched Successfully"]);

        }

        public ApiResponse<object?> GetById(string UserId)
        {

            var user = _userRepository.GetUserById(UserId);

            if (user is null)
            {

                return new ApiResponse<object?>(
                       data: null,
                       status: StatusCodes.Status400BadRequest,
                       message: ["user id dosen`t exist"]);
            }


            var userDto = new GetUserDto
            {
                Id = user.Id,
                Name = user.Name,
                UserName = user.UserName,
                Email = user.Email,
                Phone = user.Phone,
                Password = user.Password,
                ProfileImage = user.ProfileImage,
                IsActive = user.IsActive,
                Role = user.Role.Name.ToString(),
                CreatedDate = user.CreatedDate,
                LastUpdatedDate = user.LastUpdatedDate,
                CreatedbyId = user.CreatedbyId,
                CreatedByName = user.CreatedBy.Name,
                LastUpdatedbyId = user.LastUpdatedbyId,
                LastUpdatedByName = user.LastUpdatedBy.Name
            };


            return new ApiResponse<object?>(
                 data: new List<GetUserDto> { userDto },
                 status: StatusCodes.Status200OK,
                 message: [$"user data with Id {UserId} fetched successfully"]);

        }



    }
}
