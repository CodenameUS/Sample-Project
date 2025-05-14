using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider bgmVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Button quitButton;
    [SerializeField] private GameObject menuUIGo;

    private void Start()
    {
        OnMasterVolumeChanged();
        OnBgmVolumeChanged();
        OnSfxVolumeChanged();
    }

    // 전체 볼륨 조절(슬라이더로 조절)
    public void OnMasterVolumeChanged()
    {   
        AudioManager.Instance.SetMasterVolume(masterVolumeSlider.value);
        bgmVolumeSlider.value = masterVolumeSlider.value;
        sfxVolumeSlider.value = masterVolumeSlider.value;
    }

    // BGM 볼륨 조절(슬라이더로 조절)
    public void OnBgmVolumeChanged()
    {
        AudioManager.Instance.SetMasterVolume(bgmVolumeSlider.value);
    }

    // SFX 볼륨 조절(슬라이더로 조절)
    public void OnSfxVolumeChanged()
    {
        AudioManager.Instance.SetSFXVolume(sfxVolumeSlider.value);
    }

    // 나가기 버튼 클릭 이벤트
    public void OnQuitButtonClicked()
    {
        // 메뉴창으로 되돌아가기
        UIManager.Instance.CloseUI(this.gameObject);
        UIManager.Instance.OpenUI(menuUIGo);
    }
}
