using System;

namespace AccountManager.Providers.ValueObjects
{
    public struct DateRange
    {
        private readonly DateTime _dt;

        public DateTime Start => 
            new DateTime(_dt.Year, _dt.Month, 1);

        public DateTime End =>
            Start.AddMonths(1).AddDays(-1);

        public int TotalDays =>
            End.Day - Start.Day;

        public int Month =>
            _dt.Month;

        public int Year =>
            _dt.Year;

        public DateRange(DateTime dt)
        {
            _dt = dt;
        }

        public static DateRange Current =>
            new DateRange(DateTime.Now);

        public static DateRange Empty =>
            new DateRange(DateTime.MinValue);

        public bool IsEmpty =>
            _dt == DateTime.MinValue;

        public static DateRange Parse(DateTime dt)
        {
            return new DateRange(dt);
        }

        public static DateTime ToDateTime(DateRange dr)
        {
            return dr._dt;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DateRange))
            {
                return false;
            }

            var range = (DateRange)obj;
            return 
                   Start == range.Start &&
                   End == range.End &&
                   TotalDays == range.TotalDays &&
                   Month == range.Month &&
                   Year == range.Year &&
                   IsEmpty == range.IsEmpty;
        }

        public override int GetHashCode()
        {
            var hashCode = -1828684304;
            hashCode = hashCode * -1521134295 + base.GetHashCode();
            hashCode = hashCode * -1521134295 + Start.GetHashCode();
            hashCode = hashCode * -1521134295 + End.GetHashCode();
            hashCode = hashCode * -1521134295 + TotalDays.GetHashCode();
            hashCode = hashCode * -1521134295 + Month.GetHashCode();
            hashCode = hashCode * -1521134295 + Year.GetHashCode();
            hashCode = hashCode * -1521134295 + IsEmpty.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(DateRange dt, DateRange dt2)
        {
            if (dt.IsEmpty && dt2.IsEmpty)
                return true;

            return 
                dt.Month == dt2.Month && 
                dt.Year == dt2.Year && 
                dt.Start == dt2.Start && 
                dt.End == dt2.End;
        }

        public static bool operator !=(DateRange dt, DateRange dt2)
        {
            return !(dt == dt2);
        }
    }
}
