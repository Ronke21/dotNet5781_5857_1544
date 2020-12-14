using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dotNet5781_PR_5857_1544
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
