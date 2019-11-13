namespace Arpg.Scripts.Agent
{
    public interface IAction
    {
        void Start();
        void Update();
        void Quit();
    }
}