using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using Math = System.Math;

public class Alien : MonoBehaviour
{
    [SerializeField] private string _animName;

    Animator _animator;
    AlienEvent _currentEvent;

    public AlienEvent CurrentEvent { get => _currentEvent; }

    public int ID { get; set; }

    public Alien RightNeighbour { get
        {
            Alien value = GameManager.instance.Aliens[Math.Clamp(ID + 1, 0, 3)];

            if (value == this)
                return null;
            else
                return value;
        } }

    public Alien LeftNeighbour { get
        {
            Alien value = GameManager.instance.Aliens[Math.Clamp(ID - 1, 0, 3)];

            if (value == this)
                return null;
            else
                return value;
        } }

    public void ApplyStatus(ButtonType type)
    {
        //EVENTS RESOLUTION !!!
        if (_currentEvent == null)
        {
            switch (type)
            {
                case ButtonType.HEALTH:
                    ApplyGameEvent((AlienEventType)Random.Range(0, 2));
                    break;
                case ButtonType.TASER:
                    ApplyGameEvent(AlienEventType.ANGRY);
                    break;
            }
            return;
        }

        switch (_currentEvent.Type)
        {
            case AlienEventType.HUNGRY:
                switch (type)
                {
                    case ButtonType.FOOD:
                        ResolveStatus();
                        break;
                    case ButtonType.FUN:
                        ResolveStatus();
                        ApplyGameEvent(AlienEventType.DISTRACTED);
                        break;
                    case ButtonType.HEALTH:
                        ResolveStatus();
                        switch (Random.Range(0, 4))
                        {
                            case 0:
                                ApplyGameEvent(AlienEventType.BORED);
                                break;
                            case 1:
                                ApplyGameEvent(AlienEventType.ANGRY);
                                break;
                            case 2:
                                ApplyGameEvent(AlienEventType.SICK);
                                break;
                        }
                        break;
                }
                break;
            case AlienEventType.BORED:

                switch (type)
                {
                    case ButtonType.FOOD:
                        ResolveStatus();
                        if (Random.Range(0, 2) == 0 || RightNeighbour == null)
                        {
                            LeftNeighbour.ClearStatus();
                            LeftNeighbour.ApplyGameEvent(AlienEventType.ANGRY);
                        }
                        else
                        {
                            RightNeighbour.ClearStatus();
                            RightNeighbour.ApplyGameEvent(AlienEventType.ANGRY);
                        }
                        break;
                    case ButtonType.FUN:
                        ResolveStatus();
                        break;
                    case ButtonType.HEALTH:
                        ResolveStatus();
                        switch (Random.Range(0, 4))
                        {
                            case 0:
                                ApplyGameEvent(AlienEventType.HUNGRY);
                                break;
                            case 1:
                                ApplyGameEvent(AlienEventType.ANGRY);
                                break;
                            case 2:
                                ApplyGameEvent(AlienEventType.SICK);
                                break;
                        }
                        break;
                }

                break;
            case AlienEventType.SICK:
                switch (type)
                {
                    case ButtonType.FOOD:
                        ResolveStatus();
                        int a, b;
                        do
                        {
                            a = Random.Range(0, 4);
                            b = Random.Range(0, 4);
                        } while (a == b || a == ID || b == ID);
                        GameManager.instance.Aliens[a].ApplyGameEvent(AlienEventType.SICK);
                        GameManager.instance.Aliens[b].ApplyGameEvent(AlienEventType.SICK);
                        break;
                    case ButtonType.HEALTH:
                            ResolveStatus();
                        break;
                }
                break;
            case AlienEventType.ANGRY:

                switch (type)
                {
                    case ButtonType.HEALTH:
                        ResolveStatus();
                        switch (Random.Range(0, 4))
                        {
                            case 0:
                                ApplyGameEvent(AlienEventType.HUNGRY);
                                break;
                            case 1:
                                ApplyGameEvent(AlienEventType.BORED);
                                break;
                            case 2:
                                ApplyGameEvent(AlienEventType.SICK);
                                break;
                        }
                        break;
                    case ButtonType.TASER:
                        ResolveStatus();
                        break;
                }

                break;

            case AlienEventType.FIGHTING:
                if (type == ButtonType.TASER)
                {
                    GameManager.instance.Aliens[_currentEvent.AlienInEvent[0]].ResolveStatus();
                    GameManager.instance.Aliens[_currentEvent.AlienInEvent[1]].ResolveStatus();
                } else if(type == ButtonType.HEALTH)
                {
                    GameManager.instance.Aliens[_currentEvent.AlienInEvent[0]].ResolveStatus();
                    GameManager.instance.Aliens[_currentEvent.AlienInEvent[1]].ResolveStatus();
                    switch (Random.Range(0, 4))
                    {
                        case 0:
                            GameManager.instance.Aliens[_currentEvent.AlienInEvent[0]].ApplyGameEvent(AlienEventType.HUNGRY);
                            GameManager.instance.Aliens[_currentEvent.AlienInEvent[1]].ApplyGameEvent(AlienEventType.HUNGRY);
                            break;
                        case 1:
                            GameManager.instance.Aliens[_currentEvent.AlienInEvent[0]].ApplyGameEvent(AlienEventType.SICK);
                            GameManager.instance.Aliens[_currentEvent.AlienInEvent[1]].ApplyGameEvent(AlienEventType.SICK);
                            break;
                        case 2:
                            GameManager.instance.Aliens[_currentEvent.AlienInEvent[0]].ApplyGameEvent(AlienEventType.BORED);
                            GameManager.instance.Aliens[_currentEvent.AlienInEvent[1]].ApplyGameEvent(AlienEventType.BORED);
                            break;
                    }
                }
                break;
        }


    }

    private void Update()
    {
        if (_currentEvent == null)
            return;

        _currentEvent.Duration -= Time.deltaTime;

        if(_currentEvent.Duration <= 0)
        {
            switch (_currentEvent.Type)
            {
                case AlienEventType.BORED:
                    ResolveStatus();
                    if (Random.Range(0, 2) == 0 || RightNeighbour == null)
                    {
                        ApplyGameEvent(AlienEventType.FIGHTING, ID, LeftNeighbour.ID);
                        LeftNeighbour.ApplyGameEvent(AlienEventType.FIGHTING);
                    }
                    else
                    {
                        ApplyGameEvent(AlienEventType.FIGHTING, ID, RightNeighbour.ID);
                        RightNeighbour.ApplyGameEvent(AlienEventType.FIGHTING, ID, RightNeighbour.ID);
                    }
                    break;
                case AlienEventType.SICK:
                    int a = 0;
                    do
                    {
                        a = Random.Range(0, 4);
                    } while (a == ID);
                    GameManager.instance.Aliens[a].ApplyGameEvent(AlienEventType.SICK);
                    break;
                case AlienEventType.DISTRACTED:
                    if(Random.Range(0, 2) == 1)
                    {
                        ResolveStatus();
                        ApplyGameEvent(AlienEventType.SICK);
                    }
                    break;
                case AlienEventType.ANGRY:
                    break;
            }
        }
    }

    public void ClearStatus()
    {
        _currentEvent = null;
    }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    internal void ApplyGameEvent()
    {
        switch(Random.Range(0, 4))
        {
            case 0:
                _currentEvent =
                    new AlienEvent(AlienEventType.HUNGRY, ID);
                break;
            case 1:
                _currentEvent = new AlienEvent(AlienEventType.BORED, ID);
                break;
            case 2 when ID != 0:
                _currentEvent = new AlienEvent(AlienEventType.SICK, ID);
                break;
            case 3:
                _currentEvent = new AlienEvent(AlienEventType.ANGRY, ID);
                break;

        }

        PlayAlienAnimation();
    }

    internal void ApplyGameEvent(AlienEventType type)
    {
        _currentEvent = new AlienEvent(type, ID);
        PlayAlienAnimation();
    }

    internal void ApplyGameEvent(AlienEventType type, params int[] id)
    {
        _currentEvent = new AlienEvent(type, id);
        PlayAlienAnimation();
    }

    private void PlayAlienAnimation()
    {
        if(_currentEvent == null)
        {
            _animator.Play(_animName + "IdleAnim");
            return;
        }

        switch (_currentEvent.Type)
        {
            case AlienEventType.HUNGRY:
                _animator.Play(_animName + "HungryAnim");
                break;
            case AlienEventType.BORED:
                _animator.Play(_animName + "BoredAnim");
                break;
            case AlienEventType.ANGRY:
            case AlienEventType.FIGHTING:
                _animator.Play(_animName + "AngryAnim");
                break;
            case AlienEventType.SICK:
                _animator.Play(_animName + "SickAnim");
                break;
            default:
                _animator.Play(_animName + "IdleAnim");
                break;
        }
    }

    private void ResolveStatus()
    {
        _currentEvent = null;
        PlayAlienAnimation();
    }
}
