using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.AccomodationTypeService
{
    public interface IAccomodationTypeService
    {
        IEnumerable<AccomodationType> GetAccomodationTypes();

        IEnumerable<AccomodationType> GetActiveAccomodationTypes();

        AccomodationType GetAccomodationTypeById(int id);

        string CreateAccomodationType(AccomodationType accomodationType);

        string UpdateAccomodationType(AccomodationType accomodationType);

        string DeleteAccomodationType(int id);
        bool IfAccomodationTypeCodeExist(string accomodationTypeCode);
    }
}
