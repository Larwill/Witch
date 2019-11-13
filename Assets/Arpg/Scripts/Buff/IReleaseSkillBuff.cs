namespace Arpg.Scripts.Buff
{
    public interface IReleaseSkillBuff
    {
        void DoBuff(System.Action callback);
        void UnDoBuff(System.Action callback);
    }
}