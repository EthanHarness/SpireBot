using BaseLib.Abstracts;
using BaseLib.Utils.NodeFactories;
using SpireBot.SpireBotCode.Extensions;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Characters;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Characters;
using SpireBot.SpireBotCode.Relics;

namespace SpireBot.SpireBotCode.Character;

//TODO: Bug with _character. I think restarting the game causes _character to change which affects potions generated. 
public class SpireBot : PlaceholderCharacterModel
{
    public const string CharacterId = "SpireBot";

    private CharacterModel _character;
    private IReadOnlyList<RelicModel> _relicWrapper;
    
    private static CharacterModel set_character()
    {
        var random = new Random();
        return random.Next(1, 6) switch
        {
            1 => ModelDb.Character<Ironclad>(),
            2 => ModelDb.Character<Silent>(),
            3 => ModelDb.Character<Regent>(),
            4 => ModelDb.Character<Necrobinder>(),
            5 => ModelDb.Character<Defect>(),
            _ => ModelDb.Character<Regent>()
        };
    }
    
    public SpireBot()
    {
       _character = set_character();
       _relicWrapper = [.. _character.StartingRelics, ModelDb.Relic<SpireBotBot>()];
    }
    public override Color NameColor => _character.NameColor;
    public override CharacterGender Gender => _character.Gender;
    public override int StartingHp => _character.StartingHp;
    public override IEnumerable<CardModel> StartingDeck => _character.StartingDeck;
    public override IReadOnlyList<RelicModel> StartingRelics => _relicWrapper;
    public override CardPoolModel CardPool => _character.CardPool;
    public override RelicPoolModel RelicPool => _character.RelicPool;
    public override PotionPoolModel PotionPool => _character.PotionPool;
    
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

    public override string CharacterSelectSfx
    {
        get
        {
            _character = set_character();
            _relicWrapper = [.. _character.StartingRelics, ModelDb.Relic<SpireBotBot>()];
            return $"event:/sfx/characters/{this.PlaceholderID}/{this.PlaceholderID}_select";
        } 
    }
}