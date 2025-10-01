using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private HealthBar m_healthBar;
    [SerializeField] private float m_maxHealth;
    [SerializeField] private float m_currentHealth;

    public Action OnDeath;
    
    public bool IsDead => m_currentHealth <= 0;
    
    private void Start()
    {
        m_currentHealth = m_maxHealth;
    }

    public void TakeDamage(float damage)
    {
        m_currentHealth -= damage;
        if (m_currentHealth < 0)
        {
            m_currentHealth = 0;
        }
        m_healthBar.UpdateHealthBar(m_currentHealth, m_maxHealth);
        if (m_currentHealth <= 0)
        {
            Kill();
        }
    }

    protected virtual void Kill()
    {
        OnDeath?.Invoke();
        this.gameObject.SetActive(false);
    }
}
