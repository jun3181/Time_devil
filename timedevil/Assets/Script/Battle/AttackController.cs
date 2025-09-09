using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [System.Serializable]
    public struct CellPos { public float x; public float y; }

    [Header("EnemyPanel 16셀 월드 좌표 (인덱스 0~15)")]
    public List<CellPos> cellPositions = new List<CellPos>(16);

    [Header("Marker (Resources 경로)")]
    public string markerSpritePath = "my_asset/attack";
    public int sortingOrder = 40;

    [Header("Fade (sec)")]
    public float fadeIn = 0.6f;
    public float hold = 0.1f;
    public float fadeOut = 0.6f;

    private readonly List<SpriteRenderer> _spawned = new();

    public void ShowPattern(string pattern16)
    {
        if (string.IsNullOrEmpty(pattern16) || pattern16.Length != 16)
        {
            Debug.LogError("[AttackController] pattern16은 정확히 16글자여야 함");
            return;
        }

        StartCoroutine(Co_Show(pattern16));
    }

    private IEnumerator Co_Show(string pattern16)
    {
        // 정리
        foreach (var sr in _spawned) if (sr) Destroy(sr.gameObject);
        _spawned.Clear();

        var sprite = Resources.Load<Sprite>(markerSpritePath);
        if (sprite == null)
        {
            Debug.LogError($"[AttackController] 마커 스프라이트 못 찾음: {markerSpritePath}");
            yield break;
        }

        // 1) 마커 생성 (알파 0으로 시작)
        for (int i = 0; i < 16; i++)
        {
            if (pattern16[i] != '1') continue;
            if (i >= cellPositions.Count) { Debug.LogWarning($"셀 좌표 없음: {i}"); continue; }

            var pos = new Vector3(cellPositions[i].x, cellPositions[i].y, 0f);
            var go = new GameObject($"attack ({i})");
            go.transform.position = pos;

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.sortingLayerName = "Default";
            sr.sortingOrder = sortingOrder;
            sr.color = new Color(1, 1, 1, 0);
            _spawned.Add(sr);
        }

        // 2) 페이드 인
        if (fadeIn > 0f)
        {
            float t = 0;
            while (t < fadeIn)
            {
                t += Time.deltaTime;
                float a = Mathf.Clamp01(t / fadeIn);
                SetAlpha(a);
                yield return null;
            }
        }
        else SetAlpha(1f);

        // 3) 유지
        if (hold > 0f) yield return new WaitForSeconds(hold);

        // 4) 페이드 아웃
        if (fadeOut > 0f)
        {
            float t = 0;
            while (t < fadeOut)
            {
                t += Time.deltaTime;
                float a = 1f - Mathf.Clamp01(t / fadeOut);
                SetAlpha(a);
                yield return null;
            }
        }
        else SetAlpha(0f);

        // 5) 정리 (여기서 턴 전환 호출)
        foreach (var sr in _spawned) if (sr) Destroy(sr.gameObject);
        _spawned.Clear();

        // TODO: TurnManager로 "상대 턴" 전환 호출
        // FindObjectOfType<TurnManager>()?.SetEnemyTurn();
    }

    private void SetAlpha(float a)
    {
        for (int i = 0; i < _spawned.Count; i++)
        {
            if (_spawned[i] == null) continue;
            var c = _spawned[i].color;
            c.a = a;
            _spawned[i].color = c;
        }
    }
}
