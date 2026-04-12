using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using Mordekaiser.power;

namespace Mordekaiser.cards;

public sealed class Mordekaiser_rare_finaljudgment() : CardModel(2, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];
    
    protected override IEnumerable<DynamicVar> CanonicalVars => [new ("Power",10m)];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);
        await PowerCmd.Apply<Mordekaiser_finaljudgmentpower>(Owner.Creature, DynamicVars["Power"].BaseValue, Owner.Creature, this);
    }

    public override string PortraitPath => $"res://images/card_portraits/{Id.Entry.ToLowerInvariant()}.png";

    protected override void OnUpgrade()
    {
        DynamicVars["Power"].UpgradeValueBy(-3m);
    }
    
}