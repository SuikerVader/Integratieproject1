using System;
using System.Collections.Generic;
using System.Linq;
using Integratieproject1.DAL.Interfaces;
using Integratieproject1.Domain.Datatypes;
using Integratieproject1.Domain.Ideations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Integratieproject1.DAL.Repositories
{
    public class DataTypeRepository : IDataTypeRepository
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

        #region Position

        public Position CreatePosition(Position position)
        {
            _ctx.Positions.Add(position);
            _ctx.SaveChanges();
            return position;
        }

        public void DeletePosition(Position position)
        {
            _ctx.Positions.Remove(position);
            _ctx.SaveChanges();
        }

        public Position GetPosition(int positionId)
        {
           return  _ctx.Positions.Find(positionId);
        }
        public void UpdatePosition(Position position)
        {
            _ctx.Positions.Update(position);
            _ctx.SaveChanges();
        }

        #endregion

       
    }
}