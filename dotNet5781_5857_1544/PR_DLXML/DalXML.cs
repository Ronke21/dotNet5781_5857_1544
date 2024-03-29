﻿using System;
using System.Collections.Generic;
using System.Device.Location;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml.Linq;
using DalApi;
using DO;
using System.Xml;


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
        private const string conStatsPath = @"ConsecutiveStationsXml.xml"; //XElement
        private const string LineExitPath = @"LineExitXml.xml"; //XMLSerializer
        private const string keyGeneratorPath = @"KeyGeneratorXml.xml"; //XMLSerializer

        #endregion

        #region Bus
        public DO.Bus GetBus(int licenseNum)
        {
            var busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            //look the bus and create an instance
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

            //look the buses and create an instance
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
                throw new EmptyListException("Buses List is Empty");
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

            return buses;
        }
        public void AddBus(Bus bus)
        {
            var busesRootElem = XMLTools.LoadListFromXMLElement(busesPath);

            //make shore bus does not exist already
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

            //replace properties of existing bus in the new updated bus
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

        #region Bus Station
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

        #region Bus Line
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
            return null;
            //throw new BusLineDoesNotExistsException($"Bus line number {busLineId} does not exist");
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

        #region Line Station
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
        public IEnumerable<LineStation> GetAllLineStationsBy(Predicate<LineStation> predicate)
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
        public IEnumerable<LineStation> GetAllLineStations()
        {
            var lineStationsList = XMLTools.LoadListFromXMLSerializer<LineStation>(lineStationsPath);

            var stations = from ls in lineStationsList
                           where ls.Active is true
                           select ls;

            if (stations is null)
            {
                throw new LineStationsDoesNotExistsException($"No stations in this line");
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

        #region Consecutive Stations
        public IEnumerable<ConsecutiveStations> GetAllConsecutiveStations()
        {
            var conStatRootElem = XMLTools.LoadListFromXMLElement(conStatsPath);

            var conStats = from cs in conStatRootElem.Elements()
                           where bool.Parse(cs.Element("Active").Value) // == true
                           select new ConsecutiveStations()
                           {
                               Active = bool.Parse(cs.Element("Active").Value),
                               Distance = double.Parse(cs.Element("Distance").Value),
                               StatCode1 = int.Parse(cs.Element("StatCode1").Value),
                               StatCode2 = int.Parse(cs.Element("StatCode2").Value),
                               AverageTravelTime = XmlConvert.ToTimeSpan(cs.Element("AverageTravelTime").Value)
                           };

            if (conStats is null)
            {
                throw new StationsAreNotConsecutiveException("No consecutive stations found");
            }

            return conStats;
        }
        public void AddConsecutiveStations(int statCode1, int statCode2, TimeSpan toNext, double distance)
        {
            var conStatRootElem = XMLTools.LoadListFromXMLElement(conStatsPath);

            var cons = (from cs in conStatRootElem.Elements()
                        where bool.Parse(cs.Element("Active").Value) &&
                              statCode1 == int.Parse(cs.Element("StatCode1").Value) &&
                              statCode2 == int.Parse(cs.Element("StatCode2").Value)
                        select cs).FirstOrDefault();

            if (!(cons is null)) return;

            var con = new XElement("ConsecutiveStations",
                new XElement("Active", true),
                new XElement("StatCode1", statCode1),
                new XElement("StatCode2", statCode2),
                new XElement("Distance", distance),
                new XElement("AverageTravelTime", XmlConvert.ToString(toNext)));

            conStatRootElem.Add(con);

            XMLTools.SaveListToXMLElement(conStatRootElem, conStatsPath);
        }
        public ConsecutiveStations GetConsecutiveStations(int statCode1, int statCode2)
        {
            var conStatRootElem = XMLTools.LoadListFromXMLElement(conStatsPath);

            var cons = (from cs in conStatRootElem.Elements()
                        where bool.Parse(cs.Element("Active").Value) &&
                              statCode1 == int.Parse(cs.Element("StatCode1").Value) &&
                              statCode2 == int.Parse(cs.Element("StatCode2").Value)
                        select cs).FirstOrDefault();

            if (cons is null)
            {
                throw new StationsAreNotConsecutiveException($"Stations {statCode1} and {statCode2} are not consecutive stations");
            }

            return new ConsecutiveStations()
            {
                Active = bool.Parse(cons.Element("Active").Value),
                Distance = double.Parse(cons.Element("Distance").Value),
                StatCode1 = int.Parse(cons.Element("StatCode1").Value),
                StatCode2 = int.Parse(cons.Element("StatCode2").Value),
                AverageTravelTime = XmlConvert.ToTimeSpan(cons.Element("AverageTravelTime").Value)
            };
        }
        public void UpdateConsecutiveStations(ConsecutiveStations conStat)
        {
            var conStatRootElem = XMLTools.LoadListFromXMLElement(conStatsPath);

            var cons = (from cs in conStatRootElem.Elements()
                        where bool.Parse(cs.Element("Active").Value) &&
                              conStat.StatCode1 == int.Parse(cs.Element("StatCode1").Value) &&
                              conStat.StatCode2 == int.Parse(cs.Element("StatCode2").Value)
                        select cs).FirstOrDefault();

            if (cons is null)
            {
                throw new StationsAreNotConsecutiveException($"Stations {conStat.StatCode1} and {conStat.StatCode2} are not consecutive stations");
            }

            cons.Element("Active").Value = conStat.Active.ToString();
            cons.Element("AverageTravelTime").Value = XmlConvert.ToString(conStat.AverageTravelTime);
            cons.Element("Distance").Value = conStat.Distance.ToString();
            cons.Element("StatCode1").Value = conStat.StatCode1.ToString();
            cons.Element("StatCode2").Value = conStat.StatCode2.ToString();

            XMLTools.SaveListToXMLElement(conStatRootElem, conStatsPath);
        }
        public bool CheckConsecutiveStationsNotExist(int statCode1, int statCode2)
        {
            var conStatRootElem = XMLTools.LoadListFromXMLElement(conStatsPath);

            var cons = (from cs in conStatRootElem.Elements()
                        where statCode1 == int.Parse(cs.Element("StatCode1").Value) &&
                              statCode2 == int.Parse(cs.Element("StatCode2").Value)
                        select cs).FirstOrDefault();

            return cons is null;
        }
        #endregion

        #region Line exit

        public void AddLineExit(LineExit lineExit)
        {
            var lineExitRootElem = XMLTools.LoadListFromXMLElement(LineExitPath);

            var exit = (from le in lineExitRootElem.Elements()
                        where (int.Parse(le.Element("BusLineId").Value) == lineExit.BusLineId &&
                               XmlConvert.ToTimeSpan(le.Element("StartTime").Value) == lineExit.StartTime)
                        select le).FirstOrDefault();
            
            if (exit == null)
            {
                var lineExitElem = new XElement("LineExit",
                    new XElement("BusLineId", lineExit.BusLineId),
                    new XElement("Active", lineExit.Active),
                    new XElement("StartTime", XmlConvert.ToString(lineExit.StartTime)),
                    new XElement("EndTime", XmlConvert.ToString(lineExit.EndTime)),
                    new XElement("Freq", lineExit.Freq));

                lineExitRootElem.Add(lineExitElem);
            }

            else if (bool.Parse(exit.Element("Active").Value) is false)
            {
                exit.Element("Active").Value = lineExit.Active.ToString();
            }

            else throw new LineExitAlreadyExistsException($"Line exit number {lineExit.BusLineId},{lineExit.StartTime} already exist");

            XMLTools.SaveListToXMLElement(lineExitRootElem, LineExitPath);
        }
        public IEnumerable<LineExit> GetAllLineExits()
        {
            var lineExitRootElem = XMLTools.LoadListFromXMLElement(LineExitPath);

            var lineExits = from le in lineExitRootElem.Elements()
                where bool.Parse(le.Element("Active").Value) // == true
                select new LineExit()
                {
                    BusLineId = int.Parse(le.Element("BusLineId").Value),
                    Active = bool.Parse(le.Element("Active").Value),
                    Freq = XmlConvert.ToTimeSpan(le.Element("Freq").Value),
                    StartTime = XmlConvert.ToTimeSpan(le.Element("StartTime").Value),
                    EndTime = XmlConvert.ToTimeSpan(le.Element("EndTime").Value)
                };

            //if (!lineExits.Any())
            //{
            //    throw new EmptyListException("Buses List is Empty");
            //}

            return lineExits;
        }
        public LineExit GetLineExit(int busLineId, TimeSpan startTime)
        {
            var lineExitRootElem = XMLTools.LoadListFromXMLElement(LineExitPath);

            var exit = (from le in lineExitRootElem.Elements()
                        where int.Parse(le.Element("BusLineId").Value) == busLineId &&
                              XmlConvert.ToTimeSpan(le.Element("StartTime").Value) == startTime
                        select new LineExit()
                        {
                            BusLineId = int.Parse(le.Element("BusLineId").Value),
                            Active = bool.Parse(le.Element("Active").Value),
                            Freq = XmlConvert.ToTimeSpan(le.Element("Freq").Value),
                            StartTime = XmlConvert.ToTimeSpan(le.Element("StartTime").Value),
                            EndTime = XmlConvert.ToTimeSpan(le.Element("EndTime").Value)
                        }).FirstOrDefault();

            if (exit == null)
            {
                throw new LineExitDoesNotExistsException($"Line exit number {busLineId},{startTime} does not exist");
            }

            return exit;
        }
        public LineExit GetGeneralLineExit(int busLineId)
        {
            var lineExitRootElem = XMLTools.LoadListFromXMLElement(LineExitPath);

            var exit = (from le in lineExitRootElem.Elements()
                where int.Parse(le.Element("BusLineId").Value) == busLineId
                select new LineExit()
                {
                    BusLineId = int.Parse(le.Element("BusLineId").Value),
                    Active = bool.Parse(le.Element("Active").Value),
                    Freq = XmlConvert.ToTimeSpan(le.Element("Freq").Value),
                    StartTime = new TimeSpan(0,0,0),
                    EndTime = new TimeSpan(3,0,0)
                }).FirstOrDefault();

            if (exit == null)
            {
                throw new LineExitDoesNotExistsException($"Bus {busLineId} does not have line exists");
            }

            return exit;
        }
        public void DeleteLineExit(int busLineId, TimeSpan startTime)
        {
            var lineExitRootElem = XMLTools.LoadListFromXMLElement(LineExitPath);

            var exit = (from le in lineExitRootElem.Elements()
                        where (int.Parse(le.Element("BusLineId").Value) == busLineId &&
                               XmlConvert.ToTimeSpan(le.Element("StartTime").Value) == startTime)
                        select le).FirstOrDefault();

            if (exit == null)
            {
                throw new DO.LineExitDoesNotExistsException("This line exit does not exist!");
            }

            else if (bool.Parse(exit.Element("Active").Value) is true)
            {
                exit.Element("Active").Value = false.ToString();
            }

            XMLTools.SaveListToXMLElement(lineExitRootElem, LineExitPath);
        }

        #endregion

        public int GetKey()
        {
            var keyRootElem = XMLTools.LoadListFromXMLElement(keyGeneratorPath);

            //check current key in the xml file
            var ser = (from s in keyRootElem.Elements()
                       select s).FirstOrDefault();


            if (ser is null) throw new CantLoadFromXmlException("Can't get key");

            var key = int.Parse(ser.Value);

            //promote the key ++
            ser.Value = (int.Parse(ser.Value) + 1).ToString();

            XMLTools.SaveListToXMLElement(keyRootElem, keyGeneratorPath);

            return key;
        }
    }
}