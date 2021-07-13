using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationUtils : MonoBehaviour
{
    /// <summary>
    /// Function return invert version of AnimationCurve by time.
    /// </summary>
    /// <param name="curve">Origin AnimationCurve</param>
    /// <returns>Invert AnimationCurve</returns>
    AnimationCurve InvertAnimationCurve(AnimationCurve curve)
    {
        AnimationCurve result = new AnimationCurve();
        foreach (var item in curve.keys)
        {
            result.AddKey(item.value, item.time);
        }
        return result;
    }
}
