namespace DO
{
    /// <summary>
    /// Enum representing status of the bus entity
    /// </summary>
    public enum Status
    { 
        Ready,
        MaintainSoon,
        During, 
        Refueling, 
        InMaintenance, 
        Unfit
    }

    /// <summary>
    /// Area of bus lines.
    /// </summary>
    public enum Area
    { 
        General,
        North,
        South,
        Center,
        Jerusalem
    }


}
