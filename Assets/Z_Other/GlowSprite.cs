using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(SpriteRenderer))]
public class GlowSprite : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color hdrColor = Color.white;

    SpriteRenderer sr;
    MaterialPropertyBlock block;

    void OnEnable()
    {
        sr = GetComponent<SpriteRenderer>();
        block = new MaterialPropertyBlock();
        Apply();
    }

    void OnValidate()
    {
        if (!sr) sr = GetComponent<SpriteRenderer>();
        if (block == null) block = new MaterialPropertyBlock();
        Apply();
    }

    void Apply()
    {
        sr.GetPropertyBlock(block);

        // í èÌêF
        block.SetColor("_Color", hdrColor);

        // î≠åıÅiURP LitånÅj
        block.SetColor("_EmissionColor", hdrColor);

        sr.SetPropertyBlock(block);
    }
}
