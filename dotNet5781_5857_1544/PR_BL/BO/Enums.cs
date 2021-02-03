namespace BO
{
    public enum Status
    { 
        Ready,
        MaintainSoon,
        During, 
        Refueling, 
        InMaintenance, 
        Unfit
    }

    public enum Area
    { 
        General,
        North,
        South,
        Center,
        Jerusalem
    }

    public enum Authorization
    {
        Manager,
        User
    }
}
