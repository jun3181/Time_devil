using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [Header("Grid / Coordinates")]
    public float cellSize = 0.64f;     // 한 칸 크기(타일 간 거리)
    public bool originTopLeft = false; // true면 이 오브젝트를 4x4의 좌상단으로 간주, false면 좌하단으로 간주

    [Header("Marker Sprite (Resources)")]
    public string markerSpritePath = "my_asset/attack"; // 빨간 X 경로

    [Header("Render")]
    public int sortingOrder = 40;

    [Header("Timing (seconds)")]
    public float fadeIn = 0.6f;
    public float hold = 0.1f;
    public float fadeOut = 0.6f;

    readonly List<SpriteRenderer> _spawned = new();

    /// <summary>
    /// patternRows 예: {"1111","0100","1010","0000"} (위→아래)
    /// </summary>
    public void ShowPattern(string[] patternRows)
    {
        StartCoroutine(CoShowPattern(patternRows));
    }

    IEnumerator CoShowPattern(string[] rows)
    {
        // 기존 마커 정리
        Cleanup();

        // 스프라이트 로드
        var marker = Resources.Load<Sprite>(markerSpritePath);
        if (!marker)
        {
            Debug.LogWarning($"[AttackController] marker sprite not found: {markerSpritePath}");
            yield break;
        }

        // 좌표 기준 계산
        // 기준점(이 오브젝트 위치)을 좌하단(기본) 또는 좌상단(originTopLeft=true)로 사용
        // 행(row): 0=위, 3=아래 / 열(col): 0=왼, 3=오른
        for (int r = 0; r < rows.Length; r++)
        {
            string line = rows[r];
            for (int c = 0; c < line.Length; c++)
            {
                if (line[c] != '1') continue;

                // 화면상의 y 인덱스 (위가 0)
                int yIndexFromTop = r;

                // 좌하단 기준으로 y 오프셋 계산하려면 (3 - r)
                int yIndexBottom = (rows.Length - 1) - r;

                float x = transform.position.x + c * cellSize + cellSize * 0.5f;
                float y = originTopLeft
                    ? transform.position.y - yIndexFromTop * cellSize - cellSize * 0.5f
                    : transform.position.y + yIndexBottom * cellSize + cellSize * 0.5f;

                var go = new GameObject($"attack ({r},{c})");
                go.transform.position = new Vector3(x, y, 0f);

                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = marker;
                sr.sortingOrder = sortingOrder;
                sr.color = new Color(1, 1, 1, 0); // 투명 시작
                _spawned.Add(sr);
            }
        }

        // 페이드 인
        yield return FadeAll(0f, 1f, fadeIn);

        // 유지
        if (hold > 0f) yield return new WaitForSeconds(hold);

        // 페이드 아웃
        yield return FadeAll(1f, 0f, fadeOut);

        // 정리
        Cleanup();

        // 턴 넘기기
        if (TurnManager.Instance != null)
            TurnManager.Instance.EndPlayerTurn();
    }

    IEnumerator FadeAll(float a0, float a1, float dur)
    {
        if (dur <= 0f) { SetAlphaAll(a1); yield break; }

        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / dur);
            float a = Mathf.Lerp(a0, a1, k);
            SetAlphaAll(a);
            yield return null;
        }
        SetAlphaAll(a1);
    }

    void SetAlphaAll(float a)
    {
        foreach (var sr in _spawned)
        {
            if (!sr) continue;
            var c = sr.color;
            c.a = a;
            sr.color = c;
        }
    }

    void Cleanup()
    {
        foreach (var sr in _spawned)
            if (sr) Destroy(sr.gameObject);
        _spawned.Clear();
    }
}
