using DG.Tweening;
using SGGames.Script.Core;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image m_healthBar;

    private void Start()
    {
        m_healthBar.fillAmount = 1;
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        var fillTarget = MathHelpers.Remap(currentHealth, 0, maxHealth, 0, 1);
        m_healthBar.DOFillAmount(fillTarget, 0.2f)
            .SetEase(Ease.InOutExpo);
    }
}
