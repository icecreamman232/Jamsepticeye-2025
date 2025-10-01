using System;
using UnityEngine;

public class ChallengeBar : MonoBehaviour
{
   [SerializeField] private CanvasGroup m_canvasGroup;
   [SerializeField] private Color m_readyColor;
   [SerializeField] private Color m_reversedColor;
   [SerializeField] private Color m_completedColor;
   [SerializeField] private Sprite[] m_keySprites;
   [SerializeField] private KeyChallengeSlot[] m_keySlots;

   public void HideChallenge()
   {
      m_canvasGroup.alpha = 0;
   }
   
   public void ShowChallenge(KeyChallengeData challengeData)
   {
      m_canvasGroup.alpha = 1;
      for (int i = 0; i < m_keySlots.Length; i++)
      {
         if (i < challengeData.NumberKeys)
         {
            m_keySlots[i].gameObject.SetActive(true);
            m_keySlots[i].SetKey(GetSprite(challengeData.KeyChallenge[i]), m_readyColor);
         }
         else
         {
            m_keySlots[i].gameObject.SetActive(false);
         }
      }
   }

   public void PressedKey(int slotIndex, KeyType keyType)
   {
      m_keySlots[slotIndex].UpdateColor(m_completedColor);
   }
   
   private Sprite GetSprite(KeyType keyType)
   {
      return m_keySprites[(int)keyType];
   }
}
