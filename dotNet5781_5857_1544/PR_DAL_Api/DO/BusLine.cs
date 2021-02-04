namespace DO
{
    /// <summary>
    /// This class represents a line number.
    /// primary key - buslineId, is given from key generator.
    /// Line number can repeat itself in lines with different stations.
    /// </summary>
    public class BusLine
    {
        public int BusLineId { get; set; }
        public int LineNumber { get; set; }
        public Area BusArea { get; set; }
        public int FirstStation { get; set; }
        public int LastStation { get; set; }
        public bool AllAccessible { get; set; }
        public bool Active { get; set; }
    }
}
