using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private List<AudioClip> background_clips;
    [SerializeField] private AudioClip lvl_up_clip;
    [SerializeField] private AudioSource audioSource;

    [Space]
    [SerializeField] private float volume = 0.5f;

    void Start()
    {
        // Проверяем, есть ли компонент AudioSource, если нет, добавляем
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.loop = false; // Музыка не зацикливается, чтобы можно было переключать треки
        audioSource.volume = volume;
        PlayBackground();
    }

    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayBackground();
        }
    }

    public void PlayLvlUp()
    {
        audioSource.clip = lvl_up_clip;
        audioSource.Play();
    }

    private void PlayBackground()
    {
        if (background_clips.Count == 0) return; // Если плейлист пустой, ничего не делаем

        // Устанавливаем следующий трек
        audioSource.clip = background_clips[Random.Range(0, background_clips.Count-1)];
        audioSource.Play();
    }
}
