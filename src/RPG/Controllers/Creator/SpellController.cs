using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RPG.Models.ControllerDto.Creator;
using RPG.Models.RulebookModal.BaseTypes.Damages;
using RPG.Models.RulebookModal.BaseTypes.Dice;
using RPG.Models.RulebookModal.BaseTypes.SpecialAbilities;
using RPG.Models.RulebookModal.BaseTypes.Spells;

namespace RPG.Controllers.Creator
{
    public class SpellController : ControllerBase
    {
        //
        // GET: /Spell/

        public ActionResult Index(Guid? id)
        {
            return View("Creator/Spell", "CreatorLayoutPage", GetSpell(id));
        }

        private SpellDataDto GetSpell(Guid? id)
        {
            return new SpellDataDto(Context, id);
        }

        public ActionResult Delete(Guid? id)
        {
            if (id.HasValue)
            {
                Context.Delete<Spell>(id.Value);
            }
            return View("Creator/Spell", "CreatorLayoutPage", GetSpell(null));
        }

        [HttpPost]
        public ActionResult Save(SpellDataDto data)
        {
            Spell spellToUpdate;

            if (data.SelectedItem.ID == Guid.Empty)
            {
                spellToUpdate = data.SelectedItem;
            }
            else
            {
                spellToUpdate = Context.Get<Spell>(data.SelectedItem.ID);
            }
            if (data.DamageType.HasValue && !string.IsNullOrEmpty(data.DamageString))
            {
                if (spellToUpdate.Damage == null)
                {
                    spellToUpdate.Damage = new SpellDamage();
                    spellToUpdate.Damage.Damage = new Damage();
                }
                spellToUpdate.Damage.Damage.Type = data.DamageType.Value;
                var newDiceRoll = DiceRoll.Parse(data.DamageString);
                if (spellToUpdate.Damage.Damage.Amount == null)
                {
                    spellToUpdate.Damage.Damage.Amount = newDiceRoll;
                }
                else
                {
                    spellToUpdate.Damage.Damage.Amount.FixedAmount = newDiceRoll.FixedAmount;
                    spellToUpdate.Damage.Damage.Amount.Dice = newDiceRoll.Dice;
                }
            }//TODO: Else if by caster level
            else
            {
                spellToUpdate.Damage = null;
            }


            spellToUpdate.Range = data.SelectedItem.Range;
            spellToUpdate.School = data.SpellSchool.HasValue ? Context.Get<SpellSchool>(data.SpellSchool.Value) : null;
            spellToUpdate.SpellAbility = data.SpellAbility.HasValue
                ? Context.Get<SpecialAbility>(data.SpellAbility.Value)
                : null;
            spellToUpdate.SpellResistance = data.SelectedItem.SpellResistance;
            spellToUpdate.SpellSaveType = data.SelectedItem.SpellSaveType;
            spellToUpdate.Target = data.SelectedItem.Target;
            spellToUpdate.CastingTime = data.SelectedItem.CastingTime;
            spellToUpdate.Descriptor = data.SpellDescriptor != null && data.SpellDescriptor.Length == 1 ? Context.Get<SpellDescriptor>(data.SpellDescriptor[0]) : null;
            spellToUpdate.FixedRangeExpresedInFeet = data.SelectedItem.FixedRangeExpresedInFeet;
            spellToUpdate.Name = data.SelectedItem.Name;
            spellToUpdate.Description = data.SelectedItem.Description;
            //spellToUpdate.CasterRequirements 
            spellToUpdate.ComponentRequirements = data.Components != null && data.Components.Length > 0
                ? Context.GetList<SpellComponent>(data.Components)
                : new List<SpellComponent>();
            
            Context.CreateOrUpdate(spellToUpdate);
            return RedirectToAction("Index", "Spell", new { id = spellToUpdate.ID });
            //return View("Creator/Spell", "CreatorLayoutPage", GetSpell(spellToUpdate.ID));
        }
    }
}
