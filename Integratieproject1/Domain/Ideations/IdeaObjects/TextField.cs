using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Ideations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Integratieproject1.Domain.Datatypes
{
    public class TextField : IdeaObject
    {

        public string Text { get; set; }
    }
}