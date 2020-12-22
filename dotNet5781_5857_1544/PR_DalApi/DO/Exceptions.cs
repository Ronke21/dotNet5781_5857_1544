using System;

namespace DalObject
{
    //exception for input of a station location that is not in Israel
    [Serializable]
    public class NotInIsraelException : Exception
    {
        public NotInIsraelException() : base() { }

        public NotInIsraelException(string message) : base(message) { }
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
}
