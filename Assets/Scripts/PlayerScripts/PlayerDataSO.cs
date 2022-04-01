using UnityEngine;
using static Helpers.LiteralToStringParse;


namespace PlayerScripts
{
    [CreateAssetMenu(fileName = PlayerData, menuName = PlayerDataPath, order = 1)]
    public class PlayerDataSO : ScriptableObject
    {
        public float maximumHealth;
        public float maximumStamina;
    }
}