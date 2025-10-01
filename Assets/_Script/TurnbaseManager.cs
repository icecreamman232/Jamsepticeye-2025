using System.Collections;
using UnityEngine;

public enum Turn
{
    Player,
    Enemy,
}

public class TurnbaseManager : MonoBehaviour
{
    [SerializeField] private Turn m_currentTurn;
    [SerializeField] private Animator m_hitVfxAnimator;
    [SerializeField] private KeyChallengeManager m_keyChallengeManager;
    [SerializeField] private WaveManager m_waveManager;
    
    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private Transform m_enemyTransform;
    [SerializeField] private Health m_playerHealth;
    [SerializeField] private Health m_enemyHealth;
    [SerializeField] private Weapon m_playerWeapon;
    [SerializeField] private Weapon m_enemyWeapon;

    private static readonly int Attack = Animator.StringToHash("Attack");

    public void AssignEnemy(GameObject enemy)
    {
        m_enemyTransform = enemy.transform;
        m_enemyHealth = enemy.GetComponent<Health>();
        m_enemyWeapon = enemy.GetComponent<Weapon>();
    }
    
    public void HitEnemy()
    {
        StartCoroutine(OnHittingEnemy());
    }

    public void HitPlayer()
    {
        StartCoroutine(OnHittingPlayer());
    }

    private IEnumerator OnHittingEnemy()
    {
        yield return StartCoroutine(PlayHitEffect(m_enemyTransform));
        m_enemyHealth.TakeDamage(m_playerWeapon.GetFinalDamage());
        yield return new WaitForSeconds(0.2f);

        if (m_enemyHealth.IsDead)
        {
            m_waveManager.NextEnemyInWave();
            yield return new WaitForSeconds(0.2f);
            m_keyChallengeManager.ReadyBattle();
            yield break;
        }
        
        HitPlayer();
    }

    private IEnumerator OnHittingPlayer()
    {
        yield return StartCoroutine(PlayHitEffect(m_playerTransform));
        m_playerHealth.TakeDamage(m_enemyWeapon.GetFinalDamage());
        yield return new WaitForSeconds(0.2f);

        if (m_playerHealth.IsDead)
        {
            yield break;
        }
        
        m_keyChallengeManager.ReadyBattle();
    }

    private IEnumerator PlayHitEffect(Transform target)
    {
        var randomPos = Random.insideUnitCircle.normalized * 1.5f + (Vector2)target.position;
        m_hitVfxAnimator.gameObject.SetActive(true);
        m_hitVfxAnimator.transform.position = randomPos;
        m_hitVfxAnimator.SetTrigger(Attack);
        yield return new WaitForSeconds(0.5f);
    }
}
