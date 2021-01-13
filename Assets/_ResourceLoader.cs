using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_SoundType
{
    None = 0,
    BGM = 1,
    UI = 2,
    Object = 3,
    ObjectInteraction = 4,

    Max
}

public enum E_BGMSound
{
    bgm_play = 0,
    bgm_title = 1,

    Max
}
public enum E_UISound
{
    drawer_close = 0,
    drawer_open = 1,
    click = 2,

    Max
}
public enum E_ObjectSound
{
    bomb_cut = 0,
    bomb_key = 1,
    bomb_keyopen = 2,
    bomb_button = 3,
    bomb_fail = 4,
    chess_chess = 5,
    pipe_valve = 6,
    pipe_broke = 7,
    book_next = 8,
    keyboard_click = 9,
    user_walk = 10,
    user_walk_metal = 11,

    Max
}
public enum E_ObjectInterationSound
{
    bomb_Timer = 0,
    bomb_TimerLast = 1,
    pipe_waterdrop = 2,
    pipe_vapor = 3,
    radio_morse = 4,

    Max
}

public class _ResourceLoader : Singleton<_ResourceLoader> , IAwake
{
    public Dictionary<E_BGMSound, AudioClip> m_BGMSoundDics = null;
    public Dictionary<E_UISound, AudioClip> m_UISoundDics = null;
    public Dictionary<E_ObjectSound, AudioClip> m_ObjectSoundDics = null;
    public Dictionary<E_ObjectInterationSound, AudioClip> m_ObjectInterationSoundDics = null;

    [SerializeField] string m_folderPath_BGM;
    [SerializeField] string m_folderPath_UI;
    [SerializeField] string m_folderPath_Object;
    [SerializeField] string m_folderPath_ObjectInteration;

    public void __Awake()
    {  

        m_BGMSoundDics = ResourcesManager.LoadAudios<E_BGMSound>(m_folderPath_BGM);
        m_UISoundDics = ResourcesManager.LoadAudios<E_UISound>(m_folderPath_UI);
        m_ObjectSoundDics = ResourcesManager.LoadAudios<E_ObjectSound>(m_folderPath_Object);
        m_ObjectInterationSoundDics = ResourcesManager.LoadAudios<E_ObjectInterationSound>(m_folderPath_ObjectInteration);
    }

  
}
