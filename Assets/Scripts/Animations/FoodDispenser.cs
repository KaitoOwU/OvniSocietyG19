using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodDispenser : MonoBehaviour
{
    [SerializeField] private Transform _banana;

    public IEnumerator PlayAnimation(float xPos)
    {
        transform.position = new(xPos, 6f, 0);
        _banana.transform.localPosition = new(0, 0, 1);
        _banana.transform.localRotation = Quaternion.identity;
        _banana.gameObject.SetActive(true);

        yield return transform.DOMoveY(3.5f, 1f).WaitForCompletion();

        _banana.DOLocalMoveY(-5.2f, 1f);
        yield return _banana.DORotate(new(0, 0, 360), 1f).WaitForCompletion();

        _banana.gameObject.SetActive(false);
        yield return transform.DOMoveY(6f, 1f).WaitForCompletion();
        CustomButton.isAnimationAlreadyPlaying = false;
    }

}
