using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerScripts
{
    [CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/Player/Data", order = 1)]
    public class PlayerDataSO : ScriptableObject
    {
        public float maximumHealth;
    }
}