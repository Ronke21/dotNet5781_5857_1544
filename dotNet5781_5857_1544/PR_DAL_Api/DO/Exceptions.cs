using System;

namespace DO
{
    //exception for xml files
    public class XMLFileLoadCreateException : Exception
    {
        private string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }

        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }


    //exception for input of a station location that is not in Israel
    [Serializable]
    public class NotInIsraelException : Exception
    {
        public NotInIsraelException() : base() { }

        public NotInIsraelException(string message) : base(message) { }
    }

    // exception for unreachable serializer
    [Serializable]
    public class CantLoadFromXmlException : Exception
    {
        public CantLoadFromXmlException() : base() { }

        public CantLoadFromXmlException(string message) : base(message) { }
    }

    //exception for a input of station which is already in the bus list of stations
    [Serializable]
    public class StationAlreadyExistsException : Exception
    {
        public StationAlreadyExistsException() : base() { }

        public StationAlreadyExistsException(string message) : base(message) { }
    }

    //exception for a input of station that does not exist in the bus list of stations
    [Serializable]
    public class StationDoesNotExistException : Exception
    {
        public StationDoesNotExistException() : base() { }

        public StationDoesNotExistException(string message) : base(message) { }
    }

    //exception for a input of station/bus that contains wrong digit number
    [Serializable]
    public class OutOfRangeException : Exception
    {
        public OutOfRangeException() : base() { }

        public OutOfRangeException(string message) : base(message) { }
    }

    //exception for a too far station from the last one
    [Serializable]
    public class TooLongException : Exception
    {
        public TooLongException() : base() { }

        public TooLongException(string message) : base(message) { }

        public override string ToString()
        { return "Can't ride over 1 hour without stop, you tried to ride" + Message + "\n"; }
    }

    //exception for a input of busline which is already in the bus collection
    [Serializable]
    public class BusLineAlreadyExistsException : Exception
    {
        public BusLineAlreadyExistsException() : base() { }

        public BusLineAlreadyExistsException(string message) : base(message) { }
    }

    //exception for a input of station that does not exist in the bus collection
    [Serializable]
    public class BusLineDoesNotExistsException : Exception
    {
        public BusLineDoesNotExistsException() : base() { }

        public BusLineDoesNotExistsException(string message) : base(message) { }
    }

    //exception for a input of station that does not exist in the bus collection
    [Serializable]
    public class NoSuchRouteException : Exception
    {
        public NoSuchRouteException() : base() { }

        public NoSuchRouteException(string message) : base(message) { }
    }

    [Serializable]
    public class BusLineNotActiveException : Exception
    {
        public BusLineNotActiveException() : base() { }

        public BusLineNotActiveException(string message) : base(message) { }
    }

    [Serializable]
    public class BusAlreadyExistsException : Exception
    {
        public BusAlreadyExistsException() : base() { }

        public BusAlreadyExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class BusDoesNotExistsException : Exception
    {
        public BusDoesNotExistsException() : base() { }

        public BusDoesNotExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class EmptyListException : Exception
    {
        public EmptyListException() : base() { }

        public EmptyListException(string message) : base(message) { }
    }

    [Serializable]
    public class AlreadyDeletedException : Exception
    {
        public AlreadyDeletedException() : base() { }

        public AlreadyDeletedException(string message) : base(message) { }
    }

    [Serializable]
    public class StationsAlreadyConsecutiveException : Exception
    {
        public StationsAlreadyConsecutiveException() : base() { }

        public StationsAlreadyConsecutiveException(string message) : base(message) { }
    }


    [Serializable]
    public class StationsAreNotConsecutiveException : Exception
    {
        public StationsAreNotConsecutiveException() : base() { }

        public StationsAreNotConsecutiveException(string message) : base(message) { }
    }

    [Serializable]
    public class DriverDoesNotExistsException : Exception
    {
        public DriverDoesNotExistsException() : base() { }

        public DriverDoesNotExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class DriverAlreadyExistsException : Exception
    {
        public DriverAlreadyExistsException() : base() { }

        public DriverAlreadyExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class LineStationsAlreadyExistsException : Exception
    {
        public LineStationsAlreadyExistsException() : base() { }

        public LineStationsAlreadyExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class LineStationsDoesNotExistsException : Exception
    {
        public LineStationsDoesNotExistsException() : base() { }

        public LineStationsDoesNotExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class TravelingBusesAlreadyExistsException : Exception
    {
        public TravelingBusesAlreadyExistsException() : base() { }

        public TravelingBusesAlreadyExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class TravelingBusesDoesNotExistsException : Exception
    {
        public TravelingBusesDoesNotExistsException() : base() { }

        public TravelingBusesDoesNotExistsException(string message) : base(message) { }
    }


    [Serializable]
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException() : base() { }

        public UserAlreadyExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class UserDoesNotExistsException : Exception
    {
        public UserDoesNotExistsException() : base() { }

        public UserDoesNotExistsException(string message) : base(message) { }
    }


    [Serializable]
    public class UserTravelAlreadyExistsException : Exception
    {
        public UserTravelAlreadyExistsException() : base() { }

        public UserTravelAlreadyExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class UserTravelDoesNotExistsException : Exception
    {
        public UserTravelDoesNotExistsException() : base() { }

        public UserTravelDoesNotExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class LineExitAlreadyExistsException : Exception
    {
        public LineExitAlreadyExistsException() : base() { }

        public LineExitAlreadyExistsException(string message) : base(message) { }
    }

    [Serializable]
    public class LineExitDoesNotExistsException : Exception
    {
        public LineExitDoesNotExistsException() : base() { }

        public LineExitDoesNotExistsException(string message) : base(message) { }
    }
}
