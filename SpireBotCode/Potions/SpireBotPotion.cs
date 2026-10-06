using BaseLib.Abstracts;
using BaseLib.Extensions;
using BaseLib.Utils;
using SpireBot.SpireBotCode.Character;
using SpireBot.SpireBotCode.Extensions;

namespace SpireBot.SpireBotCode.Potions;

[Pool(typeof(SpireBotPotionPool))]
public abstract class SpireBotPotion : CustomPotionModel
{
    public override string? CustomPackedImagePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionImagePath();

    public override string? CustomPackedOutlinePath =>
        $"{Id.Entry.RemovePrefix().ToLowerInvariant()}.png".PotionOutlineImagePath();
}