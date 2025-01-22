using UnityEngine;
using System.Collections;

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

        _doorAnimator.SetTrigger("open");

        _closeCoroutine = StartCoroutine(AutoCloseDoor());
    }

    private IEnumerator AutoCloseDoor()
    {
        yield return new WaitForSeconds(_autoCloseDelay);

        _doorAnimator.SetTrigger("close");
        _closeCoroutine = null;
    }

}