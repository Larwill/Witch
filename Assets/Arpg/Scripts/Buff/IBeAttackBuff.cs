namespace Arpg.Scripts.Buff
{
    public interface IBeAttackBuff
    {
        void DoBuff(System.Action callback);
        void UnDoBuff(System.Action callback);
    }
}