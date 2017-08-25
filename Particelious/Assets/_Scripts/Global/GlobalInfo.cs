using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GlobalInfo : MonoBehaviour{
 
    static protected GlobalInfo s_Instance = null;
    static public GlobalInfo instance
    {
        get
        {
            if (s_Instance == null)
            {
                GameObject o = new GameObject("GameInfo");
                DontDestroyOnLoad(o);
                s_Instance = o.AddComponent<GlobalInfo>();

                Debug.LogWarning("Manually constructed GameInfo object, shouldn't usually happen.");
            }
            return s_Instance;
        }
    }

    [SerializeField]
    private Text m_CoinText = null;
    const string m_CoinDisplay = "x{0}";

    private int m_GlobalCoinCount = 0;
    public int CointCount { get { return m_GlobalCoinCount; } set { m_GlobalCoinCount = value; } }

    private int m_CoinsFromCurrentSession = 0;
    public int CoinsFromCurrentSession { get { return m_CoinsFromCurrentSession; } set { m_CoinsFromCurrentSession = value; } }

    private int m_CurrentLevel = 1;
    public int CurrentLevel { get { return m_CurrentLevel; } set { m_CurrentLevel = value; } }

    void Start()
    {
        if (null != s_Instance)
        {
            Destroy(this.gameObject);
            return;
        }

        s_Instance = this;
        DontDestroyOnLoad(this.gameObject);

        GameManager.instance.OnEndedGameSession += OnRestartLevel;
        UpdateCoinText();
    }

    void OnDestroy()
    {
        if (this == s_Instance)
            s_Instance = null;
    }

    public void OnCollectedCoin()
    {
        m_GlobalCoinCount++;
        CoinsFromCurrentSession++;
        UpdateCoinText();
    }

    public void OnAddGlobalCoins(int AddAmount)
    {
        m_GlobalCoinCount += AddAmount;
        UpdateCoinText();
    }

    private void OnRestartLevel()
    {
        CoinsFromCurrentSession = 0;
    }

    private void UpdateCoinText()
    {
        if(m_CoinText)
            m_CoinText.text = string.Format(m_CoinDisplay, m_GlobalCoinCount);
    }
}
