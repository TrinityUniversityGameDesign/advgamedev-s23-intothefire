using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName="Into the Fire/Character")]
public class CharacterData : ScriptableObject
{
    // Start is called before the first frame update
    [Header("Character Settings")]
    public string Description;

    [Tooltip("Render texture for the rotating effect")]
    public RenderTexture renderTexture;
    
    [Tooltip("Game object to instantiate for player")]
    public GameObject gameObject;
}