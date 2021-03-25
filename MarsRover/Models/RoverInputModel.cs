using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarsRover.Models
{
    public class RoverInputModel
    {
        public string Plateau { get; set; }
        public string FirstRoverPos { get; set; }
        public string FirstRoverMove { get; set; }
        public string SecondRoverPos { get; set; }
        public string SecondRoverMove { get; set; }
    }
}
