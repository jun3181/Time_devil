// ICardPattern.cs
public interface ICardPattern
{
    // 카드 이미지 경로 (Resources 기준 경로, 예: "my_asset/Card1")
    string CardImagePath { get; }

    // 16칸 공격 패턴 (0/1 문자열, 길이 16)
    string Pattern16 { get; }
}
