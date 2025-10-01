using SGGames.Script.Core;
using UnityEngine;
using UnityEngine.UI;

public enum DamageZone
{
    None,
    Low,
    Average,
    High,
}

public class TimingBar : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private KeyChallengeManager m_keyChallengeManager;
    [SerializeField] [Range(0.3f, 1.2f)] private float m_speed;
    [SerializeField] private RectTransform m_damageBar;
    [SerializeField] private RectTransform m_lowDamageImage;
    [SerializeField] private RectTransform m_avgDamageImage;
    [SerializeField] private RectTransform m_highDamageImage;
    [SerializeField] private Slider m_slider;
    [SerializeField] private Vector2 m_lowDamageZone;
    [SerializeField] private Vector2 m_averageDamageZone;
    [SerializeField] private Vector2 m_highDamageZone;

    private bool m_isRunning = true;
    
    private void Start()
    {
        ServiceLocator.GetService<InputManager>().OnAttackInputCallback = OnAttackInputCallback;
        SetupDamageZone();
    }

    public void ResetTimingBar()
    {
        m_slider.value = 0;
    }

    public void StopAndReset()
    {
        m_isRunning = false;
        m_canvasGroup.alpha = 0;
        ResetTimingBar();
    }

    public void StartAndActivate()
    {
        m_isRunning = true;
        m_canvasGroup.alpha = 1;
    }

    private void SetupDamageZone()
    {
        SetZoneSize(m_lowDamageImage, m_lowDamageZone);
        SetZoneSize(m_avgDamageImage, m_averageDamageZone);
        SetZoneSize(m_highDamageImage, m_highDamageZone);
    }
    
    private void SetZoneSize(RectTransform zoneRT, Vector2 zonePercent)
    {
        var barSize = m_damageBar.sizeDelta.x;
        var minZoneWidth = barSize * zonePercent.x;
        var maxZoneWidth = barSize * zonePercent.y;
        var zoneWidth = maxZoneWidth - minZoneWidth;

        var size = zoneRT.sizeDelta;
        size.x = zoneWidth;
        zoneRT.sizeDelta = size;
        
        
        zoneRT.anchoredPosition = new Vector2(minZoneWidth, 0);
    }

    private void OnAttackInputCallback()
    {
        //Debug.Log("Slider value: " + m_slider.value + "");
        if (IsInZone(m_highDamageZone))
        {
            m_keyChallengeManager.OnPressAttack(DamageZone.High);
        }
        else if (IsInZone(m_averageDamageZone))
        {
            m_keyChallengeManager.OnPressAttack(DamageZone.Average);
        }
        else if (IsInZone(m_lowDamageZone))
        {
            m_keyChallengeManager.OnPressAttack(DamageZone.Low);
        }
        else
        {
            m_keyChallengeManager.OnPressAttack(DamageZone.None);
        }
    }

    private bool IsInZone(Vector2 zonePercent)
    {
        return m_slider.value >= zonePercent.x && m_slider.value <= zonePercent.y; 
    }

    private void Update()
    {
        if(!m_isRunning) return;
        m_slider.value += (m_speed * Time.deltaTime);
        if (m_slider.value >= 1)
        {
            m_slider.value = 0;
        }
    }
}
