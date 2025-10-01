using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private WaveData m_currentWave;
    [SerializeField] private WaveData[] m_easyWaveList;
    [SerializeField] private TurnbaseManager m_turnbaseManager;
    private readonly Vector2 m_spawnPosition = new Vector2(3, 0);
    private int m_currentEnemyIndex;
    private GameObject m_currentEnemy;
    
    public void StartWave()
    {
        m_currentWave = GetRandomWave(m_easyWaveList);
        m_currentEnemyIndex = 0;
        m_currentEnemy = CreateNextEnemy();
        AssignEnemy(m_currentEnemy);
    }
    
    public void NextEnemyInWave()
    {
        Destroy(m_currentEnemy);
        m_currentEnemy = CreateNextEnemy();
        AssignEnemy(m_currentEnemy);
    }


    private void AssignEnemy(GameObject newEnemy)
    {
        m_turnbaseManager.AssignEnemy(newEnemy);
    }
    
    public GameObject CreateNextEnemy()
    {
        if (m_currentEnemyIndex >= m_currentWave.Enemies.Length)
        {
            //Pick new wave
            m_currentWave = GetRandomWave(m_easyWaveList);
            m_currentEnemyIndex = 0;
        }
        
        var newEnemy  = Instantiate(m_currentWave.Enemies[m_currentEnemyIndex], m_spawnPosition, Quaternion.identity);
        m_currentEnemyIndex++;
        return newEnemy;
    }

    private WaveData GetRandomWave(WaveData[] m_list)
    {
        return m_list[Random.Range(0, m_list.Length)];
    }
}
