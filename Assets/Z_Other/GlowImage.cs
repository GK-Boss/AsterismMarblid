using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Image))]
public class GlowImage : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color hdrColor = Color.white;

    private Image _image;
    // UIの場合、シェーダー内の変数名は一般的に "_Color" です
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    void OnValidate() => Apply();
    void Start() => Apply();

    void Apply()
    {
        if (_image == null) _image = GetComponent<Image>();

        // 1. Imageコンポーネント自体の色を更新（アルファ値などのため）
        _image.color = hdrColor;

        // 2. マテリアルに対してHDRカラーを直接適用
        // UIの場合、instantiated materialを作成して適用します
        if (_image.material != null && _image.material != _image.defaultMaterial)
        {
            _image.material.SetColor(ColorProperty, hdrColor);
        }
    }
}