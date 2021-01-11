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
	private ChatClient m_chatClient;
    [SerializeField] string m_userName; // 임시
    [SerializeField] string m_channelName;  // 임시

    [SerializeField] TMP_InputField m_InputField;
    [SerializeField] TextMeshProUGUI m_outputText;


    private void Start()
    {
        // output 비우기
        m_outputText.text = "";
        AddLine("연결 시도...");

        // 채팅 연결 및 초기화
        __Initialize();
    }

    private void __Initialize()
    {
        // IChatClientListener 포함하는 obj(this)
        m_chatClient = new ChatClient(this);

        // chat server 연결
        m_chatClient.Connect(
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,       // setting 참조
            PhotonNetwork.AppVersion,
            new AuthenticationValues(m_userName));
    }

    private void Update()
    {
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

    public void AddLine(TextMeshProUGUI p_outputText ,string p_lineString)
    {
        p_outputText.text += p_lineString + "\r\n";        
    }
    private void AddLine(string p_lineString)
    {
        m_outputText.text += p_lineString + "\r\n";
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
        AddLine("서버에 연결되었습니다.");

        // 채널 참가(채팅방 참가)
        m_chatClient.Subscribe(m_channelName);
    }

    // 네트워크 연결이 끊겼을 때
    public void OnDisconnected()
    {
        AddLine("서버와 연결이 끊어졌습니다.");        
    }

    // 메세지 수신
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
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
    }

    // 채팅 나가기
    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("채널 퇴장 ({0})", string.Join(",", channels)));
    }

    // 다른 한 유저가 채팅 참가
    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    // 다른 한 유저가 채팅 나감
    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    

}
