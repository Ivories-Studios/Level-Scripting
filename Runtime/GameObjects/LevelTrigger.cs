using UnityEngine;
using UnityEngine.Events;
using IvoriesStudios.LevelScripting.Process;

namespace IvoriesStudios.LevelScripting
{
    public class LevelTrigger : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private Triggers _trigger;
        [SerializeField] private UnityEvent _event;
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            LevelObject.Trigger(_trigger);
            _event?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
