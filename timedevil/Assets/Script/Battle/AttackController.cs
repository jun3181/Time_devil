using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackController : MonoBehaviour
{
    [Header("Grid / Coordinates")]
    public float cellSize = 0.64f;     // �� ĭ ũ��(Ÿ�� �� �Ÿ�)
    public bool originTopLeft = false; // true�� �� ������Ʈ�� 4x4�� �»������ ����, false�� ���ϴ����� ����

    [Header("Marker Sprite (Resources)")]
    public string markerSpritePath = "my_asset/attack"; // ���� X ���

    [Header("Render")]
    public int sortingOrder = 40;

    [Header("Timing (seconds)")]
    public float fadeIn = 0.6f;
    public float hold = 0.1f;
    public float fadeOut = 0.6f;

    readonly List<SpriteRenderer> _spawned = new();

    /// <summary>
    /// patternRows ��: {"1111","0100","1010","0000"} (����Ʒ�)
    /// </summary>
    public void ShowPattern(string[] patternRows)
    {
        StartCoroutine(CoShowPattern(patternRows));
    }

    IEnumerator CoShowPattern(string[] rows)
    {
        // ���� ��Ŀ ����
        Cleanup();

        // ��������Ʈ �ε�
        var marker = Resources.Load<Sprite>(markerSpritePath);
        if (!marker)
        {
            Debug.LogWarning($"[AttackController] marker sprite not found: {markerSpritePath}");
            yield break;
        }

        // ��ǥ ���� ���
        // ������(�� ������Ʈ ��ġ)�� ���ϴ�(�⺻) �Ǵ� �»��(originTopLeft=true)�� ���
        // ��(row): 0=��, 3=�Ʒ� / ��(col): 0=��, 3=����
        for (int r = 0; r < rows.Length; r++)
        {
            string line = rows[r];
            for (int c = 0; c < line.Length; c++)
            {
                if (line[c] != '1') continue;

                // ȭ����� y �ε��� (���� 0)
                int yIndexFromTop = r;

                // ���ϴ� �������� y ������ ����Ϸ��� (3 - r)
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
                sr.color = new Color(1, 1, 1, 0); // ���� ����
                _spawned.Add(sr);
            }
        }

        // ���̵� ��
        yield return FadeAll(0f, 1f, fadeIn);

        // ����
        if (hold > 0f) yield return new WaitForSeconds(hold);

        // ���̵� �ƿ�
        yield return FadeAll(1f, 0f, fadeOut);

        // ����
        Cleanup();

        // �� �ѱ��
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
