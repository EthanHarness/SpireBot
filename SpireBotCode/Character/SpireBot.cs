using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using SpireBot.SpireBotCode.Extensions;
using Godot;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;

namespace SpireBot.SpireBotCode.Character;

//TODO: Bug with _character. I think restarting the game causes _character to change which affects potions generated. 
public class SpireBot : PlaceholderCharacterModel
{
    public const string CharacterId = "SpireBot";
    public override Color NameColor => ModelDb.Character<Ironclad>().NameColor;
    public override CharacterGender Gender => CharacterGender.Neutral;
    public override int StartingHp => 1;
    public override IEnumerable<CardModel> StartingDeck => ModelDb.Character<Ironclad>().StartingDeck;
    public override IReadOnlyList<RelicModel> StartingRelics => ModelDb.Character<Ironclad>().StartingRelics;
    public override CardPoolModel CardPool => ModelDb.Character<Ironclad>().CardPool;
    public override RelicPoolModel RelicPool => ModelDb.Character<Ironclad>().RelicPool;
    public override PotionPoolModel PotionPool => ModelDb.Character<Ironclad>().PotionPool;
    
    public static readonly Color Color = new("ffffff");

    /*  PlaceholderCharacterModel will utilize placeholder basegame assets for most of your character assets until you
        override all the other methods that define those assets.
        These are just some of the simplest assets, given some placeholders to differentiate your character with.
        You don't have to, but you're suggested to rename these images. */
    public override Control CustomIcon
    {
        get
        {
            var icon = NodeFactory<Control>.CreateFromResource(CustomIconTexturePath);
            icon.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
            return icon;
        }
    }

    public override string CustomIconTexturePath => "character_icon_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectIconPath => "char_select_char_name.png".CharacterUiPath();
    public override string CustomCharacterSelectLockedIconPath => "char_select_char_name_locked.png".CharacterUiPath();
    public override string CustomMapMarkerPath => "map_marker_char_name.png".CharacterUiPath();
    public override string CharacterSelectSfx => $"event:/sfx/characters/{this.PlaceholderID}/{this.PlaceholderID}_select";
}