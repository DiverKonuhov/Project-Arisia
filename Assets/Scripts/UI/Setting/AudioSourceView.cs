using UnityEngine;
//---------------------------------[ ÑÈÃÌÎ ÊÎÄÈÍÃ ]---------------------------------
namespace Menu.Sound {
    public enum AudioSourceType {
        MusicMenu,
        Audio
    }

    [RequireComponent(typeof(AudioSource))]
    public class AudioSourceView : MonoBehaviour {
        [SerializeField]
        private AudioSourceType _audioSourceType;

        public AudioSourceType m_audioSourceType {
            get => _audioSourceType;
            set => _audioSourceType = value;
        }

        private AudioSource _audioSource;

        public static AudioSourceView s_instance;

        private void Awake() {
            _audioSource = GetComponent<AudioSource>();

            s_instance = this;

            if (_audioSourceType == AudioSourceType.MusicMenu)
                _audioSource.volume = PlayerPrefs.GetFloat("SliderVolumeValueMusic");
            else
                _audioSource.volume = PlayerPrefs.GetFloat("SliderVolumeValue");
        }
        private void Update()
        {
            if (_audioSourceType == AudioSourceType.MusicMenu)
                _audioSource.volume = PlayerPrefs.GetFloat("SliderVolumeValueMusic");
            else
                _audioSource.volume = PlayerPrefs.GetFloat("SliderVolumeValue");
        }
        public void SetVolume(float value) {
            _audioSource.volume = value;
        }
    }

}