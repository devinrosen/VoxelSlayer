public interface IInputHandler
{
    void HandleInput(bool jump, bool attack, bool leftPunch, bool rightPunch, float x, float y, float z, float w);
}
