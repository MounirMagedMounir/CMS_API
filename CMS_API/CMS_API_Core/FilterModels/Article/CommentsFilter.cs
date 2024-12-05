using CMS_API_Core.DomainModels.Article;
using CMS_API_Core.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CMS_API_Core.Validations;
using System.Numerics;

namespace CMS_API_Core.FilterModels.Article
{
    public class CommentsFilter : BaseFilterEntity
    {
        public string? Content { get; set; }
        public bool? IsApproved { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public string? ApprovedById { get; set; }
        public string? ApprovedByName { get; set; }
        public string? ParentId { get; set; }
        public string? ArticleId { get; set; }
        public string? ArticleName { get; set; }
        public override List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsTrueOrFalse(IsApproved, "IsActive", "IsActive must be (true) or (false)")
                       .IsString(ArticleName, "ArticleName", "ArticleName Is Not a String")
                       ;


            return validations.GetValidations();
        }

    }
}
