using UnityEngine;

public interface ICardPattern
{
    // Resources ����� ī�� �̹��� (��: "my_asset/Card1")
    string SpritePath { get; }
    // ����: "1/0"�� 4���ھ�, ����Ʒ� ������ 4�� (��: 1111,0100,1010,0000)
    string[] PatternRows { get; }
}
