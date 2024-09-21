using UnityEngine;

namespace IvoriesStudios.LevelScripting.Dialogue
{
    [System.Serializable]
    public class Character
    {
        [field: SerializeField] public string Name { get; set; }
        [field: SerializeField] public Sprite Portrait { get; set; }

        public Character() { }
        public Character(string name, Sprite portrait)
        {
            Name = name;
            Portrait = portrait;
        }
    }
}
