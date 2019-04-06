using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.Domain.Datatypes;

namespace Integratieproject1.DAL.Repositories
{
    public class DataTypeRepository
    {
        private readonly CityOfIdeasDbContext _ctx;

        public DataTypeRepository(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
      
            _ctx = unitOfWork.Ctx;
        }

        #region Locations
        
        public IEnumerable<Location> GetLocations()
        {
            return _ctx.Locations.AsEnumerable();
        }
        
        #endregion
        
        #region Addresses
        
        public Address GetAddress(int addressId)
        {
           return _ctx.Addresses.Find(addressId);
        }

        public IEnumerable<Address> GetAddresses()
        {
            return _ctx.Addresses.AsEnumerable();
        }

        #endregion

        #region Images

        public Image CreateImage(Image image)
        {
            _ctx.Images.Add(image);
            _ctx.SaveChanges();
            return image;
        }

        public Image UpdateImage(Image newImage)
        {
            Image image = _ctx.Images.Find(newImage.ImageId);
            image.ImageName = newImage.ImageName;
            image.ImagePath = newImage.ImagePath;
            image.Idea = newImage.Idea;
            
            _ctx.SaveChanges();
            return image;
        }

        public IEnumerable<Image> ReadImagesOfIdea(int ideaId)
        {
            return _ctx.Images.Where(i => i.Idea.IdeaId == ideaId).AsEnumerable();
        }

        #endregion
    }
}