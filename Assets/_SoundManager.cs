using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _SoundManager : Singleton<_SoundManager>, IAwake
{
    _OptionInfoManager m_optionManager = null;

    _ResourceLoader m_resourceLoader = null;
    Dictionary<E_BGMSound, AudioClip> m_bgmDic = null;
    Dictionary<E_UISound, AudioClip> m_uiDic = null;
    Dictionary<E_ObjectInterationSound, AudioClip> m_interationDic = null;


    private Queue<AudioSource> m_ui_audio_queue = null;
    private AudioSource m_bgm_audio = null;

    private AudioSource[] _audios = null;

    public void __Awake()
    {
        m_optionManager = _OptionInfoManager.Instance;

        // dictionary ��������
        m_resourceLoader = _ResourceLoader.Instance;
        m_bgmDic = m_resourceLoader.m_BGMSoundDics;
        m_uiDic = m_resourceLoader.m_UISoundDics;
        m_interationDic = m_resourceLoader.m_ObjectInterationSoundDics;

        // audio ������Ʈ ����
        _audios = GetComponents<AudioSource>();

        __InitBgm();
        __InitUI();

    }

    public void _ApplyOptions()
    {
        __SetVolume();
        __SetEffecVolume();
    }


    #region BGM

    // ��� ����� loop

    private void __InitBgm()
    {
        m_bgm_audio = _audios[0];

        m_bgm_audio.volume = 0.5f;
        m_bgm_audio.loop = true;
        PlayBGMSound(E_BGMSound.bgm_title);
    }

    private void __SetVolume()
    {
        m_bgm_audio.volume = m_optionManager.m_currentOptionInfo.volume_bgm * 0.5f;
    }

    public void PlayBGMSound(E_BGMSound p_soundName)
    {
        m_bgm_audio.clip = m_bgmDic[p_soundName];
        m_bgm_audio.Play();
    }

    #endregion

    #region UI & ObjectInteration

    // ��� ����� �ܹ߼���
    // ���ÿ� ����Ǵ� Sound�� AudioSource ������ ���� �ʴ´ٰ� ������
    // Queue�� �����Ǿ� ������ �Ѿ �� ���� ����� ���尡 ���� �� ����
    // �ʿ� �� ������Ʈ ���� �߰� �ٶ�

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

    private void __SetEffecVolume()
    {
        foreach (var item in m_ui_audio_queue)
        {
            item.volume = m_optionManager.m_currentOptionInfo.volume_effect;
        }
    }

    public void PlayUISound(E_UISound p_soundName)
    {
        if (m_ui_audio_queue.Count > 0)
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
