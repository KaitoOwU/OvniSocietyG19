using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour
{
    [SerializeField] Transform _woofer1, _woofer2, _woofer3, _woofer4;
    [SerializeField] List<AudioClip> _musics = new();

    public IEnumerator PlayAnimation(float posX)
    {
        transform.position = new Vector3(posX, 6, 1);
        GetComponent<AudioSource>().clip = _musics[Random.Range(0, _musics.Count)];
        Camera.main.GetComponent<AudioSource>().DOFade(0f, 0.5f);
        GetComponent<AudioSource>().Play();
        GetComponent<AudioSource>().DOFade(0.1f, 0.5f);
        GetComponent<AudioSource>().time = 30f;

        yield return transform.DOMoveY(-2.5f, 0.5f).SetEase(Ease.OutBounce).WaitForCompletion();

        for(int i = 0; i < 3; i++)
        {
            _woofer1.DOScale(0.8f, 0f);
            _woofer2.DOScale(0.8f, 0f);
            _woofer3.DOScale(0.8f, 0f);
            _woofer4.DOScale(0.8f, 0f);

            _woofer1.DOScale(1f, 1f);
            _woofer2.DOScale(1f, 1f);
            _woofer3.DOScale(1f, 1f);
            yield return _woofer4.DOScale(1f, 1f).WaitForCompletion();
        }

        GetComponent<AudioSource>().DOFade(0, 0.5f).OnComplete(() => GetComponent<AudioSource>().Stop());
        Camera.main.GetComponent<AudioSource>().DOFade(0.05f, 0.5f);
        yield return transform.DOMoveY(6f, 1f).WaitForCompletion();
        CustomButton.isAnimationAlreadyPlaying = false;
    }
}
