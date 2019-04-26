

using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Ideations;

namespace Integratieproject1.Domain.Datatypes
{
    public class Video : IdeaObject
    {
        public string Url { get; set; }
    }
}