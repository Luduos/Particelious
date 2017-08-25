using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Text))]
public class CollectedTextController : MonoBehaviour {
    [SerializeField]
    const string display = "Collected: {0} Coins";

    // Use this for initialization
    void Start () {
        this.enabled = false;
        Text collectedText = GetComponent<Text>();

        collectedText.text = string.Format(display, GlobalInfo.instance.CoinsFromCurrentSession);
    }
}
