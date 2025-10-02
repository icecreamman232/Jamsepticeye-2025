using System.Collections;
using SGGames.Script.Core;
using UnityEngine;

public class LevelManager : MonoBehaviour, IGameService, IBootStrap
{
    [SerializeField] private GameEvent m_gameEvent;
    [SerializeField] private WaveManager m_waveManager;
    [SerializeField] private KeyChallengeManager m_keyChallengeManager;
    [SerializeField] private ButtonController m_startBattleButton;
    public void Install()
    {
        ServiceLocator.RegisterService<LevelManager>(this);
        m_startBattleButton.gameObject.SetActive(true);
        m_startBattleButton.OnButtonClick = StartBattle;
        m_gameEvent.AddListener(OnReceiveGameEvent);
    }

    public void Uninstall()
    {
        m_gameEvent.RemoveListener(OnReceiveGameEvent);
    }
    private void StartBattle()
    {
        StartCoroutine(LoadFirstLevel());
    }

    private IEnumerator LoadFirstLevel()
    {
        m_startBattleButton.gameObject.SetActive(false);
        m_waveManager.StartWave();
        yield return new WaitForSeconds(0.1f);
        m_keyChallengeManager.NewBattle();
    }
    
    private void OnReceiveGameEvent(GameEventType gameEventType)
    {
        if (gameEventType == GameEventType.WaveFinished)
        {
            m_startBattleButton.gameObject.SetActive(true);
        }
    }
}
