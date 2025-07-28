using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Cinemachine;

/*
                            << TimeLineCameraBinder >>

        - Timeline의 시네머신트랙에 카메라 바인딩
            - 미리 바인딩할 수 없는 구조에 사용
 */

public class TimeLineCameraBinder : MonoBehaviour
{
    [SerializeField] PlayableDirector timelineDirector;

    public GameObject mainCamera;

    private void Start()
    {
        BindTimelineCameraTracks();
    }
    
    private void BindTimelineCameraTracks()
    {
        // 카메라 가져오기
        mainCamera = Camera.main.gameObject;

        if (timelineDirector == null || mainCamera == null)
        {
            Debug.Log("PlayableDirector 또는 Main Camera 없음");
            return;
        }

        TimelineAsset timelineAsset = timelineDirector.playableAsset as TimelineAsset;

        // 타임라인 내 트랙 중 CinemachineTrack 을 찾아 카메라 바인딩
        foreach (var track in timelineAsset.GetOutputTracks())
        {
            if (track is CinemachineTrack)
            {
                timelineDirector.SetGenericBinding(track, mainCamera.GetComponent<CinemachineBrain>());
                Debug.Log("Cinemachine Brain 바인딩 완료");
            }
        }
    }

}
