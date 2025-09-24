using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Catalogue
{
    internal class Car
    {
        private string _make;
        private string _model;
        private string _color;
        private int _maxSpeed;
        private string _gearType;
        private string _engineType;
        public string Make => _make;
        public string Model => _model;
        public string Color => _color;
        public int MaxSpeed => _maxSpeed;
        public string GearType => _gearType;        
        public string EngineType => _engineType;
        public Car(string make, string model, string color, int maxSpeed, string gearType, string engineType)
        {
            _make = make;
            _model = model;
            _color = color;
            _maxSpeed = maxSpeed;
            _gearType = gearType;
            _engineType = engineType;
        }
        public StringBuilder GiveDetails() => new StringBuilder($"Make: {_make}, Model: {_model}, Color: {_color}\nMax Speed: {_maxSpeed} km/h\nGear Type: {_gearType}\nEngine Type: {_engineType}\n\n");
    }
}
