using System;
using SGGames.Script.Core;
using UnityEngine;

public class WonBattleScreen : MonoBehaviour
{
    [SerializeField] private CanvasGroup m_canvasGroup;
    [SerializeField] private ButtonController m_nextButton;
    [SerializeField] private GameEvent m_gameEvent;

    private void Start()
    {
        m_canvasGroup.Deactivate();
        m_gameEvent.AddListener(OnGameEventChanged);
        m_nextButton.OnButtonClick = OnNextRoom;
    }

    private void OnDestroy()
    {
        m_gameEvent.RemoveListener(OnGameEventChanged);
    }

    private void OnGameEventChanged(GameEventType gameEventType)
    {
        if (gameEventType == GameEventType.WonBattle)
        {
            m_canvasGroup.Activate();
        }
    }

    private void OnNextRoom()
    {
        m_canvasGroup.Deactivate();
        m_gameEvent.Raise(GameEventType.NewBattle);
    }
}
