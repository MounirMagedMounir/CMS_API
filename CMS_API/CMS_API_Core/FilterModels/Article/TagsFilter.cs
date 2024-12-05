using CMS_API_Core.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace CMS_API_Core.FilterModels.Article
{
    public class TagsFilter : BaseFilterEntity
    {
        public string? Name { get; set; }
        public override List<string> Validation()
        {

            ModelValidations validations = new();

            validations.IsString(Name, "Name", "Name Is Not a String")
              ;


            return validations.GetValidations();
        }
    }
}
