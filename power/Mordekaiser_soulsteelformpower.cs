using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;

namespace Mordekaiser.power;

public class Mordekaiser_soulsteelformpower : PowerModel
{
    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;
        var cards = (Owner.Player.Piles.FirstOrDefault(p => p.Type == PileType.Exhaust)?.Cards)?.Count(_ => true);
        if (cards == null) return;
        await CreatureCmd.GainBlock(Owner,(decimal)cards * Amount, ValueProp.Unpowered,null);
    }
    
}