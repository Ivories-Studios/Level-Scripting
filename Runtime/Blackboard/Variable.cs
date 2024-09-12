using System;
using UnityEngine;

namespace IvoriesStudios.LevelScripting.Blackboard
{
    public enum VariableType
    {
        Integer,
        Float,
        String,
        Boolean,
    }

    [Serializable]
    public class Variable
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public VariableType Type { get; private set; }
        [field: SerializeField] public object Value { get; set; }

        public static object GetDefault(VariableType type)
        {
            return type switch
            {
                VariableType.Integer => default(int),
                VariableType.Float => default(float),
                VariableType.String => default(string),
                VariableType.Boolean => default(bool),
                _ => throw new NotImplementedException()
            };
        }
    }
}
