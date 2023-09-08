using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Clignotement : MonoBehaviour
{
    private Image _texteClignotement;
    [SerializeField] private float _tempsClignotement = 0.7f;

    private void Start()
    {
        _texteClignotement = GetComponent<Image>();
        StartCoroutine(_clignotement());
    }
    IEnumerator _clignotement()
    {
        while (true)
        {
            _texteClignotement.enabled = !_texteClignotement.enabled;
            yield return new WaitForSeconds(_tempsClignotement);
        }
    }
}   
