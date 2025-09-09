public interface ICardPattern
{
    // Resources 경로 (예: "my_asset/Card1")
    string CardImagePath { get; }

    // 16글자 문자열(0/1) 패턴. 예: "1111000011110000"
    string Pattern16 { get; }
}
