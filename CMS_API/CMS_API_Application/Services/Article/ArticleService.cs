using CMS_API_Application.Dto.Article;
using CMS_API_Application.Interfaces.Servises.Article;
using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.enums;
using CMS_API_Core.FilterModels.Article;
using CMS_API_Core.helper.Response;
using CMS_API_Core.Interfaces.Repository;
using CMS_API_Core.Interfaces.Repository.Article;
using CMS_API_Core.Interfaces.Repository.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace CMS_API_Application.Services.Article
{
    public class ArticleService(IHttpContextAccessor _httpContextAccessor,
        IUserRepository _userRepository,
        IArticleRepository _articleRepository,
        ITagRepository _tagRepository,
        ITagArticleRepository _tagArticleRepository,
        IArticleContributorRepository _articleContributorRepository,
        IRolePermissionRepository _rolePermissionRepository,
        ICommentService _commentService) : IArticleService
    {

        public ApiResponse<object?> Create(CreateArticleDto UserRequest)
        {

            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }

            var NewArticleId = Guid.NewGuid().ToString();

            if (!Enum.IsDefined(typeof(ArticleStatus), UserRequest.Status))
            {

                return new ApiResponse<object?>(
              data: null,
              status: StatusCodes.Status404NotFound,
              message: [$" Status : {UserRequest.Status} doesn't exist"]);
            }
            var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;


            if (!_rolePermissionRepository.HasPermission(userRole, "Article_Create_" + UserRequest.Status.ToString()))
            {
                return new ApiResponse<object?>(
                          data: null,
                          status: StatusCodes.Status403Forbidden,
                          message: [$"You don't have permission to create Article with status : {UserRequest.Status}"]);
            }


            var article = new ContentArticle
            {
                Id = NewArticleId,
                Name = UserRequest.Name,
                Title = UserRequest.Title,
                PublishDate = UserRequest.Status == ArticleStatus.Publish ? DateTime.Now : null,
                Status = UserRequest.Status,
                Description = UserRequest.Description,
                Content = UserRequest.Content,
                Image = UserRequest.Image,
                Video = UserRequest.Video,
                LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value,
                LastUpdatedDate = DateTime.Now,
                CreatedDate = DateTime.Now,
                CreatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value
            };

            _articleRepository.CreateArticle(article);



            if (UserRequest.Tags != null)
            {
                var allTags = _tagRepository.GetAllTags();

                foreach (var tag in UserRequest.Tags)
                {

                    if (allTags.Any(p => p.Name == tag.Name))
                    {
                        var newTagArticle = new TagArticle
                        {
                            Id = Guid.NewGuid().ToString(),
                            ArticleId = NewArticleId,
                            TagId = allTags.FirstOrDefault(p => p.Name == tag.Name).Id

                        };

                        _tagArticleRepository.AddTagArticle(newTagArticle);
                    }
                    else
                    {
                        return new ApiResponse<object?>(
                         data: null,
                         status: StatusCodes.Status404NotFound,
                         message: [$" Tag Name : {tag.Name} dosen`t exist"]);
                    }
                }
            }




            if (UserRequest.Contributors != null)
            {

                foreach (var contributors in UserRequest.Contributors)
                {

                    if (!Enum.IsDefined(typeof(ContributorRole), contributors.ContributorRole))
                    {

                        return new ApiResponse<object?>(
                                  data: null,
                                  status: StatusCodes.Status404NotFound,
                                  message: [$" Contributor Role : {contributors.ContributorRole} doesn't exist"]);
                    }

                    if (_userRepository.GetUserById(contributors.UserId) is not null)
                    {
                        var newArticleContributor = new ArticleContributor
                        {
                            Id = Guid.NewGuid().ToString(),
                            ArticleId = NewArticleId,
                            UserId = contributors.UserId,
                            ContributorRole = contributors.ContributorRole

                        };


                        _articleContributorRepository.AddArticleContributor(newArticleContributor);
                    }
                    else
                    {

                        return new ApiResponse<object?>(
                                      data: null,
                                      status: StatusCodes.Status404NotFound,
                                      message: [$" contributors Id : {contributors.UserId} dosen`t exist"]);
                    }
                }
            }

            _articleRepository.SaveChanges();


            return new ApiResponse<object?>(
              data: null,
              status: StatusCodes.Status201Created,
              message: ["You have Created a Article successfly"]);
        }
        public ApiResponse<object?> Update(UpdateArticleDto UserRequest)
        {


            var validationErrors = UserRequest.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }


            var Article = _articleRepository.GetArticleById(UserRequest.Id);

            if (Article == null)
            {

                return new ApiResponse<object?>(
                           data: null,
                           status: StatusCodes.Status404NotFound,
                           message: [$"Article id : {UserRequest.Id} doesn't exist"]);
            }

            if (!Enum.IsDefined(typeof(ArticleStatus), UserRequest.Status))
            {

                return new ApiResponse<object?>(
                           data: null,
                           status: StatusCodes.Status404NotFound,
                           message: [$" Status : {UserRequest.Status} doesn't exist"]);
            }

            var userRole = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;


            if (!_rolePermissionRepository.HasPermission(userRole, "Article_Update_" + UserRequest.Status.ToString()))
            {
                return new ApiResponse<object?>(
                                data: null,
                                status: StatusCodes.Status403Forbidden,
                                message: [$"You don't have permission to Update Article with status : {UserRequest.Status}"]);

            }

            Article.Name = string.IsNullOrEmpty(UserRequest.Name) ? Article.Name : UserRequest.Name;
            Article.Title = string.IsNullOrEmpty(UserRequest.Title) ? Article.Title : UserRequest.Title;
            Article.Status = UserRequest.Status;
            Article.Description = string.IsNullOrEmpty(UserRequest.Description) ? Article.Description : UserRequest.Description;
            Article.Content = string.IsNullOrEmpty(UserRequest.Content) ? Article.Content : UserRequest.Content;
            Article.Image = string.IsNullOrEmpty(UserRequest.Image) ? Article.Image : UserRequest.Image;
            Article.Video = string.IsNullOrEmpty(UserRequest.Video) ? Article.Video : UserRequest.Video;
            Article.PublishDate = UserRequest.Status == ArticleStatus.Publish ? DateTime.Now : null;

            Article.LastUpdatedDate = DateTime.Now;
            Article.LastUpdatedbyId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            _articleRepository.UpdateArticle(Article);

            if (UserRequest.Tags != null)
            {
                // Fetch all Tags once
                var allTags = _tagRepository.GetAllTags();

                // Fetch current Tag Article
                var currentTagArticle = _tagArticleRepository.GetTagArticles(Article.Id);


                var requestedTagArticle = new List<string>();

                foreach (var tag in UserRequest.Tags)
                {

                    var existingTag = allTags.FirstOrDefault(p => p.Name == tag.Name || p.Id == tag.Id);
                    if (existingTag != null)
                    {
                        requestedTagArticle.Add(existingTag.Id);

                        if (!currentTagArticle.Any(rp => rp.TagId == existingTag.Id))
                        {
                            var newTagArticle = new TagArticle
                            {
                                Id = Guid.NewGuid().ToString(),
                                ArticleId = Article.Id,
                                TagId = existingTag.Id
                            };
                            _tagArticleRepository.AddTagArticle(newTagArticle);
                        }
                    }
                    else
                    {
                        return new ApiResponse<object?>(
                                    data: null,
                                    status: StatusCodes.Status404NotFound,
                                    message: [$"tag Name: {tag.Name} doesn't exist."]);
                    }
                }

                // Remove permissions that exist in the database but are not present in the request
                foreach (var tagArticle in currentTagArticle)
                {
                    if (!requestedTagArticle.Contains(tagArticle.TagId))
                    {
                        _tagArticleRepository.DeleteTagArticle(tagArticle.Id);
                    }
                }
            }




            if (UserRequest.Contributors != null)
            {
                // Fetch current Contributors Article
                var currentArticleContributor = _articleContributorRepository.GetArticleContributorsByArticleId(Article.Id);

                var requestedArticleContributor = new List<string>();

                foreach (var contributors in UserRequest.Contributors)
                {
                    if (!Enum.IsDefined(typeof(ContributorRole), contributors.ContributorRole))
                    {
                        return new ApiResponse<object?>(
                                      data: null,
                                      status: StatusCodes.Status404NotFound,
                                      message: [$" Contributor Role : {contributors.ContributorRole} doesn't exist"]);
                    }

                    var existingContributor = _userRepository.GetUserById(contributors.UserId);
                    if (existingContributor != null)
                    {
                        requestedArticleContributor.Add(existingContributor.Id);

                        if (!currentArticleContributor.Any(rp => rp.UserId == existingContributor.Id))
                        {
                            var newArticleContributor = new ArticleContributor
                            {
                                Id = Guid.NewGuid().ToString(),
                                ArticleId = Article.Id,
                                UserId = existingContributor.Id,
                                ContributorRole = contributors.ContributorRole

                            };
                            _articleContributorRepository.AddArticleContributor(newArticleContributor);
                        }
                        else
                        {

                            var currentContributor = currentArticleContributor.FirstOrDefault(rp => rp.UserId == existingContributor.Id);
                            currentContributor.ContributorRole = contributors.ContributorRole;
                            _articleContributorRepository.UpdateArticleContributor(currentContributor);

                        }
                    }
                    else
                    {

                        return new ApiResponse<object?>(
                                     data: null,
                                     status: StatusCodes.Status404NotFound,
                                     message: [$"Contributor Id: {contributors.UserId} doesn't exist."]);
                    }

                }

                // Remove Contributor that exist in the database but are not present in the request
                foreach (var articleContributor in currentArticleContributor)
                {
                    if (!requestedArticleContributor.Contains(articleContributor.UserId))
                    {
                        _articleContributorRepository.DeleteArticleContributor(articleContributor.Id);
                    }
                }

            }



            // Save the changes to the database
            _articleRepository.SaveChanges();

            return new ApiResponse<object?>(
                         data: null,
                         status: StatusCodes.Status200OK,
                         message: ["You have Updated the Article Successfuly"]);

        }
        public ApiResponse<object?> DeleteById(string ArticleId)
        {
            var article = _articleRepository.GetArticleById(ArticleId);


            if (article is null)
            {

                return new ApiResponse<object?>(
                           data: null,
                           status: StatusCodes.Status404NotFound,
                           message: ["Article id dosen`t exist"]);
            }

            _articleRepository.DeleteArticle(article.Id);
            _articleRepository.SaveChanges();



            return new ApiResponse<object?>(
                                 data: null,
                                 status: StatusCodes.Status200OK,
                                 message: [$"You have removed Article with id :{ArticleId} successfly"]);

        }
        public ApiResponse<object?> GetList(ArticleFilter filterParameter, int skip, int take, string sortBy = "Name", string sortOrder = "asc")
        {

            var validationErrors = filterParameter.Validation();
            if (validationErrors.Count > 0)
            {

                return new ApiResponse<object?>(
                    data: null,
                    status: StatusCodes.Status400BadRequest,
                    message: validationErrors);

            }

            var articles = _articleRepository.GetArticles(filterParameter, sortBy, sortOrder).Select(article => new GetArticleDto
            {

                Id = article.Id,
                Name = article.Name,
                Title = article.Title,
                PublishDate = article.PublishDate,
                Status = article.Status,
                ViewsCount = article.ViewsCount,
                Description = article.Description,
                Content = article.Content,
                Image = article.Image,
                Video = article.Video,
                Comments = _commentService.BuildCommentHierarchy(article.Id),
                Tags = article.Tags.Select(t => new GetTagDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                 ,
                Contributors = _articleContributorRepository.GetArticleContributorsByArticleId(article.Id).Select(contributor => new ArticleContributorDto
                {
                    Id = contributor.Id,
                    UserId = contributor.UserId,
                    UserName = contributor.User.UserName,
                    ContributorRole = contributor.ContributorRole
                }).ToList(),
                LastUpdatedbyId = article.LastUpdatedbyId,
                LastUpdatedDate = article.LastUpdatedDate,
                LastUpdatedByName = article.LastUpdatedBy.UserName,
                CreatedbyId = article.CreatedbyId,
                CreatedDate = article.CreatedDate,
                CreatedByName = article.CreatedBy.UserName,

            });





            var totalRecords = articles.Count();


            if (totalRecords == 0)
            {

                return new ApiResponse<object?>(
                                data: null,
                                status: StatusCodes.Status404NotFound,
                                message: ["No Article found with the given parameters."]);
            }


            var articleList = articles.Skip((skip - 1) * take).Take(take).ToList();

            var metaData = new MetaData<ArticleFilter>(_page: skip, _PerPage: take, _TotalItems: totalRecords, _Filters: filterParameter, _SortBy: sortBy, sortOrder);

            return new ApiResponse<object?>(
                    data: [articleList, metaData],
                    status: StatusCodes.Status200OK,
                    message: ["Articles fetched successfully."])
            { };

        }
        public ApiResponse<object?> GetById(string articleId)
        {

            var article = _articleRepository.GetArticleById(articleId);


            if (article is null)
            {
                return new ApiResponse<object?>(
          data: null,
          status: StatusCodes.Status404NotFound,
          message: [$"No article found with Id {articleId}"]);
            }

            var articleDto = new GetArticleDto
            {

                Id = article.Id,
                Name = article.Name,
                Title = article.Title,
                PublishDate = article.PublishDate,
                Status = article.Status,
                ViewsCount = article.ViewsCount,
                Description = article.Description,
                Content = article.Content,
                Image = article.Image,
                Video = article.Video,
                Comments = _commentService.BuildCommentHierarchy(article.Id),
                Tags = article.Tags.Select(t => new GetTagDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                 ,
                Contributors = _articleContributorRepository.GetArticleContributorsByArticleId(article.Id).Select(contributor => new ArticleContributorDto
                {
                    Id = contributor.Id,
                    UserId = contributor.UserId,
                    UserName = contributor.User.UserName,
                    ContributorRole = contributor.ContributorRole
                }).ToList(),
                LastUpdatedbyId = article.LastUpdatedbyId,
                LastUpdatedDate = article.LastUpdatedDate,
                LastUpdatedByName = article.LastUpdatedBy.UserName,
                CreatedbyId = article.CreatedbyId,
                CreatedDate = article.CreatedDate,
                CreatedByName = article.CreatedBy.UserName,

            };


            return new ApiResponse<object?>(
              data: [articleDto],
              status: StatusCodes.Status200OK,
              message: ["article fetched successfully."]);

        }
        public ApiResponse<object?> GetListByUserId(string UserId, int skip, int take, string sortBy = "Name", string sortOrder = "asc")
        {
            var articles = _articleRepository.GetArticlesByUserId(UserId, sortBy, sortOrder).Select(article => new GetArticleDto
            {

                Id = article.Id,
                Name = article.Name,
                Title = article.Title,
                PublishDate = article.PublishDate,
                Status = article.Status,
                ViewsCount = article.ViewsCount,
                Description = article.Description,
                Content = article.Content,
                Image = article.Image,
                Video = article.Video,
                Comments = _commentService.BuildCommentHierarchy(article.Id),
                Tags = article.Tags.Select(t => new GetTagDto
                {
                    Id = t.Id,
                    Name = t.Name
                })
                 ,
                Contributors = _articleContributorRepository.GetArticleContributorsByArticleId(article.Id).Select(contributor => new ArticleContributorDto
                {
                    Id = contributor.Id,
                    UserId = contributor.UserId,
                    UserName = contributor.User.UserName,
                    ContributorRole = contributor.ContributorRole
                }).ToList(),
                LastUpdatedbyId = article.LastUpdatedbyId,
                LastUpdatedDate = article.LastUpdatedDate,
                LastUpdatedByName = article.LastUpdatedBy.UserName,
                CreatedbyId = article.CreatedbyId,
                CreatedDate = article.CreatedDate,
                CreatedByName = article.CreatedBy.UserName,

            });

            var totalRecords = articles.Count();

            if (totalRecords == 0)
            {
                return new ApiResponse<object?>(
                          data: null,
                          status: StatusCodes.Status404NotFound,
                          message: [$"No Article found with the User Id {UserId}."]);
            }

            var articleList = articles.Skip((skip - 1) * take).Take(take).ToList();

            var metaData = new MetaData<object>(_page: skip, _PerPage: take, _TotalItems: totalRecords, _Filters: null, _SortBy: sortBy, sortOrder);

            return new ApiResponse<object?>(
                    data: [articleList, metaData],
                    status: StatusCodes.Status200OK,
                    message: ["Artecles fetched successfully."])
            { };

        }
        public ApiResponse<object?> GetCurrentUser(int skip, int take, string sortBy = "Name", string sortOrder = "asc")
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return GetListByUserId(userId, skip, take, sortBy, sortOrder);

        }
        public ApiResponse<object?> UpdateViewCount(string ArticleId)
        {
            var article = _articleRepository.GetArticleById(ArticleId);

            if (article is null)
            {
                return new ApiResponse<object?>(
                          data: null,
                          status: StatusCodes.Status404NotFound,
                          message: ["Article id dosen`t exist"]);
            }

            article.ViewsCount += 1;

            _articleRepository.UpdateArticle(article);
            _articleRepository.SaveChanges();

            return new ApiResponse<object?>(
                      data: null,
                      status: StatusCodes.Status200OK,
                      message: [$"You have Updated the Article View Count Successfuly"]);

        }
    }
}
