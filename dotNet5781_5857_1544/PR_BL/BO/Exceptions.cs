﻿using System;

namespace BO
{
    //exception for input of a station location that is not in Israel
    [Serializable]
    public class NotInIsraelException : Exception
    {
        public NotInIsraelException() : base() { }

        public NotInIsraelException(string message) : base(message) { }
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

    [Serializable]
    public class NotValidLicenseNumberException : Exception
    {
        public NotValidLicenseNumberException() : base() { }

        public NotValidLicenseNumberException(string message) : base(message) { }
    }

    [Serializable]
    public class NotValidFuelAmountException : Exception
    {
        public NotValidFuelAmountException() : base() { }

        public NotValidFuelAmountException(string message) : base(message) { }
    }

    [Serializable]
    public class NotValidMileageException : Exception
    {
        public NotValidMileageException() : base() { }

        public NotValidMileageException(string message) : base(message) { }
    }

    [Serializable]
    public class BadAdditionException : Exception
    {
        public BadAdditionException(Exception inner) : base() { }
        public BadAdditionException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class EmptyListException : Exception
    {
        public EmptyListException(Exception inner) : base() { }
        public EmptyListException(string message, Exception inner) : base(message, inner) { }
    }

    [Serializable]
    public class DoesNotExistException : Exception
    {
        public DoesNotExistException(Exception inner) : base() { }
        public DoesNotExistException(string message, Exception inner) : base(message, inner) { }
    }
}