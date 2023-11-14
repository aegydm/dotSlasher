using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LinkedBattleField
{
    public FieldCardObject First => _first;
    public FieldCardObject Last => _last;
    private FieldCardObject _first, _last, _tmp;

    public FieldCardObject this[int index]
    {
        get
        {
            FieldCardObject temp = First;
            for (int i = 0; i < index; i++)
            {
                temp = temp.Next;
            }
            if (temp == null)
            {
                Debug.LogError("IndexOutOfRange!");
            }
            return temp;
        }
        set
        {
            FieldCardObject temp = First;
            for (int i = 0; i < index; i++)
            {
                temp = temp.Next;
            }
            if (temp == null)
            {
                Debug.LogError("IndexOutOfRange!");
            }
            temp = value;
        }
    }

    public int Count()
    {
        return FindIndex(Last) + 1;
    }

    public int FindIndex(FieldCardObject field)
    {
        FieldCardObject tmp = First;
        int i = 0;
        for (; tmp != field; i++)
        {
            if (tmp == null)
            {
                Debug.LogError("There is no Field in LinkedBattleField");
                return -1;
            }
            tmp = tmp.Next;
        }
        return i;
    }

    public void AddFirst(GameObject gameObject)
    {
        _tmp = gameObject.GetComponent<FieldCardObject>();

        if (_first != null)
        {
            _tmp.Next = _first;
            _first.Prev = _tmp;
        }
        else
        {
            _last = _tmp;
        }

        _first = _tmp;
    }

    public void AddLast(GameObject gameObject)
    {
        _tmp = gameObject.GetComponent<FieldCardObject>();
        if (_last != null)
        {
            _tmp.Prev = _last;
            _last.Next = _tmp;
        }
        else
        {
            _first = _tmp;
        }

        _last = _tmp;
    }

    public void AddBefore(FieldCardObject battleField, GameObject gameObject)
    {
        _tmp = gameObject.GetComponent<FieldCardObject>();

        if (battleField.Prev != null)
        {
            battleField.Prev.Next = _tmp;
            _tmp.Prev = battleField.Prev;
        }
        else
        {
            _first = _tmp;
        }

        _tmp.Next = battleField;
        battleField.Prev = _tmp;
    }

    public void AddAfter(FieldCardObject battlefield, GameObject gameObject)
    {
        _tmp = gameObject.GetComponent<FieldCardObject>();

        if (battlefield.Next != null)
        {
            battlefield.Next.Prev = _tmp;
            _tmp.Next = battlefield.Next;
        }
        else
        {
            _last = _tmp;
        }

        _tmp.Prev = battlefield;
        battlefield.Next = _tmp;
    }

    public void Add(GameObject gameObject)
    {
        if (_first == null)
        {
            AddFirst(gameObject);
        }
        else
        {
            AddAfter(Last, gameObject);
        }
    }

    public FieldCardObject Find(GameObject gameObject)
    {
        _tmp = _first;
        while (_tmp.Next != null)
        {
            if (_tmp.gameObject == gameObject)
            {
                return _tmp;
            }
            _tmp = _tmp.Next;
        }
        if (_tmp.gameObject == gameObject)
        {
            return _tmp;
        }
        return null;
    }

    public FieldCardObject FindLast(GameObject gameObject)
    {
        _tmp = _last;
        while (_tmp.Prev != null)
        {
            if (_tmp.gameObject == gameObject)
            {
                return _tmp;
            }
            _tmp = _tmp.Prev;
        }
        if (_tmp.gameObject == gameObject)
        {
            return _tmp;
        }
        return null;

    }

    public bool Remove(FieldCardObject battleField)
    {
        if (battleField == null)
        {
            return false;
        }

        if (battleField.Prev != null)
        {
            battleField.Prev.Next = battleField.Next;
        }
        else
        {
            _first = battleField.Next;
        }

        if (battleField.Next != null)
        {
            battleField.Next.Prev = battleField.Prev;
        }
        else
        {
            _last = battleField.Prev;
        }

        return true;
    }

    public bool Remove(GameObject gameObject)
    {
        return Remove(Find(gameObject));
    }

    public bool RemoveLast(GameObject gameObject)
    {
        return Remove(FindLast(gameObject));
    }
}
