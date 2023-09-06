using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    Stack<AlienEvent> _currentEvent = new();
    SpriteRenderer _sprite;

    public int ID { get; set; }

    public void ApplyStatus(ButtonType type)
    {
        if (_currentEvent.Count <= 0)
            return;

        switch (type)
        {
            case ButtonType.FOOD:
                if(_currentEvent.Peek().Type == AlienEventType.HUNGRY)
                {
                    ResolveStatus();
                }
                break;
            case ButtonType.SANITY:
                if (_currentEvent.Peek().Type == AlienEventType.DEPRESSED)
                {
                    ResolveStatus();
                }
                break;
            case ButtonType.HEALTH:
                if (_currentEvent.Peek().Type == AlienEventType.SICK)
                {
                    ResolveStatus();
                }
                break;
            case ButtonType.TASER:
                if (_currentEvent.Peek().Type == AlienEventType.ANGRY)
                {
                    ResolveStatus();
                }
                break;
        }
    }

    private void Awake()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    internal void ApplyGameEvent()
    {
        switch(Random.Range(0, 4))
        {
            case 0:
                _currentEvent.Push(
                    new AlienEvent(AlienEventType.HUNGRY, ID)
                    );
                break;
            case 1:
                _currentEvent.Push(
                    new AlienEvent(AlienEventType.DEPRESSED, ID)
                    );
                break;
            case 2:
                _currentEvent.Push(
                    new AlienEvent(AlienEventType.SICK, ID)
                    );
                break;
            case 3:
                _currentEvent.Push(
                    new AlienEvent(AlienEventType.ANGRY, ID)
                    );
                break;

        }

        PlayAlienAnimation();
    }

    private void PlayAlienAnimation()
    {
        if(_currentEvent.Count <= 0)
        {
            _sprite.DOColor(Color.white, 1.5f);
        }

        switch (_currentEvent.Peek().Type)
        {
            case AlienEventType.HUNGRY:
                _sprite.DOColor(Color.yellow, 1.5f);
                break;
            case AlienEventType.DEPRESSED:
                _sprite.DOColor(Color.blue, 1.5f);
                break;
            case AlienEventType.ANGRY:
                _sprite.DOColor(Color.red, 1.5f);
                break;
            case AlienEventType.SICK:
                _sprite.DOColor(Color.green, 1.5f);
                break;
        }
    }

    private void ResolveStatus()
    {
        _currentEvent.Pop();
        PlayAlienAnimation();
    }
}
