using Integratieproject1.Domain.Datatypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Integratieproject1.BL.Interfaces
{
interface IDataTypeManager
    {
        #region Location

        Location CheckLocation(Location toCheckLocation);

        #endregion

        #region Address

        Address GetAddress(int addressId);
        Address CheckAddress(Address toCheckAddress);

        #endregion

        #region Position

        Position GetPosition(int positionId);
        Position CreatePosition(Position position);
        Position EditPosition(Position position, int positionId);
        void DeletePosition(int positionId);

        #endregion
    }
}
