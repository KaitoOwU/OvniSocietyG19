using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MedecineDispenser : MonoBehaviour
{
    [SerializeField] private Transform _medecine;

    public IEnumerator PlayAnimation(float xPos)
    {
        transform.position = new(xPos, 6.45f, 0);
        _medecine.transform.localPosition = new(0, 0, 1);
        _medecine.transform.localRotation = Quaternion.identity;
        _medecine.gameObject.SetActive(true);

        yield return transform.DOMoveY(2.6f, 1f).WaitForCompletion();

        _medecine.DOLocalMoveY(-4.15f, 1f);
        yield return _medecine.DORotate(new(0, 0, 360), 1f).WaitForCompletion();

        _medecine.gameObject.SetActive(false);
        yield return transform.DOMoveY(6.45f, 1f).WaitForCompletion();
        CustomButton.isAnimationAlreadyPlaying = false;
    }
}
