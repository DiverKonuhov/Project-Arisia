using UnityEngine;
using UnityEngine.UI;

namespace Menu {
   // [RequireComponent(typeof(Animator))] ну блять, в игре настройки не работают
    public class SettingsMaunMenu : MonoBehaviour {
        [SerializeField]
        private AudioSource[] _audioSound;

        [SerializeField]
        private AudioSource[] _audioMusic;

        [SerializeField]
        private Slider _sliderSensitivity;

        [SerializeField]
        private Slider _sliderSound;

        [SerializeField]
        private Slider _sliderMusic;

      
        private void Awake() {
            _sliderSensitivity.onValueChanged.AddListener(SetSensitivity);
            _sliderSound.onValueChanged.AddListener(SetSound);
            _sliderMusic.onValueChanged.AddListener(SetMusic);

            _sliderSound.value = PlayerPrefs.GetFloat("SliderVolumeValue");
            _sliderMusic.value = PlayerPrefs.GetFloat("SliderVolumeValueMusic");          
        }

        private void Update()
        {
            _sliderSensitivity.onValueChanged.AddListener(SetSensitivity);
            _sliderSound.onValueChanged.AddListener(SetSound);
            _sliderMusic.onValueChanged.AddListener(SetMusic);

            _sliderSound.value = PlayerPrefs.GetFloat("SliderVolumeValue");
            _sliderMusic.value = PlayerPrefs.GetFloat("SliderVolumeValueMusic");
        }


        private void SetSensitivity(float newvalue)
        {
            PlayerPrefs.SetFloat("Sensitivity", newvalue);
        }

        private void SetSound(float newvalue)
        {
            PlayerPrefs.SetFloat("SliderVolumeValue", newvalue);
            for(int i = 0; i < _audioSound.Length; i++) {
                _audioSound[i].volume = newvalue;
            }
        }

        private void SetMusic(float newvalue)
        {
            PlayerPrefs.SetFloat("SliderVolumeValueMusic", newvalue);
            for (int i = 0; i < _audioMusic.Length; i++) {
                _audioMusic[i].volume = newvalue;
            }
        }
    }

}