using System.ComponentModel.DataAnnotations.Schema;

namespace RPG.Models.RulebookModal.BaseTypes.PhysicalAppearances
{
    [ComplexType]
    public class ImperialWeight
    {
        public double  Lb { get;  set; }

        public ImperialWeight()
        {
        }
        public ImperialWeight(double lb)
        {
            Lb = lb;
        }

        public double InKilograms()
        {
            return Lb / 2.2;
        }

        public override string ToString()
        {
            return Lb + " lb.";
        }
    }
}
