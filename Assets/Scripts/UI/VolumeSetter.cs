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

    // ��ü ���� ����(�����̴��� ����)
    public void OnMasterVolumeChanged()
    {   
        AudioManager.Instance.SetMasterVolume(masterVolumeSlider.value);
        bgmVolumeSlider.value = masterVolumeSlider.value;
        sfxVolumeSlider.value = masterVolumeSlider.value;
    }

    // BGM ���� ����(�����̴��� ����)
    public void OnBgmVolumeChanged()
    {
        AudioManager.Instance.SetMasterVolume(bgmVolumeSlider.value);
    }

    // SFX ���� ����(�����̴��� ����)
    public void OnSfxVolumeChanged()
    {
        AudioManager.Instance.SetSFXVolume(sfxVolumeSlider.value);
    }

    // ������ ��ư Ŭ�� �̺�Ʈ
    public void OnQuitButtonClicked()
    {
        // �޴�â���� �ǵ��ư���
        UIManager.Instance.CloseUI(this.gameObject);
        UIManager.Instance.OpenUI(menuUIGo);
    }
}
