using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    private readonly Queue<IEnumerator> _coroutineQueue = new Queue<IEnumerator>();
    private Coroutine coordinator = null;
    public void AddToQueue(IEnumerator coroutine)
    {
        Debug.Log("Adding to coroutine: " + coroutine);
        _coroutineQueue.Enqueue(coroutine);
        if (coordinator == null) coordinator = StartCoroutine(CoroutineCoordinator());
    }
 
    private IEnumerator CoroutineCoordinator()
    {
        while (_coroutineQueue.Count > 0)
        {
            yield return _coroutineQueue.Dequeue();
        }
        coordinator = null;
    }
}

