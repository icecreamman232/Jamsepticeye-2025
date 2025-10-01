using UnityEngine;
using UnityEngine.UI;

public class KeyChallengeSlot : MonoBehaviour
{
    [SerializeField] private Image m_keyImage;

    public void SetKey(Sprite sprite, Color color)
    {
        m_keyImage.sprite = sprite;
        m_keyImage.color = color;
    }

    public void UpdateColor(Color color)
    {
        m_keyImage.color = color;   
    }
}
