using HarmonyLib;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using SpireBot.SpireBotCode.Relics;

namespace SpireBot.SpireBotCode.Patches;

[HarmonyPatch(typeof(Player), "PopulateRelics", new[] { typeof(IEnumerable<RelicModel>), typeof(bool) })]
public static class SpireBotRelicPatch
{
    // ReSharper disable once InconsistentNaming
    public static void Prefix(Player __instance, ref IEnumerable<RelicModel> relics, bool silent)
    {
        MainFile.Logger.Info("Adding the SpireBots holy grail. The SpireBotBot....... bitch.");

        ulong playerId = __instance.NetId;
        
        if (!SpireBotRandomizationState.SpireBotPlayers.Contains(playerId)) return;
        MainFile.Logger.Info($"{playerId} in map.");
        
        RelicModel spireBotRelic = ModelDb.Relic<SpireBotBot>().ToMutable();
        relics = relics.Append(spireBotRelic);
    }
}