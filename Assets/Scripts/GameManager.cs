using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private GameInputs _gameInputs;
    
    [SerializeField] Alien[] _aliens;
    [SerializeField] CustomButton[] _customButtons;
    [SerializeField] float _eventAppearDelayInSeconds;

    private float _currentEventDelay;


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
        _gameInputs = new();
    }

    #region INPUTS
    private void OnEnable()
    {
        _gameInputs.Inputs.Alien1Fun.started += (ctx) => ActivateButton(0, ButtonType.SANITY);
        _gameInputs.Inputs.Alien1Hunger.started += (ctx) => ActivateButton(0, ButtonType.FOOD);
        _gameInputs.Inputs.Alien1Taser.started += (ctx) => ActivateButton(0, ButtonType.TASER);

        _gameInputs.Inputs.Alien2Fun.started += (ctx) => ActivateButton(1, ButtonType.SANITY);
        _gameInputs.Inputs.Alien2Health.started += (ctx) => ActivateButton(1, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien2Taser.started += (ctx) => ActivateButton(1, ButtonType.TASER);

        _gameInputs.Inputs.Alien3Fun.started += (ctx) => ActivateButton(2, ButtonType.SANITY);
        _gameInputs.Inputs.Alien3Health.started += (ctx) => ActivateButton(2, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien3Hunger.started += (ctx) => ActivateButton(2, ButtonType.FOOD);

        _gameInputs.Inputs.Alien4Health.started += (ctx) => ActivateButton(3, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien4Hunger.started += (ctx) => ActivateButton(3, ButtonType.FOOD);
        _gameInputs.Inputs.Alien4Taser.started += (ctx) => ActivateButton(3, ButtonType.TASER);

        _gameInputs.Enable();
    }

    private void OnDisable()
    {
        _gameInputs.Inputs.Alien1Fun.started -= (ctx) => ActivateButton(0, ButtonType.SANITY);
        _gameInputs.Inputs.Alien1Hunger.started -= (ctx) => ActivateButton(0, ButtonType.FOOD);
        _gameInputs.Inputs.Alien1Taser.started -= (ctx) => ActivateButton(0, ButtonType.TASER);

        _gameInputs.Inputs.Alien2Fun.started -= (ctx) => ActivateButton(1, ButtonType.SANITY);
        _gameInputs.Inputs.Alien2Health.started -= (ctx) => ActivateButton(1, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien2Taser.started -= (ctx) => ActivateButton(1, ButtonType.TASER);

        _gameInputs.Inputs.Alien3Fun.started -= (ctx) => ActivateButton(2, ButtonType.SANITY);
        _gameInputs.Inputs.Alien3Health.started -= (ctx) => ActivateButton(2, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien3Hunger.started -= (ctx) => ActivateButton(2, ButtonType.FOOD);

        _gameInputs.Inputs.Alien4Health.started -= (ctx) => ActivateButton(3, ButtonType.HEALTH);
        _gameInputs.Inputs.Alien4Hunger.started -= (ctx) => ActivateButton(3, ButtonType.FOOD);
        _gameInputs.Inputs.Alien4Taser.started -= (ctx) => ActivateButton(3, ButtonType.TASER);

        _gameInputs.Disable();
    }
    #endregion

    #region GAME_EVENTS
    private void Update()
    {
        _currentEventDelay -= Time.deltaTime;
        if(_currentEventDelay <= 0)
        {
            _aliens[Random.Range(0, 4)].ApplyGameEvent();
            _currentEventDelay = _eventAppearDelayInSeconds;
        }
    }
    #endregion

    private void ActivateButton(int alienId, ButtonType type)
    {
        _aliens[alienId].ApplyStatus(type);
        _customButtons[(int)type].Use();
    }
}

public enum AlienEventType
{
    HUNGRY,
    DEPRESSED,
    ANGRY,
    SICK
}

public class AlienEvent
{
    public AlienEventType Type { get; private set; }
    public int[] AlienInEvent { get; private set; }

    public AlienEvent(AlienEventType type, params int[] alienInEvent)
    {
        Type = type;
        AlienInEvent = alienInEvent;
    }

}

public enum ButtonType
{
    FOOD,
    SANITY,
    HEALTH,
    TASER
}
