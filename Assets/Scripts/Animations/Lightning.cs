using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    public IEnumerator PlayAnimation(float xPos)
    {
        GetComponent<AudioSource>().Play();
        SpriteRenderer _sprite = GetComponent<SpriteRenderer>();
        transform.position = new(xPos, 0, -1);

        _sprite.DOColor(new(1, 1, 1, 1), 0f);

        GetComponent<AudioSource>().DOFade(0f, 1f).OnComplete(() => GetComponent<AudioSource>().Stop());
        yield return _sprite.DOColor(new(1, 1, 1, 0), 1f).WaitForCompletion();
        CustomButton.isAnimationAlreadyPlaying = false;
    }

}
