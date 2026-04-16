using MegaCrit.Sts2.Core.Timeline;
using Mordekaiser.Core.Timeline.Epochs;

namespace Mordekaiser.Core.Timeline.Stories;

public class MordekaiserStory : StoryModel
{
    protected override string Id => "MORDEKAISER";

    public override EpochModel[] Epochs => [EpochModel.Get<Mordekaiser1Epoch>()];
    
}