using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Screens.Timeline;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Timeline;

namespace Mordekaiser.Core.Timeline.Epochs;

public class Mordekaiser1Epoch : EpochModel
{
    public override string Id => "MORDEKAISER1_EPOCH";

    public override EpochEra Era => EpochEra.Prehistoria0;
    
    public override int EraPosition => 0;

    public override string StoryId => "Mordekaiser";

    public override bool IsArtPlaceholder => false;

    //public override EpochModel[] GetTimelineExpansion(){}

    public override void QueueUnlocks()
    {
        NTimelineScreen.Instance.QueueCharacterUnlock<Characters.Mordekaiser>(this);
        SaveManager.Instance.Progress.PendingCharacterUnlock = ModelDb.Character<Characters.Mordekaiser>().Id;
        QueueTimelineExpansion(GetTimelineExpansion());
    }
}