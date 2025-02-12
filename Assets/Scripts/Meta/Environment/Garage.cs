using System.Collections;
using UnityEngine;

public class Garage : MonoBehaviour
{
    [SerializeField] private Animator _doorAnimator;
    [SerializeField] private float _autoCloseDelay = 5f;
    private Coroutine _closeCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        if (_closeCoroutine != null)
        {
            StopCoroutine(_closeCoroutine);
        }

        _doorAnimator.SetBool("open", true);
        _closeCoroutine = StartCoroutine(AutoCloseDoor());
    }

    private IEnumerator AutoCloseDoor()
    {
        yield return new WaitForSeconds(_autoCloseDelay);
        _doorAnimator.SetBool("open", false);
        _closeCoroutine = null;
    }
}