using DemoWebApi.Models;
using System.Collections.Generic;

namespace DemoWebApi.Repository 
{
    public interface IUfoSightingRepository
    {
        UfoSighting Get(int id);
        IEnumerable<UfoSighting> GetAll();
        UfoSighting Add(UfoSighting ufoSighting);
        UfoSighting Update(UfoSighting ufoSighting);
        void Delete(int id);

        void Clear();
    }
}

