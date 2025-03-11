using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutSceneTrigger : MonoBehaviour
{
    private PlayableDirector pd;
    public TimelineAsset[] ta;

    private void Start()
    {
        pd = GetComponent<PlayableDirector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            pd.Play(ta[0]);
        }
    }
}
