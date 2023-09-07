using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField] public ButtonType _ignoreType;
    Stack<AlienEvent> _currentEvents = new();
    SpriteRenderer _sprite;

    public IReadOnlyCollection<AlienEvent> CurrentEvents { get => _currentEvents; }

    public int ID { get; set; }

    public void ApplyStatus(ButtonType type)
    {
        if (_currentEvents.Count <= 0)
            return;

        switch (type)
        {
            case ButtonType.FOOD when _ignoreType != ButtonType.FOOD:
                if(_currentEvents.Peek().Type == AlienEventType.HUNGRY)
                {
                    ResolveStatus();
                }
                break;
            case ButtonType.SANITY when _ignoreType != ButtonType.SANITY:
                if (_currentEvents.Peek().Type == AlienEventType.DEPRESSED)
                {
                    ResolveStatus();
                }
                break;
            case ButtonType.HEALTH when _ignoreType != ButtonType.HEALTH:
                if (_currentEvents.Peek().Type == AlienEventType.SICK)
                {
                    ResolveStatus();
                }
                break;
            case ButtonType.TASER when _ignoreType != ButtonType.TASER:
                if (_currentEvents.Peek().Type == AlienEventType.ANGRY)
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
                _currentEvents.Push(
                    new AlienEvent(AlienEventType.HUNGRY, ID)
                    );
                break;
            case 1:
                _currentEvents.Push(
                    new AlienEvent(AlienEventType.DEPRESSED, ID)
                    );
                break;
            case 2:
                _currentEvents.Push(
                    new AlienEvent(AlienEventType.SICK, ID)
                    );
                break;
            case 3:
                _currentEvents.Push(
                    new AlienEvent(AlienEventType.ANGRY, ID)
                    );
                break;

        }

        PlayAlienAnimation();
    }

    private void PlayAlienAnimation()
    {
        if(_currentEvents.Count <= 0)
        {
            _sprite.DOColor(Color.white, 1.5f);
        }

        switch (_currentEvents.Peek().Type)
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
        _currentEvents.Pop();
        PlayAlienAnimation();
    }
}
