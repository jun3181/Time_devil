using UnityEngine;

public interface ICardPattern
{
    // Resources 경로의 카드 이미지 (예: "my_asset/Card1")
    string SpritePath { get; }
    // 패턴: "1/0"로 4글자씩, 위→아래 순서로 4줄 (예: 1111,0100,1010,0000)
    string[] PatternRows { get; }
}
