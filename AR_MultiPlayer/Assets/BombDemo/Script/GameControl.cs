using System.Collections;
using System.Collections.Generic;
using Niantic.ARDK.Extensions;
using Niantic.ARDK.Networking;
using Niantic.ARDK.Networking.HLAPI.Object.Unity;
using Niantic.ARDKExamples.Helpers;
using UnityEngine;
using static MessagingManager;

public static class GameVal
{
    public static GameControl gc;
}

public class GameControl : MonoBehaviour
{
    private GameTimer gameTimer;
    public InterfaceReference ir;

    [Header("Connection Data")]
    public ARSessionManager _ARSessionManager;
    public ARPlaneManager _ARPlaneManager;
    public SessionIDGenerator _SessionIDGenerator;
    public SessionIDField _SessionIDField;
    public ARNetworkingManager _ARNetworkingManager;
    public MessagingManager messagingManager;
    public SyncStateTrackingList _SyncStateTrackingList;

    [Header("Connection Data")]
    public bool isHost;

    [Header("Page Data")]
    public int _pageNow;
    public bool gameEnabled;
    public List<int> _scoreList = new List<int>();
    public Dictionary<IPeer, int> _peerScoreList = new Dictionary<IPeer, int>();




    [Header("Player Data")]
    public int score;
    public int scorePerExplosion = 300;


    [Header("Spawn Data")]
    public NetworkedUnityObject _objectToNetworkSpawn;
    public List<NetworkedUnityObject> _spawnedObjectList = new List<NetworkedUnityObject>();


    private void Awake()
    {
        GameVal.gc = this;
    }

    #region Init
    void Start()
    {
        gameTimer = GetComponent<GameTimer>();
        gameTimer.Init();

        Init();
    }

    public void Init()
    {
        score = 0;
        gameEnabled = false;

        ir.hostStartGameButton.SetActive(true);
        ir.hostStartGameMsg.SetActive(true);
        ir.clientStartGameButton.SetActive(true);
        ir.clientStartGameMsg.SetActive(true);


        //Reset Score UI
        foreach (IPeer peer in _SyncStateTrackingList._peerTrackerDict.Keys)
        {
            ScoreUI scoreUI = _SyncStateTrackingList._peerTrackerDict[peer].GetComponentInChildren<ScoreUI>();
            scoreUI.scoreText.text = "0";
        }

        _peerScoreList.Clear();

        _SessionIDField.text = "";
        ir.codeInput.text = "";
        ir.codeDisplay.text = "";

        PageChange(0);
    }
    #endregion


    #region Game Flow

    public void PageChange(int page)
    {
        _pageNow = page;
        foreach (GameObject go in ir._pageObjs)
            go.SetActive(false);
        ir._pageObjs[_pageNow].SetActive(true);
    }
    public void ShowHideHostClientObjects()
    {
        ir.hostStartGameButton.SetActive(true);
        ir.hostStartGameMsg.SetActive(true);
        ir.clientStartGameButton.SetActive(true);
        ir.clientStartGameMsg.SetActive(true);

        foreach (GameObject go in ir._hostObjs)
            go.SetActive(isHost);
        foreach (GameObject go in ir._clientObjs)
            go.SetActive(!isHost);
    }



    public void HostStart()
    {
        //Start AR Session
        //Connect AR Network
        //Debug.Log("Host Connect to AR Network");
        _SessionIDGenerator.AssignRandomText();
        _ARNetworkingManager.EnableFeatures();
        ir.codeDisplay.text = _SessionIDField.text;


        isHost = true;
        ShowHideHostClientObjects();

        //Go to Page 1 – Code
        PageChange(1);
    }

    public void HostEnterGame()
    {
        //Hide timer before game start
        gameTimer.timerBlock.SetActive(false);

        //Go to Page 2 - Game Page
        PageChange(2);
    }

    public void HostStartGame()
    {
        //Send Game Start to peer

        //Hide msg
        ir.hostStartGameButton.SetActive(false);
        ir.hostStartGameMsg.SetActive(false);

        //Send game state
        messagingManager.BroadcastString((uint)MessageType.GameState, "Start", TransportType.ReliableUnordered);

        //Game start
        GameStart();
    }

    public void HostSendRestart()
    {

        //Send game state
        messagingManager.BroadcastString((uint)MessageType.GameState, "Restart", TransportType.ReliableUnordered);

        //Go back to page 0
        GameRestart();
    }


    public void ClientStart()
    {
        isHost = false;
        ShowHideHostClientObjects();

        //Go to Page 1 – Code
        PageChange(1);
    }
    public void ClientConnectToGame()
    {
        //Start AR Session
        //Connect AR Network
        _SessionIDField.text = ir.codeInput.text;
        _ARNetworkingManager.EnableFeatures();

        //Hide timer before game start
        gameTimer.timerBlock.SetActive(false);

        //Go to Page 2 - Game Page
        PageChange(2);
    }
    public void ClientStartGame()
    {
        //Hide msg
        ir.clientStartGameButton.SetActive(false);
        ir.clientStartGameMsg.SetActive(false);

        //Game start
        GameStart();
    }



    public void GameStart()
    {
        gameEnabled = true;
        gameTimer.StartCounting();
    }

    public void GameRestart()
    {
        Init();

        //Go to game start screen
        PageChange(2);
    }

    public void GameResult()
    {
        ir.endScoreText.text = score.ToString();

        gameEnabled = false;
        //Check if I am winner

        //Show result
        DisplayResult(CheckIsWinner());

        //Go to page 4
        PageChange(3);
    }

    public void GameReset()
    {
        //Send game state
        if (isHost)
            messagingManager.BroadcastString((uint)MessageType.GameState, "Reset", TransportType.ReliableUnordered);

        Init();

        //End AR session
        //End Network session
        //Debug.Log("End AR Network");
        _ARPlaneManager.ClearAllPlanes();

        _ARNetworkingManager.DisableFeatures();
        _ARNetworkingManager.Deinitialize();

        //Go back to page 0
        PageChange(0);
    }




    #endregion



    #region Spawn/Destory

    public void SpawnObjectForAllPeers(Vector3 position, Quaternion rotation)
    {
        _spawnedObjectList.Add(_objectToNetworkSpawn.NetworkSpawn(position, rotation));
    }

    public void DestroyNetworkedUnityObject(NetworkedUnityObject objectToDestroy)
    {
        objectToDestroy.NetworkDestroy();
    }

    #endregion



    #region Score

    public void AddScore()
    {
        score += scorePerExplosion;
        Debug.Log("Score now: " + score);

        messagingManager.BroadcastString((uint)MessageType.PlayerScore, score.ToString(), TransportType.ReliableUnordered);
        UpdateScoreUI();
    }

    public void UpdateScoreUI()
    {
        //Put my score into the list
        foreach (IPeer peer in _SyncStateTrackingList._peerTrackerDict.Keys)
        {
            //is self
            if (_ARNetworkingManager.ARNetworking.Networking.Self == peer)
            {
                ScoreUI self_scoreUI = _SyncStateTrackingList._peerTrackerDict[peer].GetComponentInChildren<ScoreUI>();
                self_scoreUI.scoreText.text = score.ToString();
            }
        }

        //Find other peers score to put score UI
        foreach (IPeer peer in _peerScoreList.Keys)
        {
            ScoreUI scoreUI = _SyncStateTrackingList._peerTrackerDict[peer].GetComponentInChildren<ScoreUI>();
            scoreUI.scoreText.text = _peerScoreList[peer].ToString();
        }
    }

    public bool CheckIsWinner()
    {
        bool isWinner = false;

        //Populate _peerScoreList if the _scoreList if not match with peer list
        foreach (IPeer peer in _SyncStateTrackingList._peerTrackerDict.Keys)
            if (!_peerScoreList.ContainsKey(peer))
                _peerScoreList.Add(peer, 0);

        foreach (int _score in _peerScoreList.Values)
        {
            if (score > _score)
            {
                isWinner = true;
                return isWinner;
            }
        }

        return isWinner;
    }

    public void DisplayResult(bool _isWinner)
    {
        ir.winObj.SetActive(_isWinner);
        ir.loseObj.SetActive(!_isWinner);
    }

    #endregion


}