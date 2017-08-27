using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoinInfoController : MonoBehaviour {

    [SerializeField]
    private Text m_CoinText = null;
    const string m_CoinDisplay = "x{0}";

    [SerializeField]
    private Slider m_CoinProgressSlider = null;

    // Use this for initialization

    void Start () {
        GlobalInfo.instance.OnGlobalCoinCountChanged += OnUpdateCoinText;
        GlobalInfo.instance.OnCurrentSessionCoinCountChanged += OnUpdateCoinProgressSlider;
        GlobalInfo.instance.UpdateCoinInfo();
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
            if(UpdatedCurrentSessionCoinCount > 0)
            {
                m_CoinProgressSlider.gameObject.SetActive(true);
                m_CoinProgressSlider.value = UpdatedCurrentSessionCoinCount;
            }
            else
            {
                m_CoinProgressSlider.gameObject.SetActive(false);
            }
        }
    }
}
