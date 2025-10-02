using System;
using SGGames.Script.Core;
using UnityEngine;

public enum KeyType
{
    Left,
    Right,
    Up,
    Down,
    
    COUNT_BASIC,
    
    //Diagonal keys
    LeftUp,
    LeftDown,
    RightUp,
    RightDown,
    
    COUNT_ADVANCED,
}

[Serializable]
public class KeyChallengeData
{
    public int NumberKeys;
    public KeyType[] KeyChallenge;
}

public class KeyChallengeManager : MonoBehaviour, IGameService, IBootStrap
{
    [SerializeField] private TimingBar m_timingBar;
    [SerializeField] private ChallengeBar m_challengeBar;
    [SerializeField] private KeyChallengeData m_currentChallenge;
    [SerializeField] private TurnbaseManager m_turnbaseManager;
    [SerializeField] private ButtonController m_startBattleButton;
    [SerializeField] private GameEvent m_gameEvent;
    
    
    private int m_numberKeyPressed;
    private const int k_startNumberKeys = 4;
    private bool m_isActivated = false;
    public void Install()
    {
        ServiceLocator.RegisterService<KeyChallengeManager>(this);
        ServiceLocator.GetService<InputManager>().OnMoveInputCallback = OnMoveInputCallback;
        m_currentChallenge = new KeyChallengeData();
        m_currentChallenge.NumberKeys = k_startNumberKeys;
        m_currentChallenge.KeyChallenge = new KeyType[k_startNumberKeys];
        
        m_gameEvent.AddListener(OnReceiveGameEvent);
        
        //Setup
        m_timingBar.StopAndReset();
        m_timingBar.Hide();
        m_challengeBar.HideChallenge();
        m_isActivated = false;

    }
    
    
    public void Uninstall()
    {
        m_gameEvent.RemoveListener(OnReceiveGameEvent);
    }
    
    public void NewBattle()
    {
        GenerateChallenge();
        m_challengeBar.ShowChallenge(m_currentChallenge);
        m_timingBar.StartAndActivate();
        m_isActivated = true;
    }
    
    private void AfterAttack()
    {
        m_timingBar.StopAndReset();
        m_timingBar.Hide();
        m_challengeBar.HideChallenge();
        m_isActivated = false;
    }

    private void OnMoveInputCallback(Vector2 input)
    {
        if (!m_isActivated) return;
        if (m_numberKeyPressed >= m_currentChallenge.NumberKeys)
        {
            return;
        }
        
        var keyType = InputToKey(input);
        if (keyType != KeyType.COUNT_BASIC)
        {
            if (CheckChallenge(keyType))
            {
                m_challengeBar.PressedKey(m_numberKeyPressed, keyType);
                m_numberKeyPressed++;
                if (m_numberKeyPressed >= m_currentChallenge.NumberKeys)
                {
                    m_numberKeyPressed = m_currentChallenge.NumberKeys;
                }
            }
        }
    }

    public void OnPressAttack(DamageZone damageZone)
    {
        if (!m_isActivated) return;
        if (damageZone != DamageZone.None)
        {
            if (m_numberKeyPressed >= m_currentChallenge.NumberKeys)
            {
                AfterAttack();
                m_turnbaseManager.HitEnemy();
            }
            else
            {
                //TODO:
                //Miss atk
                Debug.Log("MISS ATK 1!");
                ResetChallenge();
                m_turnbaseManager.HitPlayer();
            }
        }
        else
        {
            m_timingBar.StopAndReset();
            //TODO:
            //Miss atk
            Debug.Log("MISS ATK 2!");
            //ResetChallenge();
            m_turnbaseManager.HitPlayer();
        }
    }

    public void ResetChallenge()
    {
        m_timingBar.ResetTimingBar();
        GenerateChallenge();
    }
    
    
    [ContextMenu("Test")]
    private void GenerateChallenge()
    {
        Debug.Log("GenerateChallenge");
        for(int i=0; i<m_currentChallenge.NumberKeys; i++)
        {
            m_currentChallenge.KeyChallenge[i] = (KeyType)UnityEngine.Random.Range(0, (int)KeyType.COUNT_BASIC);
        }

        m_numberKeyPressed = 0;
        m_challengeBar.ShowChallenge(m_currentChallenge);
    }

    private KeyType InputToKey(Vector2 input)
    {
        //2 keys combination
        if (input.x != 0 && input.y != 0)
        {
            if (input.x > 0 && input.y > 0)
            {
                return KeyType.RightUp;
            }

            if (input.x < 0 && input.y > 0)
            {
                return KeyType.LeftUp;
            }

            if (input.x > 0 && input.y < 0)
            {
                return KeyType.RightDown;
            }

            if (input.x < 0 && input.y < 0)
            {
                return KeyType.LeftDown;
            }
        }
        else
        {
            if (input.x > 0)
            {
                return KeyType.Right;
            }

            if (input.x < 0)
            {
                return KeyType.Left;
            }

            if (input.y > 0)
            {
                return KeyType.Up;
            }

            if (input.y < 0)
            {
                return KeyType.Down;
            }
        }
        return KeyType.COUNT_BASIC;
    }

    private bool CheckChallenge(KeyType keyType)
    {
        return m_currentChallenge.KeyChallenge[m_numberKeyPressed] == keyType;
    }
    
    
    private void OnReceiveGameEvent(GameEventType gameEventType)
    {
        if (gameEventType == GameEventType.WaveFinished)
        {
            AfterAttack();
        }
    }
}
