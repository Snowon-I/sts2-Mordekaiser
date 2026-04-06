using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;

namespace Mordekaiser.cards;

public class Mordekaiser_rare_collapse() : CardModel(2, CardType.Skill, CardRarity.Rare, TargetType.AllEnemies)
{
    public override IEnumerable<CardKeyword> CanonicalKeywords => [CardKeyword.Exhaust];

    protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
    {
        ArgumentNullException.ThrowIfNull(CombatState);
        foreach (var enemy in CombatState.HittableEnemies)
        {
            var halfHp = (dynamic)Math.Floor(enemy.MaxHp * 0.5);
            await CreatureCmd.LoseMaxHp(choiceContext, enemy, halfHp, true);
        }
    }

    public override string PortraitPath => $"res://images/packed/card_portraits/ironclad/anger.png";

    protected override void OnUpgrade()
    {
        EnergyCost.UpgradeBy(-1);
    }
    
}