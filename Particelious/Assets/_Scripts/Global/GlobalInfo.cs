using UnityEngine;

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

                Debug.LogWarning("Manually constructed GameInfo object, usually shouldn't happen.");
            }
            return s_Instance;
        }
    }

    public System.Action<int> OnGlobalCoinCountChanged;
    public System.Action<int> OnCurrentSessionCoinCountChanged;

    private int m_GlobalCoinCount = 0;
    public int CointCount { get { return m_GlobalCoinCount; } set { m_GlobalCoinCount = value; } }

    private int m_CoinsFromCurrentSession = 0;
    public int CoinsFromCurrentSession { get { return m_CoinsFromCurrentSession; } set { m_CoinsFromCurrentSession = value; } }

    private int m_CurrentLevel = -1;
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
    }

    void OnDestroy()
    {
        if (this == s_Instance)
            s_Instance = null;
    }

    public void CollectedCoin()
    {
        m_GlobalCoinCount++;
        CoinsFromCurrentSession++;
        UpdateCoinInfo();
    }

    public void AddGlobalCoins(int AddAmount)
    {
        m_GlobalCoinCount += AddAmount;
        UpdateCoinInfo();
    }

    private void OnExitRestart()
    {
        CoinsFromCurrentSession = 0;
        UpdateCoinInfo();
    }

    public void UpdateCoinInfo()
    {
        if (null != OnGlobalCoinCountChanged)
        {
            OnGlobalCoinCountChanged.Invoke(m_GlobalCoinCount);
        }
        if(null != OnCurrentSessionCoinCountChanged)
        {
            OnCurrentSessionCoinCountChanged.Invoke(m_CoinsFromCurrentSession);
        }
    }
}
