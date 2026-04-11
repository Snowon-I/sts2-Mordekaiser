using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Mordekaiser.power;

namespace Mordekaiser.cards;

public class Mordekaiser_rare_absolutewill() : CardModel(1, CardType.Power, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new("Power",1m)];

    protected override IEnumerable<IHoverTip> ExtraHoverTips => [
        HoverTipFactory.FromPower<StrengthPower>(),
        HoverTipFactory.FromPower<DexterityPower>(),
        HoverTipFactory.FromPower<Mordekaiser_absolutewillpower>()
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<StrengthPower>(Owner.Creature,DynamicVars["Power"].BaseValue,Owner.Creature,this);
        await PowerCmd.Apply<DexterityPower>(Owner.Creature,DynamicVars["Power"].BaseValue,Owner.Creature,this);
        await PowerCmd.Apply<Mordekaiser_absolutewillpower>(Owner.Creature, 1m, Owner.Creature, this);
    }

    public override async Task OnEnqueuePlayVfx(Creature? target)
    {
        await CreatureCmd.TriggerAnim(Owner.Creature, "Cast", Owner.Character.CastAnimDelay);
    }

    protected override void OnUpgrade()
    {
        DynamicVars["Power"].UpgradeValueBy(1m);
    }
    
}