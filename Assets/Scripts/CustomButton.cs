using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    [SerializeField] private float _cooldown;
    [SerializeField] ButtonType _type;

    private float _currentCooldown = 0f;
    private Image _image, _childIcon;
    private bool _isActive;

    public bool IsActive { get => _isActive; }

    private void Awake()
    {
        _image = GetComponent<Image>();
        _childIcon = transform.GetChild(0).GetComponent<Image>();
    }

    private void Start()
    {
        _isActive = true;

        switch (_type)
        {
            case ButtonType.FOOD:
                _image.color = Color.yellow;
                break;
            case ButtonType.FUN:
                _image.color = Color.blue;
                break;
            case ButtonType.HEALTH:
                _image.color = Color.green;
                break;
            case ButtonType.TASER:
                _image.color = Color.red;
                break;
        }
    }

    private void Update()
    {
        if (!_isActive)
        {
            _currentCooldown -= Time.deltaTime;
            if (_currentCooldown <= 0)
            {
                _isActive = true;
                _childIcon.transform.localPosition = new Vector3(0, 38f, 0);
                _image.sprite = Resources.Load<Sprite>("Buttons/B1");
                
                DOTween.Kill(_image);
                switch (_type)
                {
                    case ButtonType.FOOD:
                        _image.DOColor(Color.yellow, 1f);
                        break;
                    case ButtonType.FUN:
                        _image.DOColor(Color.blue, 1f);
                        break;
                    case ButtonType.HEALTH:
                        _image.DOColor(Color.green, 1f);
                        break;
                    case ButtonType.TASER:
                        _image.DOColor(Color.red, 1f);
                        break;
                }
            }
        }
    }

    public void Use(float xPos)
    {
        if (!_isActive)
            return;

        DOTween.Kill(_image);
        _image.DOColor(Color.grey, 0.2f);
        _childIcon.transform.localPosition = Vector3.zero;
        _image.sprite = Resources.Load<Sprite>("Buttons/B2");
        _currentCooldown = _cooldown;
        _isActive = false;

        switch (_type)
        {
            case ButtonType.FOOD:
                StartCoroutine(GameManager.instance.Food.PlayAnimation(xPos));
                break;
            case ButtonType.FUN:
                break;
            case ButtonType.HEALTH:
                StartCoroutine(GameManager.instance.Medecine.PlayAnimation(xPos));
                break;
            case ButtonType.TASER:
                StartCoroutine(GameManager.instance.Light.PlayAnimation(xPos));
                break;
        }
    }
}
