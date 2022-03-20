using UnityEngine;

namespace DummyScripts
{
    [CreateAssetMenu(fileName = "DummyData", menuName = "ScriptableObjects/Dummy/Data", order = 1)]
    public class DummyDataSO : ScriptableObject
    {
        public float maximumHealth;
    }
}