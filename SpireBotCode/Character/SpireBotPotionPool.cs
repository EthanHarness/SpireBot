using BaseLib.Abstracts;
using SpireBot.SpireBotCode.Extensions;
using Godot;

namespace SpireBot.SpireBotCode.Character;

public class SpireBotPotionPool : CustomPotionPoolModel
{
    public override Color LabOutlineColor => SpireBot.Color;


    public override string BigEnergyIconPath => "charui/big_energy.png".ImagePath();
    public override string TextEnergyIconPath => "charui/text_energy.png".ImagePath();
}