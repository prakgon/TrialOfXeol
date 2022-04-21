using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Helpers.LiteralToStringParse;

namespace Configuration
{
    [CreateAssetMenu(fileName = MultiplayerConfiguration, menuName = MultiplayerConfigurationPath, order = 1)]
    public class MultiplayerConfigurationSO : ScriptableObject
    {
        public byte maxPlayersPerRoom;
        public byte maxFighters;
        public byte maxSpectators;
    }
}