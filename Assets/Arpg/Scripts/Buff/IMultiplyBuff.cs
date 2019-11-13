namespace Arpg.Scripts.Buff
{
    public interface IMultiplyBuff
    {
        void DoBuff(System.Action callback);
        void UnDoBuff(System.Action callback);
    }
}