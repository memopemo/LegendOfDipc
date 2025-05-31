using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerColor", menuName = "Player Color", order = 3)]
public class PlayerColors : ScriptableObject
{
    [Serializable]
    public struct Palette
    {
        public Color white;
        public Color light;
        public Color dark;
        public Color black;
    }
    public Palette[] swordPalettes;
    public Palette[] shieldPalettes;
}
