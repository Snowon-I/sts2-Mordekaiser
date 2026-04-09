using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.power;
using Mordekaiser.scripts;

namespace Mordekaiser.cards;

public class Mordekaiser_rare_soulsteelform() : CardModel(3, CardType.Power, CardRarity.Uncommon, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Power",1m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<Mordekaiser_soulcallpower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<Mordekaiser_soulcallpower>(Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, this);
    }

    public override async Task OnEnqueuePlayVfx(Creature? target)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
    }

    public override async Task AfterCardExhausted(PlayerChoiceContext choiceContext, CardModel card, bool causedByEthereal)
    {
        if (card == this && CombatState != null && IsUpgraded )
        {
            CombatManager.Instance.History.MordekaiserQuiesceTrigger(CombatState,card);
            await PowerCmd.Apply<Mordekaiser_soulcallpower>(Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, this);
        }
    }
    
}