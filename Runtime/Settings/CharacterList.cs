using IvoriesStudios.LevelScripting.Dialogue;
using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterList : ScriptableObject
{
    [SerializeField] private List<Character> _list = new List<Character>();
    private static CharacterList _instance;
    public static CharacterList Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = AssetDatabase.LoadAssetAtPath<CharacterList>("Assets/_GGJ_2024/LevelScripting/Generated/CharacterList.asset");
            }
            return _instance;
        }
    }

    public void Initialize(List<Character> list)
    {
        foreach (Character character in list)
        {
            _list.Add(new Character(character.Name, character.Portrait));
        }
    }

    public static Character GetCharacter(Characters character)
    {
        return Instance._list[(int)character];
    }
}
