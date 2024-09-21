using IvoriesStudios.LevelScripting.Dialogue;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : ScriptableObject
{
    [SerializeField] private List<Character> _list = new List<Character>();
    public static CharacterList Instance { get; private set; }

    public void Initialize(List<Character> list)
    {
        foreach (Character character in list)
        {
            _list.Add(new Character(character.Name, character.Portrait));
        }
        Instance = this;
    }

    public static Character GetCharacter(Characters character)
    {
        return Instance._list[(int)character];
    }
}
