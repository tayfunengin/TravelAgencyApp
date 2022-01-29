using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Service.TourGuideService
{
    public interface ITourGuideService
    {
        IEnumerable<TourGuide> GetTourGuides();

        string CreateTourGuide(TourGuide tourGuide);
        string UpdateTourGuide(TourGuide tourGuide);
        string DeleteTourGuide(int tourGuideId);
        bool IfTourGuideCodeExist(string code);
        TourGuide GetTourGuideById(int tourGuideId);
    }
}
