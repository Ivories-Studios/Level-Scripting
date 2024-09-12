using UnityEngine;

namespace IvoriesStudios.LevelScripting.Dialogue
{
    [System.Serializable]
    public class Character
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite Portrait { get; set; }
    }
}
