using UnityEngine;
using IvoriesStudios.LevelScripting.Attributes;

namespace IvoriesStudios.LevelScripting.Level
{
    [NodeInfo("Instantiate Object", "Level/Instantiate Object")]
    public class InstantiateObject : ScriptingNode
    {
        [ExposedProperty] public GameObject Object;
        [ExposedProperty] public Vector3 Location;
        [ExposedProperty] public Vector3 Rotation;

        public override string[] OnProcess(LevelScript currentSequence)
        {
            GameObject.Instantiate(Object, Location, Quaternion.Euler(Rotation));
            return base.OnProcess(currentSequence);
        }
    }
}
