using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteUI : MonoBehaviour
{
    public List<Sprite> sprite_list = null;

    Image NoteImg;
    private void Awake()
    {
        sprite_list = new List<Sprite>();
        _LoadSprites();
        NoteImg = GetComponent<Image>();
        NoteImg.transform.gameObject.SetActive(false);
    }

    [SerializeField] string m_folderName;

    [ContextMenu("LOADLOADLOAD")]
    public void _LoadSprites()
    {
        var sprites = Resources.LoadAll<Sprite>(m_folderName);
        foreach (var item in sprites)
        {
            sprite_list.Add(item);
        }
    }

    int CurCount;
    public void ActiveTrue()
    {
        NoteImg.transform.gameObject.SetActive(true);
        NoteImg.sprite = sprite_list[CurCount];
    }

    public void _OnNextButton()
    {
        if (CurCount >= sprite_list.Count - 1)
        {
            return;
        }
        NoteImg.sprite = sprite_list[++CurCount];

    }

    public void _OnPrevButton()
    {
        if (CurCount <= 0)
        {
            return;
        }
        NoteImg.sprite = sprite_list[--CurCount];

    }

    public void _OnBackButton()
    {
        NoteImg.transform.gameObject.SetActive(false);
    }
}
