using DemoWebApi.Models;
using System.Collections.Generic;
using System.Linq;

namespace DemoWebApi.Repository
{
    public class UfoSightingRepository : IUfoSightingRepository
    {
        private readonly List<UfoSighting> _ufoSightings = new List<UfoSighting>();

        public UfoSighting Get(int id)
        {
            return _ufoSightings.FirstOrDefault(u => u.Id == id);
        }

        public IEnumerable<UfoSighting> GetAll()
        {
            return _ufoSightings;
        }

        public UfoSighting Add(UfoSighting ufoSighting)
        {
            ufoSighting.Id = _ufoSightings.Count + 1;
            _ufoSightings.Add(ufoSighting);
            return ufoSighting;
        }

        public UfoSighting Update(UfoSighting ufoSighting)
        {
            var existingUfoSighting = _ufoSightings.FirstOrDefault(u => u.Id == ufoSighting.Id);
            if (existingUfoSighting == null) return null;

            existingUfoSighting.Location = ufoSighting.Location;
            existingUfoSighting.Date = ufoSighting.Date;
            existingUfoSighting.Description = ufoSighting.Description;
            existingUfoSighting.Witness = ufoSighting.Witness;
            existingUfoSighting.WasAlienSeen = ufoSighting.WasAlienSeen;
            existingUfoSighting.Shape = ufoSighting.Shape;
            existingUfoSighting.Color = ufoSighting.Color;
            existingUfoSighting.Duration = ufoSighting.Duration;

            return existingUfoSighting;
        }

        public void Delete(int id)
        {
            var ufoSighting = _ufoSightings.FirstOrDefault(u => u.Id == id);
            if (ufoSighting != null)
            {
                _ufoSightings.Remove(ufoSighting);
            }
        }

        public void Clear()
        {
            _ufoSightings.Clear();
        }
    }
}


