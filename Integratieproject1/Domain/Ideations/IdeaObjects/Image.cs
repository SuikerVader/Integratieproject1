using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Ideations;

namespace Integratieproject1.Domain.Datatypes
{
    public class Image : IdeaObject
    {

        public string ImageName { get; set; }
        public string ImagePath { get; set; }
        
    }
}