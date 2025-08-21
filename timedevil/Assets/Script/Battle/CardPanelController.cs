using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardPanelController : MonoBehaviour
{
    [Header("UI")]
    public GameObject cardGroup;
    public Button card5Button;
    public Image card5Image;

    [Header("Resources")]
    public string card5SpritePath = "my_asset/5";       // 5.png
    public string attackSpritePath = "my_asset/attack"; // 빨간 X

    [Header("Board")]
    public Transform enemyBoardOrigin; // ❗ 한번만 배치(적 보드 좌하단)
    public float tileSize = 1f;
    public int sortingOrder = 20;

    [Header("Timing")]
    public float fadeInTime = 3.0f;
    public float holdTime = 0.1f;
    public float fadeOutTime = 0.6f;

    // ▶ 카드별 패턴만 여기서 교체/추가하면 됨
    // "5" 카드
    private static readonly string[] PATTERN_EDGE = {
        "1010",
        "0100",
        "1010",
        "0000"
    };
    // 예시: "엣지라인" 카드 (위/아래 전부 1)
    private static readonly string[] PATTERN_5 = {
        "1111",
        "0000",
        "0000",
        "1111"
    };

    bool isOpen = false;
    Sprite attackSprite;
    readonly List<GameObject> spawned = new();

    public void OnCardButton()
    {
        if (TurnManager.Instance.currentTurn != TurnState.PlayerTurn) return;

        var sp = Resources.Load<Sprite>(card5SpritePath);
        if (!sp) { Debug.LogWarning($"not found: {card5SpritePath}"); return; }

        card5Image.sprite = sp;
        card5Button.onClick.RemoveAllListeners();
        // ❗여기서 어떤 패턴을 쓸지 선택
        card5Button.onClick.AddListener(() => OnCardChosen(PATTERN_5));
        // 다른 카드를 추가하면 위와 같이 OnClick에 PATTERN_EDGE 등 연결만 바꾸면 됨

        cardGroup.SetActive(true);
        isOpen = true;
    }

    // 선택된 카드의 패턴을 받아서 처리
    void OnCardChosen(string[] patternRows)
    {
        if (!isOpen) return;
        Debug.Log("[Card] 카드 선택 → 패턴 표시 시작");

        cardGroup.SetActive(false);
        isOpen = false;

        if (!attackSprite) attackSprite = Resources.Load<Sprite>(attackSpritePath);
        if (!attackSprite) { Debug.LogWarning($"not found: {attackSpritePath}"); TurnManager.Instance.EndPlayerTurn(); return; }

        StartCoroutine(PlayAttackFXThenEndTurn(patternRows));
    }

    IEnumerator PlayAttackFXThenEndTurn(string[] patternRows)
    {
        SpawnMarkers(patternRows);
        yield return FadeMarkers(0f, 1f, fadeInTime); // 점점 밝게(약 3초)
        yield return new WaitForSeconds(holdTime);
        yield return FadeMarkers(1f, 0f, fadeOutTime); // 점점 사라짐
        ClearMarkers();

        TurnManager.Instance.EndPlayerTurn();
    }

    void SpawnMarkers(string[] patternRows)
    {
        ClearMarkers();
        if (!enemyBoardOrigin) { Debug.LogWarning("enemyBoardOrigin 미지정"); return; }

        for (int y = 0; y < 4; y++)
        {
            string row = (y < patternRows.Length) ? patternRows[y] : "0000";
            for (int x = 0; x < 4; x++)
            {
                bool hit = (x < row.Length) && row[x] == '1';
                if (!hit) continue;

                var go = new GameObject($"attack ({x},{y})");
                go.transform.position = enemyBoardOrigin.position + new Vector3(x * tileSize, y * tileSize, 0f);

                var sr = go.AddComponent<SpriteRenderer>();
                sr.sprite = attackSprite;
                sr.sortingOrder = sortingOrder;
                sr.color = new Color(1f, 1f, 1f, 0f); // 처음엔 투명

                spawned.Add(go);
            }
        }
    }

    IEnumerator FadeMarkers(float from, float to, float duration)
    {
        if (duration <= 0f) duration = 0.001f;
        float t = 0f;
        var srs = new List<SpriteRenderer>(spawned.Count);
        foreach (var go in spawned) if (go) srs.Add(go.GetComponent<SpriteRenderer>());

        while (t < duration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(from, to, t / duration);
            for (int i = 0; i < srs.Count; i++)
            {
                if (!srs[i]) continue;
                var c = srs[i].color;
                c.a = a;
                srs[i].color = c;
            }
            yield return null;
        }
        // 보정
        for (int i = 0; i < srs.Count; i++)
        {
            if (!srs[i]) continue;
            var c = srs[i].color;
            c.a = to;
            srs[i].color = c;
        }
    }

    void ClearMarkers()
    {
        for (int i = 0; i < spawned.Count; i++)
            if (spawned[i]) Destroy(spawned[i]);
        spawned.Clear();
    }
}
