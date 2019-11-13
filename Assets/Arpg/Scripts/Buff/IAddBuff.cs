namespace Arpg.Scripts.Buff
{
    public interface IAddBuff
    {
        void DoBuff(System.Action callback);
        void UnDoBuff(System.Action callback);
    }
}