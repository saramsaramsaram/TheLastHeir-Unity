using UnityEngine;

/// 게임 서비스 구현체. 서버 클라이언트를 통해 실제 요청을 전달함
public class GameService : IGameService
{
    private readonly IServerClient serverClient;
    
    /// 생성자에서 서버 클라이언트를 주입받음
    public GameService(IServerClient client)
    {
        serverClient = client;
    }
    
    /// 새 게임 요청을 서버에 전달
    public void StartNewGame(string nickname)
    {
        Debug.Log($"[GameService] Starting new game for {nickname}");
        serverClient.SendNewGameRequest(nickname);
    }
    
    /// 기존 게임 불러오기 요청을 서버에 전달
    public void LoadGame(string nickname)
    {
        Debug.Log($"[GameService] Loading game for {nickname}");
        serverClient.SendLoadGameRequest(nickname);
    }
}