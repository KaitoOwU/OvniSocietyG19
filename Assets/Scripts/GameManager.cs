using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private GameInputs _gameInputs;
    
    [SerializeField] Alien[] _aliens;
    [SerializeField] CustomButton[] _customButtons;
    [SerializeField] float _eventAppearDelayInSeconds;
    [SerializeField] float _appreciationDecreaseOverTimeSpeed;
    [SerializeField] Image _greyHeart, _redHeart, _heartFace;

    private float _currentEventDelay;
    private float _appreciation;

    public Alien[] Aliens { get => _aliens; }
    
    public int ActiveEventsAmount { get
        {
            int amount = 0;
            foreach(var aliens in _aliens)
            {
                amount += aliens.CurrentEvents.Count;
            }
            return amount;
        } }

    private void OnValidate()
    {
        if(_aliens.Length != 4)
        {
            _aliens = new Alien[4];
        }

        if(_customButtons.Length != 4)
        {
            _customButtons = new CustomButton[4];
        }
    }

    private void Awake()
    {
        instance = this;
        _gameInputs = new();
    }

    #region INPUTS
    private void OnEnable()
    {
        _gameInputs.Inputs.Alien1Fun.started += (ctx) => ActivateButton(0, ButtonType.FUN);
        _gameInputs.Inputs.Alien1Hunger.started += (ctx) => ActivateButton(0, ButtonType.FOOD);
        _gameInputs.Inputs.Alien1Taser.started += (ctx) => ActivateButton(0, ButtonType.TASER);

        _gameInputs.Inputs.Alien2Fun.started += (ctx) => ActivateButton(1, ButtonType.FUN);
        _gameInputs.Inputs.Alien2Health.started += (ctx) => ActivateButton(1, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien2Taser.started += (ctx) => ActivateButton(1, ButtonType.TASER);

        _gameInputs.Inputs.Alien3Fun.started += (ctx) => ActivateButton(2, ButtonType.FUN);
        _gameInputs.Inputs.Alien3Health.started += (ctx) => ActivateButton(2, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien3Hunger.started += (ctx) => ActivateButton(2, ButtonType.FOOD);

        _gameInputs.Inputs.Alien4Health.started += (ctx) => ActivateButton(3, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien4Hunger.started += (ctx) => ActivateButton(3, ButtonType.FOOD);
        _gameInputs.Inputs.Alien4Taser.started += (ctx) => ActivateButton(3, ButtonType.TASER);

        _gameInputs.Enable();
    }

    private void OnDisable()
    {
        _gameInputs.Inputs.Alien1Fun.started -= (ctx) => ActivateButton(0, ButtonType.FUN);
        _gameInputs.Inputs.Alien1Hunger.started -= (ctx) => ActivateButton(0, ButtonType.FOOD);
        _gameInputs.Inputs.Alien1Taser.started -= (ctx) => ActivateButton(0, ButtonType.TASER);

        _gameInputs.Inputs.Alien2Fun.started -= (ctx) => ActivateButton(1, ButtonType.FUN);
        _gameInputs.Inputs.Alien2Health.started -= (ctx) => ActivateButton(1, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien2Taser.started -= (ctx) => ActivateButton(1, ButtonType.TASER);

        _gameInputs.Inputs.Alien3Fun.started -= (ctx) => ActivateButton(2, ButtonType.FUN);
        _gameInputs.Inputs.Alien3Health.started -= (ctx) => ActivateButton(2, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien3Hunger.started -= (ctx) => ActivateButton(2, ButtonType.FOOD);

        _gameInputs.Inputs.Alien4Health.started -= (ctx) => ActivateButton(3, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien4Hunger.started -= (ctx) => ActivateButton(3, ButtonType.FOOD);
        _gameInputs.Inputs.Alien4Taser.started -= (ctx) => ActivateButton(3, ButtonType.TASER);

        _gameInputs.Disable();
    }
    #endregion

    private void Update()
    {
        _appreciation -= Time.deltaTime * ActiveEventsAmount * _appreciationDecreaseOverTimeSpeed;
        _redHeart.fillAmount = _appreciation / 100f;

        _currentEventDelay -= Time.deltaTime;
        if(_currentEventDelay <= 0)
        {
            _aliens[Random.Range(0, 4)].ApplyGameEvent();
            _currentEventDelay = _eventAppearDelayInSeconds;
        }
    }

    private void Start()
    {
        _appreciation = 100f;
        StartCoroutine(HeartBeat());
    }

    private void ActivateButton(int alienId, ButtonType type)
    {
        if (_customButtons[(int)type].IsActive)
        {
            _customButtons[(int)type].Use();
            _aliens[alienId].ApplyStatus(type);
        }
    }

    private IEnumerator HeartBeat()
    {
        do
        {
            switch (_appreciation)
            {
                case < 33:
                    _heartFace.sprite = Resources.Load<Sprite>("Visages/SickFace");
                    break;
                case < 67:
                    _heartFace.sprite = Resources.Load<Sprite>("Visages/NeutralFace");
                    break;
            }

            float _beatSpeed = (_appreciation / 77f) + 0.1f;

            DOTween.Kill(_greyHeart.transform);

            yield return _greyHeart.transform.DOScale(1f, 0.1f).OnComplete(() => { _greyHeart.transform.DOScale(1.3f, 0.5f); }).WaitForCompletion();
            yield return new WaitForSecondsRealtime(_beatSpeed);
        } while (_appreciation > 0);

        _heartFace.sprite = Resources.Load<Sprite>("Visages/DeadFace");
    }
}

public enum AlienEventType
{
    HUNGRY,
    BORED,
    SICK,
    ANGRY,
    FIGHTING,
    DISTRACTED
}

public class AlienEvent
{
    public AlienEventType Type { get; private set; }
    public int[] AlienInEvent { get; private set; }
    public float Duration { get; set; }

    public AlienEvent(AlienEventType type, params int[] alienInEvent)
    {
        Type = type;
        AlienInEvent = alienInEvent;
        Duration = 10f;
    }

}

public enum ButtonType
{
    FOOD,
    FUN,
    HEALTH,
    TASER
}
