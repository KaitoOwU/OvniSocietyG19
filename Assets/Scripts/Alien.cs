using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Math = System.Math;

public class Alien : MonoBehaviour
{
    [SerializeField] private string _animName;
    [SerializeField] SpriteRenderer _dangerSpotlight;

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

    private IEnumerator TaseAlien(AlienEventType? statusAfter, float point = 0f)
    {
        _animator.Play(_animName + "ElectricityAnim");
        yield return new WaitForSecondsRealtime(1f);
        if (statusAfter != null)
            ApplyGameEvent((AlienEventType)statusAfter);
        else
            ResolveStatus(point);
    }

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
                    StartCoroutine(TaseAlien(AlienEventType.ANGRY));
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
                        ResolveStatus(10f);
                        break;
                    case ButtonType.FUN:
                        ResolveStatus(5f);
                        ApplyGameEvent(AlienEventType.DISTRACTED);
                        break;
                    case ButtonType.HEALTH:
                        ResolveStatus(0f);
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
                        ResolveStatus(0f);
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
                        ResolveStatus(10f);
                        break;
                    case ButtonType.HEALTH:
                        ResolveStatus(0f);
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
                        ResolveStatus(0f);
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
                            ResolveStatus(10f);
                        break;
                }
                break;
            case AlienEventType.ANGRY:

                switch (type)
                {
                    case ButtonType.HEALTH:
                        ResolveStatus(0);
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
                        StartCoroutine(TaseAlien(null, 10f));
                        break;
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
                    ResolveStatus(0);
                    if (Random.Range(0, 2) == 0 || RightNeighbour == null)
                    {
                        ApplyGameEvent(AlienEventType.ANGRY, ID, LeftNeighbour.ID);
                        LeftNeighbour.ApplyGameEvent(AlienEventType.ANGRY);
                    }
                    else
                    {
                        ApplyGameEvent(AlienEventType.ANGRY, ID, RightNeighbour.ID);
                        RightNeighbour.ApplyGameEvent(AlienEventType.ANGRY, ID, RightNeighbour.ID);
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
                        ResolveStatus(0);
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
        AnimateSpotlight();
        switch(Random.Range(0, 3))
        {
            case 0:
                _currentEvent =
                    new AlienEvent(AlienEventType.HUNGRY, 10f, ID);
                break;
            case 1:
                _currentEvent = new AlienEvent(AlienEventType.BORED, 10f, ID);
                break;
            case 2 when ID != 0:
                _currentEvent = new AlienEvent(AlienEventType.SICK, 10f, ID);
                break;

        }

        PlayAlienAnimation();
    }

    internal void ApplyGameEvent(AlienEventType type)
    {
        _currentEvent = new AlienEvent(type, 10f, ID);
        PlayAlienAnimation();
    }

    internal void ApplyGameEvent(AlienEventType type, params int[] id)
    {
        _currentEvent = new AlienEvent(type, 10f, id);
        PlayAlienAnimation();
    }

    internal void ApplyGameEvent(AlienEventType type, float duration, params int[] id)
    {
        _currentEvent = new AlienEvent(type, duration, id);
        PlayAlienAnimation();
    }

    private void AnimateSpotlight()
    {
        _dangerSpotlight.GetComponent<AudioSource>().Play();
        _dangerSpotlight.DOColor(new (1, 1, 1, 1), 0.5f).OnComplete(() =>
        {
            _dangerSpotlight.DOColor(new(1, 1, 1, 0.5f), 0.5f).OnComplete(() =>
            {
                _dangerSpotlight.DOColor(new(1, 1, 1, 1), 0.5f).OnComplete(() =>
                {
                    _dangerSpotlight.DOColor(new(1, 1, 1, 0), 1f);
                });
            });
        });
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
                AnimateSpotlight();
                _animator.Play(_animName + "HungryAnim");
                break;
            case AlienEventType.BORED:
                AnimateSpotlight();
                _animator.Play(_animName + "BoredAnim");
                break;
            case AlienEventType.ANGRY:
                AnimateSpotlight();
                _animator.Play(_animName + "AngryAnim");
                break;
            case AlienEventType.SICK:
                AnimateSpotlight();
                _animator.Play(_animName + "SickAnim");
                break;
            default:
                _animator.Play(_animName + "IdleAnim");
                break;
        }
    }

    private void ResolveStatus(float appreciationAcquired)
    {
        _currentEvent = null;
        PlayAlienAnimation();

        Mathf.Clamp(appreciationAcquired, 0f, 100f);
        if (appreciationAcquired > 0f)
        {
            GameManager.instance.PlayFunnySound();
            GameManager.instance.HeartAnimation();
        }
    }
}
