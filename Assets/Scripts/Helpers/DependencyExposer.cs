using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;

namespace Helpers
{
    /// <summary>
    /// Script to expose and access all Level-related dependencies
    /// </summary>
    public class DependencyExposer : MonoBehaviour
    {


        public IEnumerable<T> GetDependencies<T>()
        {
            FieldInfo[] myFieldInfo;
            Type myType = typeof(DependencyExposer);
            // Get the type and fields of FieldInfoClass.
            myFieldInfo = myType.GetFields(BindingFlags.NonPublic | BindingFlags.Instance
                | BindingFlags.Public);

            if (myFieldInfo.Any(x => x.FieldType == typeof(T)))
            {
                var dependencies = myFieldInfo.Where(x => x.FieldType == typeof(T));
                foreach (var dependency in dependencies)
                {
                    yield return (T)dependency.GetValue(this);
                }
            }
            yield return GetComponent<T>();
        }
    }
}
