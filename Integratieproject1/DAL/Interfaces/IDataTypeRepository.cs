using System.Collections.Generic;
using Integratieproject1.Domain.Datatypes;

namespace Integratieproject1.DAL.Interfaces
{
    public interface IDataTypeRepository
    {
        
        #region Locations

        IEnumerable<Location> GetLocations();
        #endregion
        
        #region Addresses

        Address GetAddress(int addressId);
        IEnumerable<Address> GetAddresses();
        #endregion

        #region Position

        Position CreatePosition(Position position);
        void DeletePosition(Position position);
        Position GetPosition(int positionId);
        void UpdatePosition(Position position);

        #endregion

    }
}