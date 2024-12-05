using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.Interfaces.Repository.Article;
using System.Security.Claims;
using CMS_API_Core.FilterModels.Article;
using Microsoft.AspNetCore.Http;
using CMS_API_Application.Dto.Article;
using CMS_API_Application.Interfaces.Servises.Article;
using CMS_API_Core.helper.Response;

namespace CMS_API.Services.Article
{
    public class TagService(IHttpContextAccessor _httpContextAccessor,
        ITagRepository _tagRepository
       ) : ITagService
    {


        public ApiResponse<object?> Create(CreateTagDto UserRequest)
        {

            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }

            if (_tagRepository.GetTagByName(UserRequest.Name) is not null)
            {

                return new ApiResponse<object?>(
                      data: null,
                      status: StatusCodes.Status400BadRequest,
                      message: [$"Tag Name : {UserRequest.Name} Already exist"]);
            }


            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var newTag = new Tag
            {

                Id = Guid.NewGuid().ToString(),
                Name = UserRequest.Name,
                CreatedbyId = userId,
                CreatedDate = DateTime.Now,
                LastUpdatedbyId = userId,
                LastUpdatedDate = DateTime.Now,


            };


            _tagRepository.AddTag(newTag);
            _tagRepository.SaveChanges();

            return new ApiResponse<object?>(
           data: null,
           status: StatusCodes.Status201Created,
           message: ["Tag Created successfully"]);
        }

        public ApiResponse<object?> Update(UpdateTagDto UserRequest)
        {


            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }


            if (!_tagRepository.IsTagExists(UserRequest.Id))
            {
                return new ApiResponse<object?>(
                            data: null,
                            status: StatusCodes.Status404NotFound,
                            message: [$"Tag Id : {UserRequest.Id} dosen`t exist"]);
            }

            if (_tagRepository.GetTagByName(UserRequest.Name) is not null)
            {

                return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status400BadRequest,
                        message: [$"Tag Name : {UserRequest.Name} Already exist"]);
            }


            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var oldTag = _tagRepository.GetTagById(UserRequest.Id);



            oldTag.Name = UserRequest.Name;
            oldTag.LastUpdatedbyId = userId;
            oldTag.LastUpdatedDate = DateTime.Now;

            _tagRepository.UpdateTag(oldTag);
            _tagRepository.SaveChanges();

            return new ApiResponse<object?>(
        data: null,
        status: StatusCodes.Status200OK,
        message: ["Tag Updated successfully"]);
        }

        public ApiResponse<object?> Delete(string id)
        {


            if (!_tagRepository.IsTagExists(id))
            {
                return new ApiResponse<object?>(
                             data: null,
                             status: StatusCodes.Status404NotFound,
                             message: [$"Tag Id : {id} dosen`t exist"]);
            }


            _tagRepository.DeleteTag(id);
            _tagRepository.SaveChanges();

            return new ApiResponse<object?>(
                  data: null,
                  status: StatusCodes.Status200OK,
                  message: ["Tag Deleted successfully"]);

        }

        public ApiResponse<object?> DeleteList(List<string> ids)
        {

            var message = new List<string>();
            int index = 0;
            foreach (var TagId in ids)
            {

                if (!_tagRepository.IsTagExists(TagId))
                {

                    return new ApiResponse<object?>(
                              data: null,
                              status: StatusCodes.Status404NotFound,
                              message: [$"Tag Id : {TagId} dosen`t exist"]);
                }



                _tagRepository.DeleteTag(TagId);
                message.Add($"You have removed Tag with id :{TagId} successfly");
            }
            _tagRepository.SaveChanges();

            return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status200OK,
                    message: message);

        }

        public ApiResponse<object?> GetList(TagsFilter filterParameter, int skip, int take, string sortBy, string sortOrder)
        {


            var validationErrors = filterParameter.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }

            var tags = _tagRepository.GetTags(filterParameter, sortBy, sortOrder).Select(t => new GetTagDto
            {

                Id = t.Id,
                Name = t.Name,
                CreatedbyId = t.CreatedbyId,
                CreatedDate = t.CreatedDate,
                CreatedByName = t.CreatedBy.UserName,
                LastUpdatedbyId = t.LastUpdatedbyId,
                LastUpdatedDate = t.LastUpdatedDate,
                LastUpdatedByName = t.LastUpdatedBy.UserName

            });

            var totalRecords = tags.Count();

            if (totalRecords == 0)
            {

                return new ApiResponse<object?>(
                                data: null,
                                status: StatusCodes.Status404NotFound,
                                message: ["No Tag found with the given parameters."]);
            }

            var tagList = tags.Skip((skip - 1) * take).Take(take).ToList();

            var metaData = new MetaData<TagsFilter>(_page: skip, _PerPage: take, _TotalItems: totalRecords, _Filters: filterParameter, _SortBy: sortBy, sortOrder);

            return new ApiResponse<object?>(
                    data: [tagList, metaData],
                    status: StatusCodes.Status200OK,
                    message: ["Tags fetched successfully."]);
        }

        public ApiResponse<object?> GetById(string id)
        {


            var tag = _tagRepository.GetTagById(id);

            if (tag is null)
            {

                return new ApiResponse<object?>(
                         data: null,
                         status: StatusCodes.Status404NotFound,
                         message: [$"Tag Id : {id} dosen`t exist"]);
            }

            var tagDto = new GetTagDto
            {

                Id = tag.Id,
                Name = tag.Name,
                CreatedbyId = tag.CreatedbyId,
                CreatedDate = tag.CreatedDate,
                CreatedByName = tag.CreatedBy.UserName,
                LastUpdatedbyId = tag.LastUpdatedbyId,
                LastUpdatedDate = tag.LastUpdatedDate,
                LastUpdatedByName = tag.LastUpdatedBy.UserName

            };




            return new ApiResponse<object?>(
                         data: [tagDto],
                         status: StatusCodes.Status200OK,
                         message: ["Tags fetched successfully"]);
        }

        public ApiResponse<object?> GetAll(int skip, int take)
        {


            var tags = _tagRepository.GetAllTags().Select(t => new GetTagDto
            {

                Id = t.Id,
                Name = t.Name,
                CreatedbyId = t.CreatedbyId,
                CreatedDate = t.CreatedDate,
                CreatedByName = t.CreatedBy.UserName,
                LastUpdatedbyId = t.LastUpdatedbyId,
                LastUpdatedDate = t.LastUpdatedDate,
                LastUpdatedByName = t.LastUpdatedBy.UserName

            });
            var totalRecords = tags.Count();

            if (totalRecords == 0)
            {

                return new ApiResponse<object?>(
                                data: null,
                                status: StatusCodes.Status404NotFound,
                                message: ["No tag found with the given parameters."]);
            }



            var tagList = tags.Skip((skip - 1) * take).Take(take).ToList();
            var metaData = new MetaData<TagsFilter>(_page: skip, _PerPage: take, _TotalItems: totalRecords, _Filters: null);

            return new ApiResponse<object?>(
            data: [tagList, metaData],
                    status: StatusCodes.Status200OK,
                    message: ["Tags fetched successfully."]);
        }

    }
}
