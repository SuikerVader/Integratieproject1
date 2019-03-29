using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.Domain.Datatypes;

namespace Integratieproject1.DAL.Repositories
{
    public class DataTypeRepostiory
    {
        private readonly CityOfIdeasDbContext ctx;

   
        public DataTypeRepostiory(UnitOfWork unitOfWork)
        {
            if (unitOfWork == null)
                throw new ArgumentNullException("unitOfWork");
      
            ctx = unitOfWork.ctx;
        }

        public Address GetAddress(int addressId)
        {
           return ctx.Addresses.Find(addressId);
        }

        public IEnumerable<Address> GetAddresses()
        {
            return ctx.Addresses.AsEnumerable();
        }

        public IEnumerable<Location> GetLocations()
        {
            return ctx.Locations.AsEnumerable();
        }
    }
}