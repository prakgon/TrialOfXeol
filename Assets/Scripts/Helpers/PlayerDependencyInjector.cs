using UnityEngine;

namespace Helpers
{
    public class PlayerDependencyInjector : DependencyInjector
    {
        [SerializeField] private GameObject[] _injectorUsers;
        protected override void ConfigureInjectionOnChildren()
        {
            foreach (var yunk in _injectorUsers)
            {
                //yunk.GetComponent<IInjectorUser>().ConfigureInjector(this);
            }
        }
    }
}