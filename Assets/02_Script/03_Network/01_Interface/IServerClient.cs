/// 서버 통신 인터페이스. 실제 서버 요청 로직은 이 인터페이스를 통해 추상화됨
public interface IServerClient
{
    /// <summary>
    /// 새 게임 생성 요청을 보냄
    /// </summary>
    void SendNewGameRequest(string nickname);

    /// <summary>
    /// 저장된 게임 데이터를 불러오는 요청을 보냄
    /// </summary>
    void SendLoadGameRequest(string nickname);
}