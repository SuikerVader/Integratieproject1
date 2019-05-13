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
        
        public Position CreatePosition(Position position)
        {
           Position returnPosition = _dataTypeRepository.CreatePosition(position);
            _unitOfWorkManager.Save();
            return returnPosition;
        }
        public void DeletePosition(Position position)
        {
            _dataTypeRepository.DeletePosition(position);
            _unitOfWorkManager.Save();
        }
        public Position EditPosition(Position position, int positionId)
        {
            Position returnPosition = GetPosition(positionId);
            returnPosition.Lat = position.Lat;
            returnPosition.Lng = position.Lng;
            _dataTypeRepository.UpdatePosition(returnPosition);
            _unitOfWorkManager.Save();
            return returnPosition;
        }

        public Position GetPosition(int positionId)
        {
            return _dataTypeRepository.GetPosition(positionId);
        }

        #endregion

        
    }
}