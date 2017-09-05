using System;

namespace RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances
{
    public class MetricDistance
    {
        public static readonly double Milimeter = 0.001;
        public static readonly double Centimeter = 0.01;
        public static readonly double Decimeter = 0.1;
        public static readonly double Meter = 1.0;
        public static readonly double Decameter = 10.0;
        public static readonly double Hectometer =100.0;
        public static readonly double Kilometer = 1000.0;

        public double _meters { get; set; }
        
        public MetricDistance(double meters)
        {
            _meters = meters;
        }

        public double ToMilimeters()
        {
            return _meters / Milimeter;
        }

        public double ToCentimeters()
        {
            return _meters / Centimeter;
        }

        public double ToDecimeters()
        {
            return _meters / Decimeter;
        }

        public double ToMeters()
        {
            return _meters;
        }

        public double ToDecameters()
        {
            return _meters / Decameter;
        }

        public double ToHectometers()
        {
            return _meters / Hectometer;
        }

        public double ToKilometers()
        {
            return _meters / Kilometer;
        }

        public ImperialDistance ToImperialDistance()
        {
            return new ImperialDistance
            {
                _inches = _meters * 39.3701
            };
        }

        public override int GetHashCode()
        {
            return _meters.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var o = obj as MetricDistance;
            if (o == null) return false;
            return _meters.Equals(o._meters);
        }

        public static bool operator ==(MetricDistance a, MetricDistance b)
        {
            // If both are null, or both are same instance, return true
            if (ReferenceEquals(a, b)) return true;

            // if either one or the other are null, return false
            if (ReferenceEquals(a, null) || ReferenceEquals(b, null)) return false;

            return a._meters == b._meters;
        }

        public static bool operator !=(MetricDistance a, MetricDistance b)
        {
            return !(a == b);
        }

        public static MetricDistance operator +(MetricDistance a, MetricDistance b)
        {
            if (a == null) throw new ArgumentNullException("a");
            if (b == null) throw new ArgumentNullException("b");
            return new MetricDistance(a._meters + b._meters);
        }

        public static MetricDistance operator -(MetricDistance a, MetricDistance b)
        {
            if (a == null) throw new ArgumentNullException("a");
            if (b == null) throw new ArgumentNullException("b");
            return new MetricDistance(a._meters - b._meters);
        }

        public static MetricDistance operator *(MetricDistance a, MetricDistance b)
        {
            if (a == null) throw new ArgumentNullException("a");
            if (b == null) throw new ArgumentNullException("b");
            return new MetricDistance(a._meters * b._meters);
        }

        public static MetricDistance operator /(MetricDistance a, MetricDistance b)
        {
            if (a == null) throw new ArgumentNullException("a");
            if (b == null) throw new ArgumentNullException("b");
            return new MetricDistance(a._meters / b._meters);
        }
    }
}
