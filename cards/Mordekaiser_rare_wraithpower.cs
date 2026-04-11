using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Powers;
using Mordekaiser.power;

namespace Mordekaiser.cards;

public class Mordekaiser_rare_wraithpower() : CardModel(1, CardType.Skill, CardRarity.Rare, TargetType.Self)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Power",15m)];
    
    protected override IEnumerable<IHoverTip> ExtraHoverTips => [HoverTipFactory.FromPower<StrengthPower>()];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        await PowerCmd.Apply<Mordekaiser_wraithgivepower>(Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, cardPlay.Card);
    }

    public override string PortraitPath => $"res://images/card_portraits/{Id.Entry.ToLowerInvariant()}.png";

    protected override void OnUpgrade()
    {
        DynamicVars["Power"].UpgradeValueBy(5);
    }
    
}