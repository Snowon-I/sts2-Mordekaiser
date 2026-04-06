using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Powers;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;

namespace Mordekaiser.power;

public class Mordekaiser_wraithgivepower : PowerModel
{
    protected override IEnumerable<DynamicVar> CanonicalVars => [];

    public override PowerType Type => PowerType.Buff;

    public override PowerStackType StackType => PowerStackType.Counter;

    public override async Task AfterPlayerTurnStart(PlayerChoiceContext choiceContext, Player player)
    {
        if (player != Owner.Player) return;
        if (Owner.CombatState == null) return;
        Flash();
        await PowerCmd.Apply<Mordekaiser_wraithgetpower>(Owner,Amount,Owner,null);
        await PowerCmd.Remove<Mordekaiser_wraithgivepower>(Owner);
    }
    
}