using System;

namespace DemoWebApi.Models
{
    public class UfoSighting
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; } // Description of the sighting
        public string Witness { get; set; } // Name of the person who saw the UFO
        public bool WasAlienSeen { get; set; } // Was an alien seen during the sighting?
        public string Shape { get; set; } // Shape of the UFO
        public string Color { get; set; } // Color of the UFO
        public string Duration { get; set; } // Duration of the sighting
    }
}


