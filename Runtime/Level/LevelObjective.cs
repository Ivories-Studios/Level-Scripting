using UnityEngine;

namespace IvoriesStudios.LevelScripting.Level
{
    [System.Serializable]
    public class LevelObjective
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public virtual string Name { get; protected set; }
        [field: SerializeField] public virtual string Description { get; protected set; }
        [field: SerializeField] public virtual Vector3 Position { get; protected set; }
    }
}
