using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using Mordekaiser.scripts;
using Mordekaiser.Utils.CardUtils;

namespace Mordekaiser.cards;

public class Mordekaiser_unc_wraithecho() : CardModel(1, CardType.Attack, CardRarity.Uncommon, TargetType.AnyEnemy)
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [
        new DamageVar(7m, ValueProp.Move)
    ];
    
    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(cardPlay.Target);
        await DamageCmd.Attack(DynamicVars.Damage.BaseValue)
            .FromCard(this)
            .Targeting(cardPlay.Target)
            .Execute(choiceContext);
        await MordekaiserCardUtils.TriggerMordekaiserCardQuiesce(
            choiceContext,
            Owner,
            1,
            PileType.Exhaust.GetPile(Owner),
            cards => cards.Where(c => c.Keywords.Contains(MordekaiserKeyWord.MordekaiserQuiesce))
            );
    }
    
    public override string PortraitPath => $"res://images/card_portraits/{Id.Entry.ToLowerInvariant()}.png";
    
    protected override void OnUpgrade()
    {
        DynamicVars.Damage.UpgradeValueBy(5m);
    }
    
}