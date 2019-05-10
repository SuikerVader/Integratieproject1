using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.IoT;
using Integratieproject1.Domain.Users;
using Microsoft.AspNetCore.Identity;

namespace Integratieproject1.Domain.Ideations
{
    public class Idea
    {
        [Key] public int IdeaId { get; set; }
        public Position Position { get; set; }
        public ICollection<IdeaObject> IdeaObjects { get; set; }
        public String Theme { get; set; }
        
        [DefaultValue(false)]
        public Boolean Reported { get; set; }
        [Required] public String Title { get; set; }
        [Required] public CustomUser IdentityUser { get; set; }
        [Required] public Ideation Ideation { get; set; }
        public ICollection<IoTSetup> IoTSetups { get; set; }
        public ICollection<Vote> Votes { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
        public ICollection<IdeaTag> IdeaTags { get; set; }

        public List<Image> GetImages()
        {
            List<Image> images = new List<Image>();
            foreach (var ideaObject in IdeaObjects)
            {
                if (ideaObject.GetType() == typeof(Image))
                {
                    images.Add((Image)ideaObject);
                }
            }

            return images;
        }
        public List<TextField> GetTextFields()
        {
            List<TextField> textFields = new List<TextField>();
            foreach (var ideaObject in IdeaObjects)
            {
                if (ideaObject.GetType() == typeof(TextField))
                {
                    textFields.Add((TextField)ideaObject);
                }
            }

            return textFields;
        }
        public List<Video> GetVideos()
        {
            List<Video> videos = new List<Video>();
            foreach (var ideaObject in IdeaObjects)
            {
                if (ideaObject.GetType() == typeof(Video))
                {
                    videos.Add((Video)ideaObject);
                }
            }

            return videos;
        }

        

    }
}