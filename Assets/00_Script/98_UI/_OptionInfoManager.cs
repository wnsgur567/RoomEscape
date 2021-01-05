using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 유저 선택한 선택한, 현재 옵션 정보
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

    // 16:9 베이스
    public string ratio;
#endregion

    #region MUSIC
    // music bool type ( false 시 음소거 상태 )
    public bool b_muste;            // 모든 사운드 재생 여부
    public bool b_sound_bgm;        // bgm 재생 여부 
    public bool b_sound_ui;         // ui 효과음 재생 여부
    public bool b_sound_effect;     // 일반 효과음 재생 여부

    // music 크기 ( 0 ~ 1 )
    [Range(0f, 1f)] public float volume;        // 모든 사운드 크기
    [Range(0f, 1f)] public float volume_bgm;    // bgm 사운드 크기
    [Range(0f, 1f)] public float volume_ui;     // ui 사운드 크기
    [Range(0f, 1f)] public float volume_effect; // effect 사운드 크기
    #endregion
}

public class _OptionInfoManager : Singleton<_OptionInfoManager>
{
    public CurrentOptionInfo m_currentOptionInfo;
}
