using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances
{
    [ComplexType]
    public class ImperialDistance
    {
        [NotMapped]
        public static readonly double Inch = 1.0;
        [NotMapped]
        public static readonly double Foot = 12.0;
        [NotMapped]
        public static readonly double Yard = 36.0;
        [NotMapped]
        public static readonly double Mile = 63360.0;

        public double _inches { get; set; }
    
        public double ToInches()
        {
            return _inches;
        }

        public double ToFeet()
        {
            return _inches / Foot;
        }

        public double ToYards()
        {
            return _inches / Yard;
        }

        public double ToMiles()
        {
            return _inches / Mile;
        }

        public MetricDistance ToMetricDistance()
        {
            return new MetricDistance(_inches * 0.0254);
        }

        public override int GetHashCode()
        {
            return _inches.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var o = obj as ImperialDistance;
            if (o == null) return false;
            return _inches.Equals(o._inches);
        }

        public static bool operator ==(ImperialDistance a, ImperialDistance b)
        {
            // If both are null, or both are same instance, return true
            if (ReferenceEquals(a, b)) return true;

            // if either one or the other are null, return false
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

            // compare
            return a._inches == b._inches;
        }

        public static bool operator !=(ImperialDistance a, ImperialDistance b)
        {
            return !(a == b);
        }

        public static ImperialDistance operator +(ImperialDistance a, ImperialDistance b)
        {
            if (a == null) throw new ArgumentNullException();
            if (b == null) throw new ArgumentNullException();
            return new ImperialDistance
            {
                _inches = a._inches + b._inches
            };
        }

        public static ImperialDistance operator -(ImperialDistance a, ImperialDistance b)
        {
            if (a == null) throw new ArgumentNullException();
            if (b == null) throw new ArgumentNullException();
            return new ImperialDistance
            {
                _inches = a._inches - b._inches
            };
        }

        public static ImperialDistance operator *(ImperialDistance a, ImperialDistance b)
        {
            if (a == null) throw new ArgumentNullException();
            if (b == null) throw new ArgumentNullException();
            return new ImperialDistance
            {
                _inches = a._inches * b._inches
            };
        }

        public static ImperialDistance operator /(ImperialDistance a, ImperialDistance b)
        {
            if (a == null) throw new ArgumentNullException();
            if (b == null) throw new ArgumentNullException();
            return new ImperialDistance
            {
                _inches = a._inches / b._inches
            };
        }

        public override string ToString()
        {
            return string.Format("{0}ft {1}inc", (int)ToFeet(), _inches % 12);
        }
    }
}
