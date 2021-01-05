using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� ������ ������, ���� �ɼ� ����
[System.Serializable]
public struct CurrentOptionInfo
{
    #region SCREEN
    /*
     1280x720 => 720p(HD)
     1920x1080 => 1080p(HD)
     2560x1440 => 1440p(HD)
     3840x2160 => 2160p(4K)
     */
    public int resolution_width;
    public int resolution_height;

    // 16:9 ���̽�
    public string ratio;
#endregion

    #region MUSIC
    // music bool type ( false �� ���Ұ� ���� )
    public bool b_muste;            // ��� ���� ��� ����
    public bool b_sound_bgm;        // bgm ��� ���� 
    public bool b_sound_ui;         // ui ȿ���� ��� ����
    public bool b_sound_effect;     // �Ϲ� ȿ���� ��� ����

    // music ũ�� ( 0 ~ 1 )
    [Range(0f, 1f)] public float volume;        // ��� ���� ũ��
    [Range(0f, 1f)] public float volume_bgm;    // bgm ���� ũ��
    [Range(0f, 1f)] public float volume_ui;     // ui ���� ũ��
    [Range(0f, 1f)] public float volume_effect; // effect ���� ũ��
    #endregion
}

public class _OptionInfoManager : Singleton<_OptionInfoManager>
{
    public CurrentOptionInfo m_currentOptionInfo;
}
