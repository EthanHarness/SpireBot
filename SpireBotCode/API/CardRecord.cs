using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Saves.Runs;

namespace SpireBot.SpireBotCode.API;

//TODO: Add rest of game state and probably refactor this
public record CardRecord(
    string Id,
    string Type,
    EnergyCostRecord EnergyCost,
    string TargetType,
    IEnumerable<String> Keywords,
    IEnumerable<String> Tags,
    Dictionary<string, DynamicVarRecord> DynamicVars,
    SerializableEnchantment? Enchantment,
    string? Affliction,
    bool IsUpgraded,
    int BaseReplayCount,
    bool ShouldRetainThisTurn,
    bool IsSlyThisTurn,
    bool GainsBlock,
    string OrbEvokeType,
    bool ExhaustOnNextPlay,
    int CurrentStarCost
    );