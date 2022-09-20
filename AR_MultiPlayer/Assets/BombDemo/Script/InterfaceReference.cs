using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceReference : MonoBehaviour
{
    [Header("Page")]
    public GameObject[] _pageObjs;

    [Header("Host/Client")]
    public GameObject[] _hostObjs;
    public GameObject[] _clientObjs;

    [Header("Interface")]
    
    public Text codeDisplay;
    public InputField codeInput;
    public Text timerText;
    public GameObject timerBlock;
    public GameObject hostStartGameMsg;
    public GameObject hostStartGameButton;
    public GameObject clientStartGameMsg;
    public GameObject clientStartGameButton;
    public GameObject winObj;
    public GameObject loseObj;
    public Text endScoreText;


}
