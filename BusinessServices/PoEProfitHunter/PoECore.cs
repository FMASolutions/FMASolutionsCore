using System;

namespace FMASolutionsCore.BusinessServices.PoEProfitHunter
{
    public class PoECore
    {
        public static PoECore.ePoEItemList ConvertStringToPoEItem(string Name)
        {
            return (PoECore.ePoEItemList)Enum.Parse(typeof(PoECore.ePoEItemList), Name);
        }

        public enum ePoEItemList
        {
            Invalid,
            OrbOfAlteration,
            OrbOfFusing,
            OrbOfAlchemy,
            ChaosOrb,
            GemcuttersPrism,
            ExaltedOrb,
            ChromaticOrb,
            JewllersOrb,
            OrbOfChance,
            CartographersChisel,
            OrbOfScouring,
            BlessedOrb,
            OrbOfRegret,
            RegalOrb,
            DivineOrb,
            VaalOrb,
            ScrollOfWisdom,
            PortalSCroll,
            ArmourersScrap,
            BlacksmithsWhetstone,
            GlassBubble,
            OrbOfTransmutation,
            OrbOfAugmentation,
            MirrorOfKalandra,
            EternalOrb,
            PerandusCoin,
            SilverCoin,
            SacraficeAtDusk,
            SacraficeAtMidnight,
            SacraficeAtDawn,
            SacraficeAtNoon,
        }
    }
}
