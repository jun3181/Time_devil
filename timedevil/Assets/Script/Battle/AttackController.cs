using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    public enum Panel { Enemy, Player }

    [System.Serializable]
    public struct CellPos { public float x; public float y; }

    [Header("Enemy 패널(오른쪽) 16셀 좌표 0~15")]
    public List<CellPos> enemyCells = new List<CellPos>(16);

    [Header("Player 패널(왼쪽) 16셀 좌표 0~15")]
    public List<CellPos> playerCells = new List<CellPos>(16);

    [Header("Marker (Resources 경로)")]
    public string markerSpritePath = "my_asset/attack";
    public int sortingOrder = 40;

    [Header("Fade (sec)")]
    public float fadeIn = 0.6f;
    public float hold = 0.1f;
    public float fadeOut = 0.6f;

    readonly List<SpriteRenderer> _spawned = new();

    public void ShowPattern(string pattern16) => ShowPattern(pattern16, Panel.Enemy);

    public void ShowPattern(string pattern16, Panel target)
    {
        if (string.IsNullOrEmpty(pattern16) || pattern16.Length != 16)
        {
            Debug.LogError("[AttackController] pattern16 length must be 16");
            return;
        }
        StartCoroutine(Co_Show(pattern16, target));
    }

    IEnumerator Co_Show(string pattern16, Panel target)
    {
        foreach (var sr in _spawned) if (sr) Destroy(sr.gameObject);
        _spawned.Clear();

        var sprite = Resources.Load<Sprite>(markerSpritePath);
        if (!sprite) yield break;

        var cells = (target == Panel.Enemy) ? enemyCells : playerCells;

        for (int i = 0; i < 16; i++)
        {
            if (pattern16[i] != '1') continue;
            if (i >= cells.Count) continue;

            var pos = new Vector3(cells[i].x, cells[i].y, 0f);
            var go = new GameObject($"attack ({target}:{i})");
            go.transform.position = pos;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingLayerName = "Default";
            sr.sortingOrder = sortingOrder;
            sr.color = new Color(1, 1, 1, 0);
            _spawned.Add(sr);
        }

        float t = 0f;
        if (fadeIn > 0f)
        {
            while (t < fadeIn) { t += Time.deltaTime; SetAlpha(Mathf.Clamp01(t / fadeIn)); yield return null; }
        }
        else SetAlpha(1f);

        if (hold > 0f) yield return new WaitForSeconds(hold);

        t = 0f;
        if (fadeOut > 0f)
        {
            while (t < fadeOut) { t += Time.deltaTime; SetAlpha(1f - Mathf.Clamp01(t / fadeOut)); yield return null; }
        }
        else SetAlpha(0f);

        foreach (var sr in _spawned) if (sr) Destroy(sr.gameObject);
        _spawned.Clear();

        // 턴 종료는 호출 쪽(플레이어/적)이 책임지도록 남겨둠
    }

    void SetAlpha(float a)
    {
        for (int i = 0; i < _spawned.Count; i++)
        {
            if (!_spawned[i]) continue;
            var c = _spawned[i].color; c.a = a; _spawned[i].color = c;
        }
    }

    public void ClearImmediate()
    {
    StopAllCoroutines();
    for (int i = 0; i < _spawned.Count; i++)
        if (_spawned[i]) Destroy(_spawned[i].gameObject);
    _spawned.Clear();
    }
}
