using CMS_API_Application.Dto.Article;
using CMS_API_Application.Interfaces.Servises.Article;
using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.FilterModels.Article;
using CMS_API_Core.helper.Response;
using CMS_API_Core.Interfaces.Repository.Article;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CMS_API.Services.Article
{
    public class CommentService(IHttpContextAccessor _httpContextAccessor,
        ICommentRepository _commentRepository,
        IArticleRepository _articleRepository
        ) : ICommentService
    {

        public ApiResponse<object?> Add(AddCommentDto UserRequest)
        {

            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }

            if (!_articleRepository.IsArticleExists(UserRequest.ArticleId))
            {

                return new ApiResponse<object?>(
                           data: null,
                           status: StatusCodes.Status404NotFound,
                           message: [$"Article id : {UserRequest.ArticleId} doesn't exist"]);
            }
            if (UserRequest.ParentId != null && !_commentRepository.IsCommentExists(UserRequest.ParentId))
            {

                return new ApiResponse<object?>(
                             data: null,
                             status: StatusCodes.Status404NotFound,
                             message: [$"Parent Comment id : {UserRequest.ParentId} doesn't exist"]);
            }


            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var newComment = new Comment
            {

                Id = Guid.NewGuid().ToString(),
                CreatedbyId = userId,
                CreatedDate = DateTime.Now,
                LastUpdatedbyId = userId,
                LastUpdatedDate = DateTime.Now,
                IsApproved = UserRequest.IsApproved,
                ApprovedDate = UserRequest.IsApproved ? DateTime.Now : null,
                ApprovedById = UserRequest.IsApproved ? userId : null,
                ArticleId = UserRequest.ArticleId,
                ParentId = UserRequest.ParentId,
                Content = UserRequest.Content

            };

            _commentRepository.AddComment(newComment);
            _commentRepository.SaveChanges();

            return new ApiResponse<object?>(
                        data: null,
                        status: StatusCodes.Status201Created,
                        message: ["Comment Added successfully"]);
        }

        public ApiResponse<object?> UpdateStat(UpdateCommentDto UserRequest)
        {

            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);
            }

            if (!_commentRepository.IsCommentExists(UserRequest.Id))
            {
                return new ApiResponse<object?>(
                          data: null,
                          status: StatusCodes.Status404NotFound,
                          message: [$"Comment id : {UserRequest.Id} doesn't exist"]);
            }



            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            var oldComment = _commentRepository.GetComment(UserRequest.Id);

            oldComment.LastUpdatedbyId = userId;
            oldComment.LastUpdatedDate = DateTime.Now;
            oldComment.IsApproved = UserRequest.IsApproved;
            oldComment.ApprovedDate = UserRequest.IsApproved ? DateTime.Now : null;
            oldComment.ApprovedById = UserRequest.IsApproved ? userId : null;

            _commentRepository.SaveChanges();


            return new ApiResponse<object?>(
                          data: null,
                          status: StatusCodes.Status200OK,
                          message: ["Comment Updated successfully"]);
        }

        public ApiResponse<object?> Delete(string id)
        {


            if (!_commentRepository.IsCommentExists(id))
            {
                return new ApiResponse<object?>(
                            data: null,
                            status: StatusCodes.Status404NotFound,
                            message: [$"Comment id : {id} doesn't exist"]);
            }

            _commentRepository.DeleteComment(id);
            _commentRepository.SaveChanges();


            return new ApiResponse<object?>(
                         data: null,
                         status: StatusCodes.Status200OK,
                         message: ["Comment and its hierarchy deleted successfully"]);
        }

        public ApiResponse<object?> GetByArticleId(string articleId)
        {


            if (!_articleRepository.IsArticleExists(articleId))
            {

                return new ApiResponse<object?>(
                           data: null,
                           status: StatusCodes.Status404NotFound,
                           message: [$"Article id : {articleId} doesn't exist"]);
            }



            return new ApiResponse<object?>(
                    data: BuildCommentHierarchy(articleId),
                    status: StatusCodes.Status200OK,
                    message: ["Comments fetched successfully."]);
        }

        public ApiResponse<object?> GetById(string Id)
        {

            if (!_commentRepository.IsCommentExists(Id))
            {
                return new ApiResponse<object?>(
            data: null,
            status: StatusCodes.Status404NotFound,
            message: [$"Comment id : {Id} doesn't exist"]);
            }
            var comment = _commentRepository.GetComment(Id);

            var commnentDto = new GetCommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                ParentId = comment.ParentId,
                CreatedbyId = comment.CreatedbyId,
                CreatedDate = comment.CreatedDate,
                ArticleId = comment.ArticleId,
                IsApproved = comment.IsApproved,
                ApprovedDate = comment.ApprovedDate,
                ApprovedById = comment.ApprovedById,
                LastUpdatedbyId = comment.LastUpdatedbyId,
                LastUpdatedDate = comment.LastUpdatedDate,

                CreatedByName = comment.CreatedBy.UserName,
                LastUpdatedByName = comment.LastUpdatedBy.UserName,

            };


            return new ApiResponse<object?>(
                      data: [commnentDto],
                      status: StatusCodes.Status200OK,
                      message: ["Comments fetched successfully."]);
        }

        public ApiResponse<object?> GetList(CommentsFilter filterParameter, int skip, int take, string sortBy = "Name", string sortOrder = "asc")
        {

            var validationErrors = filterParameter.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }

            var comments = _commentRepository.GetComments(filterParameter, sortBy, sortOrder).Select(comment => new GetCommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                ParentId = comment.ParentId,
                CreatedbyId = comment.CreatedbyId,
                CreatedDate = comment.CreatedDate,
                ArticleId = comment.ArticleId,
                IsApproved = comment.IsApproved,
                ApprovedDate = comment.ApprovedDate,
                ApprovedById = comment.ApprovedById,
                LastUpdatedbyId = comment.LastUpdatedbyId,
                LastUpdatedDate = comment.LastUpdatedDate,
                CreatedByName = comment.CreatedBy.UserName,
                LastUpdatedByName = comment.LastUpdatedBy.UserName,

            });


            var totalRecords = comments.Count();

            if (totalRecords == 0)
            {
                return new ApiResponse<object?>(
          data: null,
          status: StatusCodes.Status404NotFound,
          message: ["No Comment found with the given parameters."]);
            }

            var commentList = comments.Skip((skip - 1) * take).Take(take).ToList();

            var metaData = new MetaData<CommentsFilter>(_page: skip, _PerPage: take, _TotalItems: totalRecords, _Filters: filterParameter, _SortBy: sortBy, sortOrder);

            return new ApiResponse<object?>(
                    data: [commentList, metaData],
                    status: StatusCodes.Status200OK,
                    message: ["Comments fetched successfully."])
            { };
        }

        public List<GetArticleCommentDto> BuildCommentHierarchy(string articleId)
        {

            var comments = _commentRepository.GetCommentsByArticleIdAsync(articleId);

            // Step 2.1: Map each Comment to a CommentDto
            var commentDtos = comments.Select(c => new GetArticleCommentDto
            {
                Id = c.Id,
                Content = c.Content,
                ParentId = c.ParentId,
                CreatedbyId = c.CreatedbyId,
                CreatedDate = c.CreatedDate,
                CreatedByName = c.CreatedBy.UserName,
                LastUpdatedByName = c.LastUpdatedBy.UserName,
                LastUpdatedDate = c.LastUpdatedDate,
                LastUpdatedbyId = c.LastUpdatedbyId,
            }).ToList();

            // Step 2.2: Group CommentDtos by Id
            var commentDict = commentDtos.ToDictionary(c => c.Id);
            var rootComments = new List<GetArticleCommentDto>();

            foreach (var dto in commentDtos)
            {
                if (dto.ParentId == null)
                {
                    // Root comment
                    rootComments.Add(dto);
                }
                else if (commentDict.TryGetValue(dto.ParentId, out var parentDto))
                {
                    // Add as a child to the parent comment
                    parentDto.Children.Add(dto);
                }
            }
            return rootComments;
        }


    }
}
