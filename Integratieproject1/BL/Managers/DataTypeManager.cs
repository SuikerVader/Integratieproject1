using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;

namespace Integratieproject1.BL.Managers
{
    public class DataTypeManager
    {
        private readonly DataTypeRepository _dataTypeRepository;
        private readonly UnitOfWorkManager _unitOfWorkManager;
        private readonly IdeationsManager _ideationsManager;

        public DataTypeManager()
        {
            _unitOfWorkManager = new UnitOfWorkManager();
            _dataTypeRepository = new DataTypeRepository(_unitOfWorkManager.UnitOfWork);
            _ideationsManager = new IdeationsManager(_unitOfWorkManager);
        }
        
        public DataTypeManager( UnitOfWorkManager unitOfWorkManager)
        {
            if (unitOfWorkManager == null)
                throw new ArgumentNullException(nameof(unitOfWorkManager));

            _unitOfWorkManager = unitOfWorkManager;
            _dataTypeRepository = new DataTypeRepository(_unitOfWorkManager.UnitOfWork);
        }

        #region Locations
        
        public Location CheckLocation(Location toCheckLocation)
        {
            IList<Location> locations = _dataTypeRepository.GetLocations().ToList();
            Location returnLocation = toCheckLocation;
            Address address = CheckAddress(toCheckLocation.Address);
            if (address != null)
            {
                returnLocation.Address = address ;
                                
            }
            foreach (Location location in locations)
            {
                if (toCheckLocation.LocationName.Equals(location.LocationName) 
                    && address == location.Address)
                {
                    return location;
                }
                
            }
            return returnLocation;
        }
        
        #endregion

        #region Addresses
        
        public Address GetAddress(int addressId)
        {
            return _dataTypeRepository.GetAddress(addressId);
        }

        private Address CheckAddress(Address toCheckAddress)
        {
            IList<Address> addresses = _dataTypeRepository.GetAddresses().ToList();
            foreach (Address address in addresses)
            {
                if (toCheckAddress.City.Equals(address.City) 
                    && toCheckAddress.Street.Equals(address.Street) 
                    && toCheckAddress.ZipCode.Equals(address.ZipCode)
                    && toCheckAddress.HouseNr == address.HouseNr)
                {
                    return address;
                }
            }
            return null;
        }
        
        #endregion

        #region Images

        public void CreateImage(string name, string path, int ideaId)
        {
            Idea ideaToAddImageTo = _ideationsManager.GetIdea(ideaId);
            
            // Create image
            Image image = new Image
            {
                ImageName = name, 
                ImagePath = path,
                Idea = ideaToAddImageTo
            };
            
            // Add image to idea
            var images = GetImages(ideaId);
            ideaToAddImageTo.Images = images != null ? images.ToList() : new List<Image>();
            ideaToAddImageTo.Images.Add(image);
            
            // Save in DB
            _dataTypeRepository.CreateImage(image);
            _ideationsManager.ChangeIdea(ideaToAddImageTo);
            _unitOfWorkManager.Save();
        }

        public void UpdateImage(Image image)
        {
            _dataTypeRepository.UpdateImage(image);
        }

        public IEnumerable<Image> GetImages(int ideaId)
        {
            return _dataTypeRepository.ReadImagesOfIdea(ideaId);
        }

        #endregion
    }
}