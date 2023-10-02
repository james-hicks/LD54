using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OffMyceliumFade : MonoBehaviour
{
    [SerializeField] private float _timeOffLand = 2f;
    [SerializeField] private Animator _fadeAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Exiting Safe Zone");
            StartCoroutine(FadeAway());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Entering Safe Zone");
            StopAllCoroutines();
            _fadeAnimator.SetBool("Fade", false);
        }
    }

    private IEnumerator FadeAway()
    {
        _fadeAnimator.SetBool("Fade", true);
        yield return new WaitForSeconds(_timeOffLand);
        PlayerMovement.PlayerInstance.HP = 0;
        _fadeAnimator.SetBool("Fade", false);
    }
}
