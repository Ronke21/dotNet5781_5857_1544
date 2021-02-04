﻿using System.Device.Location;

namespace DO
{
    /// <summary>
    /// represents a physical bus station somewhere,
    /// including an accurate location and address.
    /// primary key - code (original from ministry of transport).
    /// </summary>
    public class BusStation
    {
        //public int ID {get; set; }
        public int Code { get; set; }
        public GeoCoordinate Location { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public bool Accessible { get; set; }
        public bool Active { get; set; }
    }
}
