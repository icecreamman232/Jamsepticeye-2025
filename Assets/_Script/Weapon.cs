using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float m_minDamage;
    [SerializeField] private float m_maxDamage;

    public virtual float GetFinalDamage()
    {
        return GetRawDamage();
    }
    
    private float GetRawDamage()
    {
        return Random.Range(m_minDamage, m_maxDamage);
    }
}
