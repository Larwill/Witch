namespace Arpg.Scripts.Buff
{
    public interface IAttackBuff
    {
        void DoBuff(System.Action callback);
        void UnDoBuff(System.Action callback);
    }
}