using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equippable : Item
{
    public enum EquipType { Body, Head };
    public EquipType type;

    [SerializeField]    
    private AnimationClip[] animationClips;

    public AnimationClip[] AnimationClips { get => animationClips; }

    public bool equipped = false;
}
