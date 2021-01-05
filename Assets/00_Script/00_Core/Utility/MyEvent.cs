using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// T 타입으로 넘겨줄 args의 구조체를 정의해 줄 것
public class MyEvent<T> where T : struct
{
    public enum EventOrder
    {
        Standard = 0,   // after가 기본
        Front = 1,
        After = 2,
    }
    public delegate void MyEventHandler(GameObject sender, T args);

    // 이벤트 링크 리스트
    private LinkedList<KeyValuePair<string, MyEventHandler>> Handlers;

    // 생성자
    public MyEvent()
    {
        Handlers = new LinkedList<KeyValuePair<string, MyEventHandler>>();
    }

    // 이벤트 리스트 맨 뒤로 등록
    public void AddHandler(string p_key_name, MyEventHandler p_handler)
    {
        int cur_size = Handlers.Count;
        Handlers.AddLast(new KeyValuePair<string, MyEventHandler>(p_key_name, p_handler));
    }

    // 리스트에서 _flag로 검색하여 앞 또는 뒤로 링크 추가
    public void AddHandler(string p_key, MyEventHandler p_handler, string p_find_key, EventOrder _order = EventOrder.After)
    {
        int cur_size = Handlers.Count;
        LinkedListNode<KeyValuePair<string, MyEventHandler>> _find_node = null;
        foreach (var item in Handlers)
        {
            if (item.Key == p_find_key)
            {
                _find_node = Handlers.Find(item);
                break;
            }
        }
        
        // 탐색 실패 , 해당 key 없음
        if(_find_node == null)
        {
            return;
        }

        // 탐색 성공
        // 지정된 order에 따라 링크 수행
        switch (_order)
        {
            // 탐색한 delegate 앞에 연결
            case EventOrder.Front:
                {
                    Handlers.AddBefore(_find_node, new KeyValuePair<string, MyEventHandler>(p_key, p_handler) );
                }
                break;

            // 탐색한 delegate 뒤에 연결
            case EventOrder.Standard:
            case EventOrder.After:
                {
                    Handlers.AddAfter(_find_node, new KeyValuePair<string, MyEventHandler>(p_key, p_handler));
                }
                break;
        }
    }
    
    public void RemoveHandler(string p_find_key)
    {        
        foreach (var item in Handlers)
        {
            if(item.Key == p_find_key)
            {
                this.Handlers.Remove(item);
                break;
            }
        }        
    }
    public void RemoveHandler_All()
    {
        Handlers.Clear();        
    }

    public void Invoke(GameObject _Sender, T _Args)
    {
        foreach (var item in Handlers)
        {
            item.Value(_Sender, _Args);
        }
    }
}
