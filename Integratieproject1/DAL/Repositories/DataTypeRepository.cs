using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.Domain.Datatypes;
using Microsoft.EntityFrameworkCore;

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

        

        
    }
}