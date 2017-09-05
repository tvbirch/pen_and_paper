using System.Collections.Generic;
using System.Linq;

namespace RPG.Models.RulebookModal.Lists
{
    public class LevelableList<T>// where T : IRoundable
    {
        public Dictionary<int,List<T>> AbilitiesByLevel = new Dictionary<int,List<T>>();

        public List<T> this[int level]
        {
            get 
            {
                var keysToTake = AbilitiesByLevel.Keys.Where(x => x <= level);
                var newList = new List<T>();
                foreach (var key in keysToTake)
                {
                    newList.AddRange(AbilitiesByLevel[key]);
                }

                return newList;
            }            
        }
    }
}
