using UnityEngine;

public interface IBotState
{
    void Enter(BotController bot);
    void Update();
    void Exit();
}
