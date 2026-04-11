using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.cards;

public class Mordekaiser_unc_purifywraith() : CardModel(1, CardType.Skill, CardRarity.Uncommon, TargetType.Self)
{
    public override bool GainsBlock => true;

    protected override HashSet<CardTag> CanonicalTags => [CardTag.Defend];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new CalculationBaseVar(0m),
        new CalculationExtraVar(5m),
        new CalculatedBlockVar(ValueProp.Move).WithMultiplier((_, c) => c != null && c.HasPower<FrailPower>() ? c.GetPower<FrailPower>()!.Amount : 0 )
    ];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        var gainblock = DynamicVars.CalculatedBlock.Calculate(Owner.Creature);
        await PowerCmd.Remove<FrailPower>(Owner.Creature);
        await CreatureCmd.GainBlock(Owner.Creature,gainblock,DynamicVars.CalculatedBlock.Props,cardPlay);
    }
    
    public override string PortraitPath => $"res://images/card_portraits/{Id.Entry.ToLowerInvariant()}.png";

    
    protected override void OnUpgrade()
    {
        DynamicVars.CalculationExtra.UpgradeValueBy(2m);
    }
    
}