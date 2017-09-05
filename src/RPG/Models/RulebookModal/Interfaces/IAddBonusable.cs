using System.Collections.Generic;
using RPG.Models.RulebookModal.BaseTypes.Bonuses;

namespace RPG.Models.RulebookModal.Interfaces
{
    public interface IAddBonusable : IRoundable
    {
        void AddBonues(List<Bonus> bonues);
        void RemoveBonues(List<Bonus> bonues);

    }
}
