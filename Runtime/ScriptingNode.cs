using IvoriesStudios.LevelScripting.Attributes;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace IvoriesStudios.LevelScripting
{
    [System.Serializable]
    public class ScriptingNode
    {
        #region Variables
        [SerializeField] private string _guid;
        [SerializeField] private Rect _position;
        #endregion

        #region Properties
        public string TypeName { get; set; }
        public bool IsStopping { get; private set; }
        public string Id => _guid;
        public Rect Position => _position;
        #endregion

        #region Lifecycle
        public ScriptingNode()
        {
            NewGUID();
            IsStopping = GetType().GetCustomAttribute<NodeInfoAttribute>().IsStopping;
        }
        #endregion

        #region Public Methods
        public void SetPosition(Rect position)
        {
            _position = position;
        }

        public virtual string[] OnProcess(LevelScript currentSequence)
        {
            return currentSequence.GetNodeFromOutput(_guid, 0).Select((scriptingNode) => scriptingNode.Id).ToArray();
        }
        #endregion

        #region Private Methods
        private void NewGUID()
        {
            _guid = System.Guid.NewGuid().ToString();
        }
        #endregion
    }
}
