using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
[RequireComponent(typeof(Text))]
public class GlowText : MonoBehaviour
{
    [ColorUsage(true, true)]
    public Color hdrColor = Color.white;

    private Text _text;
    private Material _instancedMaterial;

    // UI/Defaultシェーダーなどの色プロパティ名
    private static readonly int ColorProperty = Shader.PropertyToID("_Color");

    void OnEnable() => Apply();
    void OnValidate() => Apply();

    void Update()
    {
        // エディタ実行中でなくても反映されるようにUpdateでも呼ぶ（必要に応じて）
        if (!Application.isPlaying) Apply();
    }

    void Apply()
    {
        if (_text == null) _text = GetComponent<Text>();

        // 1. テキストコンポーネント自体の色を更新
        _text.color = hdrColor;

        // 2. マテリアルへの反映
        // Text.materialにアクセスすると自動でインスタンス化されます
        if (_text.material != null && _text.material != _text.defaultMaterial)
        {
            _text.material.SetColor(ColorProperty, hdrColor);
        }
    }

    private void OnDestroy()
    {
        // メモリリーク防止のため、生成されたマテリアルを破棄
        if (_instancedMaterial != null)
        {
            DestroyImmediate(_instancedMaterial);
        }
    }
}