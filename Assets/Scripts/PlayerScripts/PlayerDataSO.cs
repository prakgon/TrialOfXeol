using UnityEngine;
using static Helpers.LiteralToStringParse;


namespace PlayerScripts
{
    [CreateAssetMenu(fileName = PlayerData, menuName = PlayerDataPath, order = 1)]
    public class PlayerDataSO : ScriptableObject
    {
        public int increasedHealthByLevel;
        public int increasedStaminaByLevel;
        public int increasedManaByLevel;
    }
}