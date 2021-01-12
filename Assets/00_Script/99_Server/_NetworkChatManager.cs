using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;


public class _NetworkChatManager : Singleton<_NetworkChatManager>, IChatClientListener
{
    _NetworkInfoManager m_networkInfoManager = null;

	private ChatClient m_chatClient;
    [SerializeField, ShowOnly] string m_userName; // 임시
    [SerializeField, ShowOnly] string m_channelName;  // 임시

    [SerializeField] TMP_InputField m_InputField;
    [SerializeField] TextMeshProUGUI m_outputText;

    [SerializeField] TMP_InputField m_termainlInputField;
    [SerializeField] TextMeshProUGUI m_termainlOutputText;

    [SerializeField, ShowOnly] public bool isGameStart;

    override protected void Awake()
    {
        m_networkInfoManager = _NetworkInfoManager.Instance;               

        // 채팅 연결 및 초기화
        __Initialize();
    }

    private void __Initialize()
    {
        // 유저 정보 초기화
        m_userName = m_networkInfoManager.m_playerInfo.nickname;
        m_channelName = m_networkInfoManager.m_playerInfo.currRoomName;

        // IChatClientListener 포함하는 obj(this)
        m_chatClient = new ChatClient(this);

        // chat server 연결
        m_chatClient.Connect(
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,       // setting 참조
            PhotonNetwork.AppVersion,
            new AuthenticationValues(m_userName));

        // output 창 모두 비우기
        FlushAllText();
        // AddLine("연결 시도...");
                

        // 게임 시작 flag
        isGameStart = false;
    }

    private void Update()
    {
        //Debug.Log(PhotonNetwork.IsMasterClient);
        // 채팅 서비스
        m_chatClient.Service();
    }

    public void OnInputText()
    {        
        if (m_chatClient.State == ChatState.ConnectedToFrontEnd)
        {
            // 해당 채널에(m_channelName), 입력 메세지(inputField.text) 뿌리기
            m_chatClient.PublishMessage(m_channelName, m_InputField.text);

            m_InputField.text = "";
        }
    }
    public void SendText(string p_text)
    {
        if (m_chatClient.State == ChatState.ConnectedToFrontEnd)
        {
            // 해당 채널에(m_channelName), 입력 메세지(inputField.text) 뿌리기
            m_chatClient.PublishMessage(m_channelName, p_text);          
        }
    }

    // 게임 시작시 호출해 주세요
    public void _GameStart()
    {
        isGameStart = true;
    }

    public void FlushAllText()
    {
        m_outputText.text = "";
        m_termainlOutputText.text = "";
    }

    public void AddLine(TextMeshProUGUI p_outputText ,string p_lineString)
    {
        p_outputText.text += p_lineString + "\r\n";        
    }
    private void AddLine(string p_lineString)
    {
        m_outputText.text += p_lineString + "\r\n";
    }
    private void AddLineTermianl(string p_lineString)
    {
        m_termainlOutputText.text += p_lineString + "\r\n";
    }

    /// 이하 IChatClientListener function
    /// //////////////////////////////////////////////////////


    // 내부 디버깅 호출
    public void DebugReturn(DebugLevel level, string message)
    {
        if (level == ExitGames.Client.Photon.DebugLevel.ERROR)
        {
            Debug.LogError(message);
        }
        else if (level == ExitGames.Client.Photon.DebugLevel.WARNING)
        {
            Debug.LogWarning(message);
        }
        else
        {
            Debug.Log(message);
        }
    }

    // 상태 변환 시
    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    // 네트워크 연결되었을 때
    public void OnConnected()
    {
        AddLine("채팅 서버에 연결되었습니다.");

        // 채널 참가(채팅방 참가)
        m_chatClient.Subscribe(m_channelName);
    }

    // 네트워크 연결이 끊겼을 때
    public void OnDisconnected()
    {
        AddLine("채팅 서버와 연결이 끊어졌습니다.");
        SendText(string.Format("{0} 퇴장111 아 왜안대ㅐㅐㅐㅐ", m_userName));
    }

    // 메세지 수신
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            if (isGameStart)
                AddLineTermianl(string.Format("{0} : {1}", senders[i], messages[i].ToString()));
            else
                AddLine(string.Format("{0} : {1}", senders[i], messages[i].ToString()));
        }
    }

    // 개인 메세지
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    // 채팅 참가
    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("채널 입장 ({0})", string.Join(",", channels)));
        SendText(string.Format("{0} 입장", m_userName));        
    }

    // 채팅 나가기
    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("채널 퇴장 ({0})", string.Join(",", channels)));
        SendText(string.Format("{0} 퇴장222", m_userName));
    }

    // 다른 한 유저가 채팅 참가
    public void OnUserSubscribed(string channel, string user)
    {
        AddLine(string.Format($"{user} 참가"));
        //throw new System.NotImplementedException();
    }

    // 다른 한 유저가 채팅 나감
    public void OnUserUnsubscribed(string channel, string user)
    {
        AddLine(string.Format($"{user} 퇴장"));
        //throw new System.NotImplementedException();
    }

}
