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
    [SerializeField] private Transform m_playerTransform;
    [SerializeField] private Transform m_enemyTransform;
    [SerializeField] private KeyChallengeManager m_keyChallengeManager;

    private static readonly int Attack = Animator.StringToHash("Attack");
    
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
        
        HitPlayer();
    }

    private IEnumerator OnHittingPlayer()
    {
        yield return StartCoroutine(PlayHitEffect(m_playerTransform));
        
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
