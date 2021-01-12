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
    [SerializeField, ShowOnly] string m_userName; // �ӽ�
    [SerializeField, ShowOnly] string m_channelName;  // �ӽ�

    [SerializeField] TMP_InputField m_InputField;
    [SerializeField] TextMeshProUGUI m_outputText;

    [SerializeField] TMP_InputField m_termainlInputField;
    [SerializeField] TextMeshProUGUI m_termainlOutputText;

    [SerializeField, ShowOnly] public bool isGameStart;

    override protected void Awake()
    {
        m_networkInfoManager = _NetworkInfoManager.Instance;               

        // ä�� ���� �� �ʱ�ȭ
        __Initialize();
    }

    private void __Initialize()
    {
        // ���� ���� �ʱ�ȭ
        m_userName = m_networkInfoManager.m_playerInfo.nickname;
        m_channelName = m_networkInfoManager.m_playerInfo.currRoomName;

        // IChatClientListener �����ϴ� obj(this)
        m_chatClient = new ChatClient(this);

        // chat server ����
        m_chatClient.Connect(
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,       // setting ����
            PhotonNetwork.AppVersion,
            new AuthenticationValues(m_userName));

        // output â ��� ����
        FlushAllText();
        // AddLine("���� �õ�...");
                

        // ���� ���� flag
        isGameStart = false;
    }

    private void Update()
    {
        //Debug.Log(PhotonNetwork.IsMasterClient);
        // ä�� ����
        m_chatClient.Service();
    }

    public void OnInputText()
    {        
        if (m_chatClient.State == ChatState.ConnectedToFrontEnd)
        {
            // �ش� ä�ο�(m_channelName), �Է� �޼���(inputField.text) �Ѹ���
            m_chatClient.PublishMessage(m_channelName, m_InputField.text);

            m_InputField.text = "";
        }
    }
    public void SendText(string p_text)
    {
        if (m_chatClient.State == ChatState.ConnectedToFrontEnd)
        {
            // �ش� ä�ο�(m_channelName), �Է� �޼���(inputField.text) �Ѹ���
            m_chatClient.PublishMessage(m_channelName, p_text);          
        }
    }

    // ���� ���۽� ȣ���� �ּ���
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

    /// ���� IChatClientListener function
    /// //////////////////////////////////////////////////////


    // ���� ����� ȣ��
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

    // ���� ��ȯ ��
    public void OnChatStateChange(ChatState state)
    {
        Debug.Log("OnChatStateChange = " + state);
    }

    // ��Ʈ��ũ ����Ǿ��� ��
    public void OnConnected()
    {
        AddLine("ä�� ������ ����Ǿ����ϴ�.");

        // ä�� ����(ä�ù� ����)
        m_chatClient.Subscribe(m_channelName);
    }

    // ��Ʈ��ũ ������ ������ ��
    public void OnDisconnected()
    {
        AddLine("ä�� ������ ������ ���������ϴ�.");
        SendText(string.Format("{0} ����111 �� �־ȴ��������", m_userName));
    }

    // �޼��� ����
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

    // ���� �޼���
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        throw new System.NotImplementedException();
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        throw new System.NotImplementedException();
    }

    // ä�� ����
    public void OnSubscribed(string[] channels, bool[] results)
    {
        AddLine(string.Format("ä�� ���� ({0})", string.Join(",", channels)));
        SendText(string.Format("{0} ����", m_userName));        
    }

    // ä�� ������
    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("ä�� ���� ({0})", string.Join(",", channels)));
        SendText(string.Format("{0} ����222", m_userName));
    }

    // �ٸ� �� ������ ä�� ����
    public void OnUserSubscribed(string channel, string user)
    {
        AddLine(string.Format($"{user} ����"));
        //throw new System.NotImplementedException();
    }

    // �ٸ� �� ������ ä�� ����
    public void OnUserUnsubscribed(string channel, string user)
    {
        AddLine(string.Format($"{user} ����"));
        //throw new System.NotImplementedException();
    }

}
