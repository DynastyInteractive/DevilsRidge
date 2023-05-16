using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "Talisman", menuName = "Items/Talisman")]
public class TalismanItem : EquippableItem
{
    [Tooltip("The modified swing vfx resulting from this talisman. (Required)")] 
    [SerializeField] VisualEffect _swingVisual;

    [Tooltip("Effect to play on the blade while talisman is equipped. (Optional)")] 
    [SerializeField] VisualEffect _bladeVisual;

    public VisualEffect SwingVisual => _swingVisual;
    public VisualEffect BladeVisual => _bladeVisual;
}
