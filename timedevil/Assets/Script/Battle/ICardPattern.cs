public interface ICardPattern
{
    // Resources ��� (��: "my_asset/Card1")
    string CardImagePath { get; }

    // 16���� ���ڿ�(0/1) ����. ��: "1111000011110000"
    string Pattern16 { get; }
}
