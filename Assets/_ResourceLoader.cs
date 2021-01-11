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

}
public enum E_UISound
{

}
public enum E_ObjectSound
{

}
public enum E_ObjectInterationSound
{

}

public class _ResourceLoader : Singleton<_ResourceLoader>
{
    public Dictionary<E_BGMSound, AudioClip> m_BGMSoundDics = null;
    public Dictionary<E_UISound, AudioClip> m_UISoundDics = null;
    public Dictionary<E_ObjectSound, AudioClip> m_ObjectSoundDics = null;
    public Dictionary<E_ObjectInterationSound, AudioClip> m_ObjectInterationSoundDics = null;

    [SerializeField] string m_folderPath_BGM;
    [SerializeField] string m_folderPath_UI;
    [SerializeField] string m_folderPath_Object;
    [SerializeField] string m_folderPath_ObjectInteration;

    protected override void Awake()
    {
        base.Awake();

        m_BGMSoundDics = ResourcesManager.LoadAudios<E_BGMSound>(m_folderPath_BGM);
        m_UISoundDics = ResourcesManager.LoadAudios<E_UISound>(m_folderPath_UI);
        m_ObjectSoundDics = ResourcesManager.LoadAudios<E_ObjectSound>(m_folderPath_Object);
        m_ObjectInterationSoundDics = ResourcesManager.LoadAudios<E_ObjectInterationSound>(m_folderPath_ObjectInteration);
    }
}
