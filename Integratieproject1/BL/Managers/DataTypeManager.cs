using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.BL.Interfaces;
using Integratieproject1.DAL.Repositories;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;

namespace Integratieproject1.BL.Managers
{
    public class DataTypeManager : IDataTypeManager
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
        
        // Checks if location already exists in database
        // Returns existing location if true
        // Returns new location if false
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
        
        // Returns the address based on the ID
        public Address GetAddress(int addressId)
        {
            return _dataTypeRepository.GetAddress(addressId);
        }

        // Checks if address already exists in database
        // Returns existing address if true
        // Returns new address if false
        public Address CheckAddress(Address toCheckAddress)
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

        #region Position
        
        // Creates a position in the database based on given position
        // Returns newly created position
        public Position CreatePosition(Position position)
        {
           Position returnPosition = _dataTypeRepository.CreatePosition(position);
            _unitOfWorkManager.Save();
            return returnPosition;
        }

        // Deletes position from database based on ID
        public void DeletePosition(int positionId)
        {
            Position position = GetPosition(positionId);
            _dataTypeRepository.DeletePosition(position);
            _unitOfWorkManager.Save();
        }

        // Updates the values of position based on new position and ID
        // Returns the updated position
        public Position EditPosition(Position position, int positionId)
        {
            Position returnPosition = GetPosition(positionId);
            returnPosition.Lat = position.Lat;
            returnPosition.Lng = position.Lng;
            _dataTypeRepository.UpdatePosition(returnPosition);
            _unitOfWorkManager.Save();
            return returnPosition;
        }

        // Returns the position based on given ID
        public Position GetPosition(int positionId)
        {
            return _dataTypeRepository.GetPosition(positionId);
        }

        #endregion

        
    }
}