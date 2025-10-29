using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class OnDestroyEventProvider : MonoBehaviour
    {
        public UnityEvent OnDestroyed = new();

        private void OnDestroy()
        {
            OnDestroyed.Invoke();
        }
    }
}
