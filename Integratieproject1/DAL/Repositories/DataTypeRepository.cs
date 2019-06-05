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
        
        // Returns enumerable of all locations
        public IEnumerable<Location> GetLocations()
        {
            return _ctx.Locations.AsEnumerable();
        }

        #endregion

        #region Addresses

        // Returns the address based on the ID
        public Address GetAddress(int addressId)
        {
           return _ctx.Addresses1.Find(addressId);
        }

        // Returns enumerable of all addresses
        public IEnumerable<Address> GetAddresses()
        {
            return _ctx.Addresses1.AsEnumerable();
        }

        #endregion

        #region Position

        // Creates a position in the database based on given position
        // Returns newly created position
        public Position CreatePosition(Position position)
        {
            _ctx.Positions.Add(position);
            _ctx.SaveChanges();
            return position;
        }

        // Deletes given position from database
        public void DeletePosition(Position position)
        {
            _ctx.Positions.Remove(position);
            _ctx.SaveChanges();
        }

        // Returns the position based on given ID
        public Position GetPosition(int positionId)
        {
           return  _ctx.Positions.Find(positionId);
        }

        // Updates the values of position based on given position
        public void UpdatePosition(Position position)
        {
            _ctx.Positions.Update(position);
            _ctx.SaveChanges();
        }

        #endregion

       
    }
}