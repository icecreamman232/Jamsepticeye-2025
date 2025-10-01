using System.Collections;
using SGGames.Script.Core;
using UnityEngine;

public class LevelManager : MonoBehaviour, IGameService, IBootStrap
{
    [SerializeField] private WaveManager m_waveManager;
    [SerializeField] private GameObject m_startBattleButton;
    public void Install()
    {
        ServiceLocator.RegisterService<LevelManager>(this);
        StartCoroutine(LoadFirstLevel());
    }

    public void Uninstall()
    {
        
    }

    private IEnumerator LoadFirstLevel()
    {
        m_waveManager.StartWave();
        yield return new WaitForSeconds(0.1f);
        m_startBattleButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.1f);
    }
}
