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
    [SerializeField] string m_userName; // �ӽ�
    [SerializeField] string m_channelName;  // �ӽ�

    [SerializeField] TMP_InputField m_InputField;
    [SerializeField] TextMeshProUGUI m_outputText;


    private void Start()
    {
        // output ����
        m_outputText.text = "";
        AddLine("���� �õ�...");

        // ä�� ���� �� �ʱ�ȭ
        __Initialize();
    }

    private void __Initialize()
    {
        // IChatClientListener �����ϴ� obj(this)
        m_chatClient = new ChatClient(this);

        // chat server ����
        m_chatClient.Connect(
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,       // setting ����
            PhotonNetwork.AppVersion,
            new AuthenticationValues(m_userName));
    }

    private void Update()
    {
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

    public void AddLine(TextMeshProUGUI p_outputText ,string p_lineString)
    {
        p_outputText.text += p_lineString + "\r\n";        
    }
    private void AddLine(string p_lineString)
    {
        m_outputText.text += p_lineString + "\r\n";
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
        AddLine("������ ����Ǿ����ϴ�.");

        // ä�� ����(ä�ù� ����)
        m_chatClient.Subscribe(m_channelName);
    }

    // ��Ʈ��ũ ������ ������ ��
    public void OnDisconnected()
    {
        AddLine("������ ������ ���������ϴ�.");        
    }

    // �޼��� ����
    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
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
    }

    // ä�� ������
    public void OnUnsubscribed(string[] channels)
    {
        AddLine(string.Format("ä�� ���� ({0})", string.Join(",", channels)));
    }

    // �ٸ� �� ������ ä�� ����
    public void OnUserSubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    // �ٸ� �� ������ ä�� ����
    public void OnUserUnsubscribed(string channel, string user)
    {
        //throw new System.NotImplementedException();
    }

    

}
