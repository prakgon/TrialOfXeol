using System.Collections.Generic;
namespace Helpers
{ public interface ISceneController { IEnumerable<T> GetDependencies<T>(); } }
