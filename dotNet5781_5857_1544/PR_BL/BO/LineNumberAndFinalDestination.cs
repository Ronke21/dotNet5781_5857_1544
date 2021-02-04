using System;

namespace BO
{
    /// <summary>
    /// special class to fill the yellow sign of lines in stations.
    /// </summary>
    public class LineNumberAndFinalDestination
    {
        public int LineNumber { get; set; }
        public string FinalDestination { get; set; }
    }
}