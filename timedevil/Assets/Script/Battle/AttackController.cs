using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적/플레이어 패널 위에 공격 마커를 시간차로 표시하는 컨트롤러
/// - 패널은 4x4 = 16셀 고정
/// - pattern16: '0'/'1' 16글자 (왼→오, 위→아래)
/// - timings : 인덱스별 지연 시간(초). null 이면 전부 0초로 처리
/// </summary>
public class AttackController : MonoBehaviour
{
    [System.Serializable]
    public struct CellPos { public float x; public float y; }

    public enum Panel { Enemy, Player } // Enemy=오른쪽, Player=왼쪽

    [Header("Enemy Panel 16셀 월드 좌표 (인덱스 0~15)")]
    public List<CellPos> enemyCellPositions = new List<CellPos>(16);

    [Header("Player Panel 16셀 월드 좌표 (인덱스 0~15)")]
    public List<CellPos> playerCellPositions = new List<CellPos>(16);

    [Header("Marker (Resources 경로)")]
    public string markerSpritePath = "my_asset/attack";
    public int sortingOrder = 40;

    [Header("Fade (sec)")]
    public float fadeIn = 0.6f;
    public float hold   = 0.1f;
    public float fadeOut= 0.6f;

    // (옵션) 외부에서 구독 가능
    public System.Action OnPatternFinished;

    // 실행 중/마지막 총 소요시간(초) 노출
    public float LastTotalDuration { get; private set; }

    readonly List<SpriteRenderer> _spawned = new();

    /// <summary>기본: Enemy 패널, 지연시간 없음</summary>
    public void ShowPattern(string pattern16)
        => ShowPattern(pattern16, null, Panel.Enemy);

    /// <summary>지연시간과 패널 지정</summary>
    public void ShowPattern(string pattern16, float[] timings, Panel panel = Panel.Enemy)
    {
        if (string.IsNullOrEmpty(pattern16) || pattern16.Length != 16)
        {
            Debug.LogError("[AttackController] pattern16은 정확히 16글자여야 합니다.");
            return;
        }
        StartCoroutine(Co_Show(pattern16, timings, panel));
    }

    IEnumerator Co_Show(string pattern16, float[] timings, Panel panel)
    {
        // 이전 것 정리
        ClearAll();

        var sprite = Resources.Load<Sprite>(markerSpritePath);
        if (sprite == null)
        {
            Debug.LogError($"[AttackController] 마커 스프라이트를 찾을 수 없음: {markerSpritePath}");
            yield break;
        }

        // 대상 패널 좌표 참조
        var cells = (panel == Panel.Enemy) ? enemyCellPositions : playerCellPositions;
        if (cells == null || cells.Count < 16)
        {
            Debug.LogError("[AttackController] 16개의 셀 좌표가 필요합니다.");
            yield break;
        }

        // 각 셀에 대해 시간차 스폰
        float maxDelay = 0f;
        for (int i = 0; i < 16; i++)
        {
            if (pattern16[i] != '1') continue;

            float delay = (timings != null && i < timings.Length) ? Mathf.Max(0f, timings[i]) : 0f;
            if (delay > maxDelay) maxDelay = delay;

            // 셀 인덱스 보존하여 코루틴 호출
            StartCoroutine(SpawnOne(i, sprite, cells, delay));
        }

        // 전체 예상 소요시간 = 가장 늦은 시작 + (fadeIn + hold + fadeOut)
        LastTotalDuration = maxDelay + fadeIn + hold + fadeOut;

        // 모두 끝날 때까지 대기
        yield return new WaitForSeconds(LastTotalDuration + 0.01f);

        // 혹시 남아있는 것 정리
        ClearAll();

        OnPatternFinished?.Invoke();
    }

    IEnumerator SpawnOne(int index, Sprite sprite, List<CellPos> cells, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (index >= cells.Count) yield break;

        var pos = new Vector3(cells[index].x, cells[index].y, -2f); // Z 고정
        var go = new GameObject($"attack ({index})");
        go.transform.position = pos;

        var sr = go.AddComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingLayerName = "Default";
        sr.sortingOrder = sortingOrder;
        sr.color = new Color(1, 1, 1, 0f);

        _spawned.Add(sr);

        // 페이드 인
        if (fadeIn > 0f)
        {
            float t = 0f;
            while (t < fadeIn)
            {
                t += Time.deltaTime;
                float a = Mathf.Clamp01(t / fadeIn);
                SetAlpha(sr, a);
                yield return null;
            }
        }
        else SetAlpha(sr, 1f);

        // 유지
        if (hold > 0f) yield return new WaitForSeconds(hold);

        // 페이드 아웃
        if (fadeOut > 0f)
        {
            float t = 0f;
            while (t < fadeOut)
            {
                t += Time.deltaTime;
                float a = 1f - Mathf.Clamp01(t / fadeOut);
                SetAlpha(sr, a);
                yield return null;
            }
        }
        else SetAlpha(sr, 0f);

        // 정리
        if (sr) _spawned.Remove(sr);
        if (sr) Destroy(sr.gameObject);
    }

    void SetAlpha(SpriteRenderer sr, float a)
    {
        if (!sr) return;
        var c = sr.color;
        c.a = a;
        sr.color = c;
    }

    // AttackController.cs 내부에 메서드 추가
    public void ClearImmediate()
    {
    StopAllCoroutines();
    for (int i = 0; i < _spawned.Count; i++)
        if (_spawned[i]) Destroy(_spawned[i].gameObject);
    _spawned.Clear();
    }


    void ClearAll()
    {
        for (int i = 0; i < _spawned.Count; i++)
        {
            if (_spawned[i]) Destroy(_spawned[i].gameObject);
        }
        _spawned.Clear();
    }
}
