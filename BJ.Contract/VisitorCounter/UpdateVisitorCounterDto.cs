namespace BJ.Contract.VisitorCounter
{
    public class UpdateVisitorCounterDto
    {
        public long DayCount { get; set; }

        public long MonthCount { get; set; }

        public long YearCount { get; set; }

        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
