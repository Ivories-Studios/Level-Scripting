using System.Collections.Generic;
using UnityEngine;

namespace IvoriesStudios.LevelScripting.Blackboard
{
    [CreateAssetMenu(fileName = "Blackboard", menuName = "Level Scripting/Blackboard")]
    public class Blackboard : ScriptableObject
    {
        #region Editor Variables
        [SerializeField] private List<Variable> _variables = new List<Variable>();
        #endregion

        #region Public Methods
        public Variable GetVariable(int index)
        {
            return _variables[index];
        }

        public T GetVariable<T>(string name)
        {
            return default(T);
        }

        public void SetVariable(string name, object value)
        {

        }

        public void Save()
        {

        }
        #endregion
    }
}
