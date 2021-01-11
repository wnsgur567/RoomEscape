using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _SoundManager : MonoBehaviour
{
    _ResourceLoader m_resourceLoader = null;
    Dictionary<E_BGMSound, AudioClip> m_bgmDic = null;
    Dictionary<E_UISound, AudioClip> m_uiDic = null;
    Dictionary<E_ObjectInterationSound, AudioClip> m_interationDic = null;


    private Queue<AudioSource> m_ui_audio_queue = null;
    private AudioSource m_bgm_audio = null;

    private AudioSource[] _audios = null;

    private void Awake()
    {
        // dictionary 가져오기
        m_resourceLoader = _ResourceLoader.Instance;
        m_bgmDic = m_resourceLoader.m_BGMSoundDics;
        m_uiDic = m_resourceLoader.m_UISoundDics;
        m_interationDic = m_resourceLoader.m_ObjectInterationSoundDics;

        // audio 컴포넌트 셋팅
        _audios = GetComponents<AudioSource>();

        __InitBgm();
        __InitUI();
    }


    #region BGM

    // 모든 사운드는 loop

    private void __InitBgm()
    {
        m_bgm_audio = _audios[0];

        m_bgm_audio.loop = true;        
    }

    public void ChangeAndPlayBGMSound(E_BGMSound p_soundName)
    {
        m_bgm_audio.clip = m_bgmDic[p_soundName];
        m_bgm_audio.Play();
    }
    #endregion

    #region UI & ObjectInteration
    
    // 모든 사운드는 단발성임
    // 동시에 실행되는 Sound는 AudioSource 개수를 넘지 않는다고 가정함
    // Queue로 구현되어 개수를 넘어갈 시 먼저 실행된 사운드가 씹힐 수 있음
    // 필요 시 컴포넌트 개수 추가 바람

    private void __InitUI()
    {
        m_ui_audio_queue = new Queue<AudioSource>();
        for (int i = 1; i < _audios.Length; i++)
        {
            _audios[i].loop = false;
            _audios[i].Stop();
            m_ui_audio_queue.Enqueue(_audios[i]);
        }
    }

    public void PlayUISound(E_UISound p_soundName)
    {
        if(m_ui_audio_queue.Count > 0)
        {
            AudioSource _source = m_ui_audio_queue.Dequeue();
            _source.clip = m_uiDic[p_soundName];
            _source.Play();
            m_ui_audio_queue.Enqueue(_source);
        }
    }
    public void PlayObjInterationSound(E_ObjectInterationSound p_soundName)
    {
        if (m_ui_audio_queue.Count > 0)
        {
            AudioSource _source = m_ui_audio_queue.Dequeue();
            _source.clip = m_interationDic[p_soundName];
            _source.Play();
            m_ui_audio_queue.Enqueue(_source);
        }
    }
    #endregion
}
