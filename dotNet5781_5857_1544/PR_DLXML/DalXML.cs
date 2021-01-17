using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using DalApi;
using DO;
using System.Device.Location;
using System.IO;


namespace Dal
{
    internal sealed class DalXML : IDal
    {
        #region singelton
        static readonly DalXML instance = new DalXML();
        static DalXML() { }// static ctor to ensure instance init is done just before first usage
        private DalXML() { } // default => private
        public static DalXML Instance { get => instance; }// The public Instance property to use
        #endregion

        #region DS XML Files

        private const string busesPath = @"BusesXml.xml"; //XElement

        private const string busStationsPath = @"BusStationsXml.xml"; //XMLSerializer
        private const string busLinesPath = @"BusLinesXml.xml"; //XMLSerializer
        private const string lineStationsPath = @"LineStationsXml.xml"; //XMLSerializer
        private const string consecutiveStationsPath = @"ConsecutiveStationsXml.xml"; //XMLSerializer
        private const string keyGeneratorPath = @"KeyGeneratorXml.xml"; //XMLSerializer

        #endregion

        #region Bus
        public DO.Bus GetBus(int licenseNum)
        {
            var busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            var bus = (from bs in busesRootElem.Elements()
                       where int.Parse(bs.Element("LicenseNum").Value) == licenseNum
                       select new Bus()
                       {
                           LicenseNum = int.Parse(bs.Element("LicenseNum").Value),
                           Fuel = double.Parse(bs.Element("Fuel").Value),
                           Active = bool.Parse(bs.Element("Active").Value),
                           Mileage = double.Parse(bs.Element("Mileage").Value),
                           MileageFromLast = double.Parse(bs.Element("MileageFromLast").Value),
                           Stat = (Status)Enum.Parse(typeof(Status), bs.Element("Stat").Value),
                           StartTime = DateTime.Parse(bs.Element("StartTime").Value),
                           LastMaint = DateTime.Parse(bs.Element("LastMaint").Value),
                       }).FirstOrDefault();

            if (bus == null)
            {
                throw new BusDoesNotExistsException($"Bus number {licenseNum} does not exist");
            }

            return bus;
        }
        public IEnumerable<Bus> GetAllActiveBuses()
        {
            var busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            var buses = from bs in busesRootElem.Elements()
                        where bool.Parse(bs.Element("Active").Value) // == true
                        select new Bus()
                        {
                            LicenseNum = int.Parse(bs.Element("LicenseNum").Value),
                            Fuel = double.Parse(bs.Element("Fuel").Value),
                            Active = bool.Parse(bs.Element("Active").Value),
                            Mileage = double.Parse(bs.Element("Mileage").Value),
                            MileageFromLast = double.Parse(bs.Element("MileageFromLast").Value),
                            Stat = (Status)Enum.Parse(typeof(Status), bs.Element("Stat").Value),
                            StartTime = DateTime.Parse(bs.Element("StartTime").Value),
                            LastMaint = DateTime.Parse(bs.Element("LastMaint").Value),
                        };

            if (!buses.Any())
            {
                throw new EmptyListException("List is Empty");
            }

            return buses;
        }
        public IEnumerable<Bus> GetAllInActiveBuses()
        {
            var busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            var buses = from bs in busesRootElem.Elements()
                        where !bool.Parse(bs.Element("Active").Value) // == false
                        select new Bus()
                        {
                            LicenseNum = int.Parse(bs.Element("LicenseNum").Value),
                            Fuel = double.Parse(bs.Element("Fuel").Value),
                            Active = bool.Parse(bs.Element("Active").Value),
                            Mileage = double.Parse(bs.Element("Mileage").Value),
                            MileageFromLast = double.Parse(bs.Element("MileageFromLast").Value),
                            Stat = (Status)Enum.Parse(typeof(Status), bs.Element("Stat").Value),
                            StartTime = DateTime.Parse(bs.Element("StartTime").Value),
                            LastMaint = DateTime.Parse(bs.Element("LastMaint").Value),
                        };

            if (!buses.Any())
            {
                throw new EmptyListException("List is Empty");
            }

            return buses;
        }
        public void AddBus(Bus bus)
        {
            var busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            var bu = (from b in busesRootElem.Elements()
                      where int.Parse(b.Element("LicenseNum").Value) == bus.LicenseNum
                      select b).FirstOrDefault();

            if (bu == null)
            {
                var busElem = new XElement("Bus",
                              new XElement("LicenseNum", bus.LicenseNum),
                              new XElement("Active", bus.Active),
                              new XElement("StartTime", bus.StartTime),
                              new XElement("LastMaint", bus.LastMaint),
                              new XElement("MileageFromLast", bus.MileageFromLast),
                              new XElement("Stat", bus.Stat.ToString()),
                              new XElement("Fuel", bus.Fuel),
                              new XElement("Mileage", bus.Mileage));

                busesRootElem.Add(busElem);
            }

            else if (bool.Parse(bu.Attribute("Active").Value) is false)
            {
                bu.Element("Active").Value = bus.Active.ToString();
            }

            else throw new BusAlreadyExistsException($"Bus number {bus.LicenseNum} already exists");

            XMLTools.SaveListToXMLElement(busesRootElem, busesPath);
        }
        public void DeleteBus(int licenseNum)
        {
            var busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            var bs = (from b in busesRootElem.Elements()
                      where int.Parse(b.Element("LicenseNum").Value) == licenseNum
                      select b).FirstOrDefault();

            if (bs != null)
            {
                bs.Element("Active").Value = false.ToString();

                XMLTools.SaveListToXMLElement(busesRootElem, busesPath);
            }
            else
            {
                throw new BusDoesNotExistsException($"Bus number {licenseNum} does not exist");
            }
        }
        public void ActivateBus(int licenseNum)
        {
            var busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            var bs = (from b in busesRootElem.Elements()
                      where int.Parse(b.Element("LicenseNum").Value) == licenseNum
                      select b).FirstOrDefault();

            if (bs != null)
            {
                bs.Element("Active").Value = true.ToString();

                XMLTools.SaveListToXMLElement(busesRootElem, busesPath);
            }
            else
            {
                throw new BusDoesNotExistsException($"Bus number {licenseNum} does not exist");
            }
        }
        public void UpdateBus(DO.Bus bus)
        {
            var busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            var bs = (from b in busesRootElem.Elements()
                      where int.Parse(b.Element("LicenseNum").Value) == bus.LicenseNum
                      select b).FirstOrDefault();

            if (bs != null)
            {
                bs.Element("LicenseNum").Value = bus.LicenseNum.ToString();
                bs.Element("Active").Value = bus.Active.ToString();
                bs.Element("StartTime").Value = bus.StartTime.ToString();
                bs.Element("LastMaint").Value = bus.LastMaint.ToString();
                bs.Element("MileageFromLast").Value = bus.MileageFromLast.ToString();
                bs.Element("Stat").Value = bus.Stat.ToString();
                bs.Element("Fuel").Value = bus.Fuel.ToString();
                bs.Element("Mileage").Value = bus.Mileage.ToString();

                XMLTools.SaveListToXMLElement(busesRootElem, busesPath);
            }
            else
            {
                throw new BusDoesNotExistsException($"Bus number {bus.LicenseNum} does not exist");
            }
        }
        #endregion

        #region BusStation
        public void AddBusStation(BusStation busStation)
        {
            var busStationsList = XMLTools.LoadListFromXMLSerializer<BusStation>(busStationsPath);

            var bs = busStationsList.Find(b => b.Code == busStation.Code);

            if (bs == null)
            {
                busStationsList.Add(busStation);
            }

            else if (bs.Active is false)
            {
                bs.Active = true;
            }

            else throw new StationAlreadyExistsException($"Station number {busStation.Code} already exists");

            XMLTools.SaveListToXMLSerializer(busStationsList, busStationsPath);
        }
        public void ActivateBusStation(int code)
        {
            var busStationsList = XMLTools.LoadListFromXMLSerializer<BusStation>(busStationsPath);

            var busStation = busStationsList.Find(l => l.Code == code);
            if (busStation is null) throw new StationDoesNotExistException($"Bus line number {code} does not exist");
            busStation.Active = true;

            XMLTools.SaveListToXMLSerializer(busStationsList, busStationsPath);
        }
        public IEnumerable<BusStation> GetAllActiveBusStations()
        {
            #region init

            //var code = File.ReadAllLines(@"..\PR_DS\DataSource\code.txt");
            //var name = File.ReadAllLines(@"..\PR_DS\DataSource\name.txt");
            //var longitude = File.ReadAllLines(@"..\PR_DS\DataSource\longitude.txt");
            //var latitude = File.ReadAllLines(@"..\PR_DS\DataSource\latitude.txt");
            //var address = File.ReadAllLines(@"..\PR_DS\DataSource\address.txt");

            //var x = new List<BusStation>();

            //for (var i = 0; i < 300; i++)
            //{
            //    x.Add(
            //        new BusStation()
            //        {
            //            Accessible = true,
            //            Active = true,
            //            Address = address[i],
            //            Code = Convert.ToInt32(code[i]),
            //            Location = new GeoCoordinate(Convert.ToDouble(latitude[i]), Convert.ToDouble(longitude[i])),
            //            Name = name[i]
            //        });
            //}

            //x = x.OrderBy(s => s.Code).ToList();

            //XMLTools.SaveListToXMLSerializer(x, busStationsPath);

            #endregion

            var busStationsList = XMLTools.LoadListFromXMLSerializer<BusStation>(busStationsPath);

            if (busStationsList.Count == 0)
            {
                throw new EmptyListException("Active Bus Station List is Empty!");
            }

            return from bs in busStationsList
                   where bs.Active // is true
                   select bs;
        }
        public IEnumerable<BusStation> GetAllInActiveBusStations()
        {
            var busStationsList = XMLTools.LoadListFromXMLSerializer<BusStation>(busStationsPath);

            if (busStationsList.Count == 0)
            {
                throw new EmptyListException("Active Bus Station List is Empty!");
            }

            return from bs in busStationsList
                   where bs.Active is false
                   select bs;
        }
        public DO.BusStation GetBusStation(int code)
        {
            var busStationsList = XMLTools.LoadListFromXMLSerializer<BusStation>(busStationsPath);

            var bs = busStationsList.Find(b => b.Code == code);
            if (bs != null) return bs;
            throw new StationDoesNotExistException($"Station number {code} does not exist");
        }
        public void UpdateBusStation(DO.BusStation bs)
        {
            var busStationsList = XMLTools.LoadListFromXMLSerializer<BusStation>(busStationsPath);

            var updatedStation = busStationsList.Find(s => s.Code == bs.Code);
            if (updatedStation is null) throw new StationDoesNotExistException($"Station number {bs.Code} does not exist");
            bs.Mover(updatedStation);

            XMLTools.SaveListToXMLSerializer(busStationsList, busStationsPath);
        }
        public void DeleteBusStation(int code)
        {
            var busStationsList = XMLTools.LoadListFromXMLSerializer<BusStation>(busStationsPath);

            var bs = busStationsList.Find(b => b.Code == code);
            if (bs is null) throw new StationDoesNotExistException($"Station number {code} does not exist");
            bs.Active = false;

            XMLTools.SaveListToXMLSerializer(busStationsList, busStationsPath);
        }
        #endregion

        #region BusLine
        public void AddBusLine(BusLine busLine)
        {
            var busLinesList = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinesPath);

            var bl = busLinesList.Find(b => b.BusLineId == busLine.BusLineId);

            if (bl is null)
            {
                busLinesList.Add(busLine);
            }
            else if (bl.Active is false)
            {
                bl.Active = true;
            }
            else throw new BusLineAlreadyExistsException($"Bus line {busLine.LineNumber} already exist and active ({busLine.BusLineId})");

            XMLTools.SaveListToXMLSerializer(busLinesList, busLinesPath);
        }
        public IEnumerable<BusLine> GetAllActiveBusLines()
        {
            var busLinesList = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinesPath);

            if (busLinesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(busLinesList)} is Empty");
            }

            return from busLine in busLinesList
                   where busLine.Active // is true
                   select busLine;
        }
        public IEnumerable<BusLine> GetAllInActiveBusLines()
        {
            var busLinesList = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinesPath);

            if (busLinesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(busLinesList)} is Empty");
            }

            return from busLine in busLinesList
                   where busLine.Active is false
                   select busLine;
        }
        public IEnumerable<BusLine> GetAllBusLinesBy(Predicate<BusLine> predicate)
        {
            var busLinesList = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinesPath);

            if (busLinesList.Count == 0)
            {
                throw new EmptyListException($"{nameof(busLinesList)} is Empty");
            }

            return from busLine in busLinesList
                   where predicate(busLine)
                   select busLine;
        }
        public BusLine GetBusLine(int busLineId)
        {
            var busLinesList = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinesPath);

            var busLine = busLinesList.Find(b => b.BusLineId == busLineId);
            if (busLine != null) return busLine;
            throw new BusLineDoesNotExistsException($"Bus line number {busLineId} does not exist");
        }
        public void ActivateBusLine(int busLineId)
        {
            var busLinesList = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinesPath);

            var busLine = busLinesList.Find(l => l.BusLineId == busLineId);
            if (busLine is null) throw new BusLineDoesNotExistsException($"Bus line number {busLineId} does not exist");
            busLine.Active = true;

            XMLTools.SaveListToXMLSerializer(busLinesList, busLinesPath);
        }
        public void UpdateBusLine(BusLine update)
        {
            var busLinesList = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinesPath);

            var updatedLine = busLinesList.Find(bl => bl.BusLineId == update.BusLineId);
            if (updatedLine is null) throw new BusDoesNotExistsException($"Line number {update.BusLineId} does not exist");
            update.Mover(updatedLine);

            XMLTools.SaveListToXMLSerializer(busLinesList, busLinesPath);
        }
        public void DeleteBusLine(int busLineId)
        {
            var busLinesList = XMLTools.LoadListFromXMLSerializer<BusLine>(busLinesPath);

            var busLine = busLinesList.Find(l => l.BusLineId == busLineId);
            if (busLine is null) throw new BusLineDoesNotExistsException($"Bus line number {busLineId} does not exist");
            busLine.Active = false;

            XMLTools.SaveListToXMLSerializer(busLinesList, busLinesPath);
        }

        #endregion

        #region LineStation
        public void AddLineStation(LineStation lineStation)
        {
            var lineStationsList = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            var ls = lineStationsList.Find(l => l.BusLineId == lineStation.BusLineId &&
                                                            l.Code == lineStation.Code);
            if (ls == null)
            {
                lineStationsList.Add(lineStation);
            }

            else if (ls.Active is false)
            {
                ls.StationIndex = lineStation.StationIndex;
                ls.Active = true;
            }

            else throw new LineStationsAlreadyExistsException($"Line station {lineStation.BusLineId}/{lineStation.Code} already exists");

            XMLTools.SaveListToXMLSerializer(lineStationsList, lineStationsPath);
        }
        public IEnumerable<LineStation> GetAllLineStationsByLineID(int LineID)
        {
            var lineStationsList = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);
            var stations = from ls in lineStationsList
                           where ls.BusLineId == LineID && ls.Active is true
                           select ls;

            if (stations is null)
            {
                throw new LineStationsDoesNotExistsException($"Line {LineID} does not have any stations");
            }

            return stations;
        }
        public IEnumerable<LineStation> GetAlLineStationsBy(Predicate<LineStation> predicate)
        {
            var lineStationsList = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            var stations = from ls in lineStationsList
                           where ls.Active is true && predicate(ls)
                           select ls;

            if (stations is null)
            {
                throw new LineStationsDoesNotExistsException($"No stations satisfies the condition {predicate}");
            }

            return stations;
        }
        public LineStation GetLineStation(int lineNumber, int stationNumber)
        {
            var lineStationsList = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            var lineStation = lineStationsList.Find(ls => ls.BusLineId == lineNumber &&
                                                                      ls.Code == stationNumber &&
                                                                      ls.Active);
            if (lineStation is null)
            {
                throw new LineStationsDoesNotExistsException($"Line station {lineNumber}/{stationNumber} does not exists or not active");

            }

            return lineStation;
        }
        public void UpdateLineStation(int lineNumber, int stationNumber, int stationIndex)
        {
            var lineStationsList = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            var lineStation = lineStationsList.Find(ls => ls.BusLineId == lineNumber &&
                                                          ls.Code == stationNumber &&
                                                          ls.Active);

            if (lineStation is null)
            {
                throw new LineStationsDoesNotExistsException($"Line station {lineNumber}/{stationNumber} does not exists or not active");

            }

            lineStation.StationIndex = stationIndex;

            XMLTools.SaveListToXMLSerializer(lineStationsList, lineStationsPath);
        }
        public void DeleteLineStation(int lineNumber, int stationNumber)
        {
            var lineStationsList = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            var lineStation = lineStationsList.Find(ls => ls.BusLineId == lineNumber &&
                                                          ls.Code == stationNumber);

            if (lineStation is null)
            {
                throw new LineStationsDoesNotExistsException($"Line station {lineNumber}/{stationNumber} does not exists or not active");
            }

            lineStation.Active = false;

            XMLTools.SaveListToXMLSerializer(lineStationsList, lineStationsPath);
        }


        #endregion

        #region ConsecutiveStations
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations()
        {
            var consecutiveStationsList = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(consecutiveStationsPath);

            var conStats = from cs in consecutiveStationsList
                           where cs.Active
                           select cs;

            if (conStats is null)
            {
                throw new StationsAreNotConsecutiveException("No consecutive stations found");
            }

            return conStats;
        }
        public void AddConsecutiveStations(int statCode1, int statCode2, TimeSpan toNext, double distance)
        {
            var consecutiveStationsList = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(consecutiveStationsPath);

            var cons = consecutiveStationsList.Find(c => c.StatCode1 == statCode1 &&
                                                                                    c.StatCode2 == statCode2);

            if (cons is null)
            {
                var con =
                    new ConsecutiveStations
                    {
                        StatCode1 = statCode1,
                        StatCode2 = statCode2,
                        Distance = distance,
                        AverageTravelTime = toNext,
                        Active = true
                    };
                consecutiveStationsList.Add(con);
            }

            XMLTools.SaveListToXMLSerializer(consecutiveStationsList, consecutiveStationsPath);
        }
        public ConsecutiveStations GetConsecutiveStations(int statCode1, int statCode2)
        {
            var consecutiveStationsList = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(consecutiveStationsPath);

            var consecutiveStations = consecutiveStationsList.Find(c => c.StatCode1 == statCode1 &&
                                                                                   c.StatCode2 == statCode2);

            if (consecutiveStations is null)
            {
                throw new StationsAreNotConsecutiveException($"Stations {statCode1} and {statCode2} are not consecutive stations");
            }

            return consecutiveStations;
        }
        public void UpdateConsecutiveStations(ConsecutiveStations conStat)
        {
            var consecutiveStationsList = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(consecutiveStationsPath);

            var updatedConStat = consecutiveStationsList.Find(c => c.StatCode1 == conStat.StatCode1 &&
                                                                   c.StatCode2 == conStat.StatCode2);

            if (updatedConStat is null)
            {
                throw new StationsAreNotConsecutiveException($"Stations {conStat.StatCode1} and {conStat.StatCode2} are not consecutive stations");
            }
            conStat.Mover(updatedConStat);

            XMLTools.SaveListToXMLSerializer(consecutiveStationsList, consecutiveStationsPath);
        }
        public bool CheckConsecutiveStationsNotExist(int statCode1, int statCode2)
        {
            var consecutiveStationsList = XMLTools.LoadListFromXMLSerializer<ConsecutiveStations>(consecutiveStationsPath);

            return (consecutiveStationsList.Find(c => c.StatCode1 == statCode1 &&
                                                                 c.StatCode2 == statCode2) is null);
        }
        #endregion

        public int GetKey()
        {
            var keyRootElem = XMLTools.LoadListFromXMLElement(keyGeneratorPath);

            var ser = (from s in keyRootElem.Elements()
                       select s).FirstOrDefault();


            if (ser is null) throw new CantLoadFromXmlException("Can't get key");

            var key = int.Parse(ser.Value);

            ser.Value = (int.Parse(ser.Value) + 1).ToString();

            XMLTools.SaveListToXMLElement(keyRootElem, keyGeneratorPath);

            return key;
        }
    }
}