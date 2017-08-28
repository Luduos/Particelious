using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinInfoController : MonoBehaviour {

    [SerializeField]
    private GlobalInfo m_GlobalInfo = null;

    [SerializeField]
    private Text m_CoinText = null;
    const string m_CoinDisplay = "x{0}";

    [SerializeField]
    private Slider m_CoinProgressSlider = null;

    // Use this for initialization

    void Start () {
        if(null != m_GlobalInfo)
        {
            m_GlobalInfo.OnGlobalCoinCountChanged += OnUpdateCoinText;
            m_GlobalInfo.OnCurrentSessionCoinCountChanged += OnUpdateCoinProgressSlider;
            m_GlobalInfo.UpdateCoinInfo();
        }

        GameState.OnGameSessionEnter += ShowProgressSlider;
        GameState.OnGameSessionExit += HideProgessSlider;
        HideProgessSlider();
    }

    private void OnDestroy()
    {
        GameState.OnGameSessionEnter -= ShowProgressSlider;
        GameState.OnGameSessionExit -= HideProgessSlider;
    }

    private void ShowProgressSlider()
    {
        if (m_CoinProgressSlider)
        {
            m_CoinProgressSlider.gameObject.SetActive(true);
        }
    }

    private void HideProgessSlider()
    {
        if (m_CoinProgressSlider)
        {
            m_CoinProgressSlider.gameObject.SetActive(false);
        }
    }

    void OnUpdateCoinText(int UpdatedCoinCount)
    {
        if (m_CoinText)
        {
            m_CoinText.text = string.Format(m_CoinDisplay, UpdatedCoinCount);
        } 
    }

    void OnUpdateCoinProgressSlider(int UpdatedCurrentSessionCoinCount)
    {
        if (m_CoinProgressSlider)
        {
            m_CoinProgressSlider.value = UpdatedCurrentSessionCoinCount;
        }
    }
}
