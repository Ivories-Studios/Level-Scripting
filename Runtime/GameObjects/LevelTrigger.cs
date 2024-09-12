using UnityEngine;
using IvoriesStudios.LevelScripting.Process;

namespace IvoriesStudios.LevelScripting
{
    public class LevelTrigger : MonoBehaviour
    {
        #region Editor Variables
        [SerializeField] private Triggers _trigger;
        #endregion

        private void OnTriggerEnter(Collider other)
        {
            LevelObject.Trigger(_trigger);
        }
    }
}
