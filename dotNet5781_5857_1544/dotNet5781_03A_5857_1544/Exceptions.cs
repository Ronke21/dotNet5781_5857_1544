/*
 all the inheritances from Exception class
 */
using System;
using System.Runtime.Serialization;

//exception for input of a station location that is not in Israel
#region NotInIsraelException
[Serializable]
public class NotInIsraelException : Exception
{
    public NotInIsraelException() : base() {}

    public NotInIsraelException(string message) : base(message) {}
}
#endregion

//exception for a input of station which is already in the bus list of stations
#region StationAlreadyExistsException
[Serializable]
public class StationAlreadyExistsException : Exception
{
    public StationAlreadyExistsException() : base() { }

    public StationAlreadyExistsException(string message) : base(message) { }
}
#endregion

//exception for a input of station that does not exist in the bus list of stations
#region StationDoesNotExistException
[Serializable]
public class StationDoesNotExistException : Exception
{
    public StationDoesNotExistException() : base() { }

    public StationDoesNotExistException(string message) : base(message) { }
}
#endregion

//exception for a input of station/bus that contains wrong digit number
#region OutOfRangeException
[Serializable]
public class OutOfRangeException : Exception
{
    public OutOfRangeException() : base() { }

    public OutOfRangeException(string message) : base(message) { }
}
#endregion

//exception for a too far station from the last one
#region TooLongException
[Serializable]
public class TooLongException : Exception
{
    public TooLongException() : base() { }

    public TooLongException(string message) : base(message) { }

    override public string ToString()
    { return "Can't ride over 1 hour without stop, you tried to ride" + Message + "\n"; }
}
#endregion

//exception for a input of busline which is already in the bus collection
#region BusLineAlreadyExistsException
[Serializable]
public class BusLineAlreadyExistsException : Exception
{
    public BusLineAlreadyExistsException() : base() { }

    public BusLineAlreadyExistsException(string message) : base(message) { }
}
#endregion

//exception for a input of station that does not exist in the bus collection
#region BusLineDoesNotExistsException
[Serializable]
public class BusLineDoesNotExistsException : Exception
{
    public BusLineDoesNotExistsException() : base() { }

    public BusLineDoesNotExistsException(string message) : base(message) { }
}
#endregion

//exception for a input of station that does not exist in the bus collection
#region NoSuchRouteException
[Serializable]
public class NoSuchRouteException : Exception
{
    public NoSuchRouteException () : base() { }

    public NoSuchRouteException (string message) : base(message) { }
}
#endregion