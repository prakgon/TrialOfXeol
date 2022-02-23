using UnityEngine;
using System.Linq;

namespace Helpers
{
    /// <summary>
    /// Script to inject all Level-related dependencies
    /// </summary>
    public class DependencyInjector : MonoBehaviour
    {
        private DependencyInjector _parent;
        private void Start()
        {
            CreateParentDependencyInjector();
            ConfigureInjectionOnChildren();
        }

        private void CreateParentDependencyInjector()
        {
            if (transform.parent == null) return;
            _parent = transform.parent.GetComponent<DependencyInjector>();
            if (transform.parent.parent != null && _parent == null)
            {
                _parent = transform.parent.gameObject.AddComponent<DependencyInjector>();
            }
        }

        private void ConfigureInjectionOnChildren()
        {
            var injectorUsers = FindObjectsOfType<MonoBehaviour>().OfType<IInjectorUser>();
            Debug.Log(injectorUsers.Last().GetType());
            foreach (var yunk in injectorUsers)
            {
                yunk.ConfigureInjector(this);
            }
        }

        public T GetDependency<T>()
        {
            return _parent != null ? _parent.GetDependency<T>() : GetDependencyFromSceneController<T>();
        }

        private T GetDependencyFromSceneController<T>()
        {
            var sceneController = GetComponent<ISceneController>();
            return sceneController.GetDependencies<T>().First();
        }
    }
}
