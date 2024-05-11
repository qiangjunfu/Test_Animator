using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : MonoSingleTon<AudioManager>, IManager
{
    [SerializeField] AudioSource musicSource; // �������ֵ���Դ
    [SerializeField] int poolSize = 10; // ��ʼ����ش�С
    private List<AudioSource> audioSourcePool3D = new List<AudioSource>();
    private List<AudioSource> audioSourcePool2D = new List<AudioSource>();
    private Dictionary<string, Queue<AudioClip>> audioClipPool = new Dictionary<string, Queue<AudioClip>>();
    List<string> commonClipList = new List<string>();


    public string gunAudioPath = "Audios/AssaultRifleSingleShot";


    public void Init()
    {
        for (int i = 0; i < poolSize; i++)
        {
            CreateAudioSource(audioSourcePool3D, true);
            CreateAudioSource(audioSourcePool2D, false);
        }

        // Ԥ���س�����Ч 
        commonClipList.Add(gunAudioPath);
        foreach (var clipName in commonClipList)
        {
            StartCoroutine(LoadAudioClipsAsync(clipName, poolSize));
        }
    }



    public void Play3DSound(string clipName, Vector3 position, float volume = 1.0f, float minDistance = 1.0f, float maxDistance = 500.0f)
    {
        AudioClip clip = GetAudioClipFromPool(clipName);
        if (clip == null)
        {
            Debug.LogWarning("Audio clip pool is exhausted for: " + clipName);
            StartCoroutine(LoadAudioClipsAsync(clipName, 10));  // ��̬���ض������Ƶ����
            return;
        }
        AudioSource source = GetPooledAudioSource(audioSourcePool3D);
        if (source == null)
        {
            Debug.LogWarning("3D audio pool is exhausted!");
            source = CreateAudioSource(audioSourcePool3D, true);
        }
        source.clip = clip;
        source.volume = volume;
        source.minDistance = minDistance;
        source.maxDistance = maxDistance;
        source.transform.position = position;
        source.Play();

        StartCoroutine(RecycleAudioSourceCoroutine(source, clip.length + 0.1f));  // ʹ��Э�̺���ʱ������Դ
    }
    private IEnumerator LoadAudioClipsAsync(string clipName, int count)
    {
        for (int i = 0; i < count; i++)
        {
            ResourceRequest request = Resources.LoadAsync<AudioClip>(clipName);
            yield return request;
            if (request.asset is AudioClip clip)
            {
                if (!audioClipPool.ContainsKey(clipName))
                {
                    audioClipPool[clipName] = new Queue<AudioClip>();
                }
                audioClipPool[clipName].Enqueue(clip);
            }
        }
    }
    private IEnumerator RecycleAudioSourceCoroutine(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.gameObject.SetActive(false);
        source.Stop();
        source.clip = null;
    }

    public void Play2DSound(string clipName, float volume = 1.0f)
    {
        AudioClip clip = GetAudioClipFromPool(clipName);
        if (clip == null)
        {
            Debug.LogWarning("Audio clip pool is exhausted for: " + clipName);
            StartCoroutine(LoadAudioClipsAsync(clipName, 10));  // �첽���ض������Ƶ������ȷ���������㹻��Դ
            return;
        }
        AudioSource source = GetPooledAudioSource(audioSourcePool2D);
        if (source == null)
        {
            Debug.LogWarning("2D audio pool is exhausted!");
            source = CreateAudioSource(audioSourcePool2D, false);  // ��Ҫʱ�����µ�2D��Դ
        }
        source.clip = clip;
        source.volume = volume;
        source.Play();
        StartCoroutine(RecycleAudioSourceCoroutine(source, clip.length + 0.1f));  // ʹ��Э�̺���ʱ��������Դ
    }



    private AudioSource GetPooledAudioSource(List<AudioSource> pool)
    {
        foreach (var source in pool)
        {
            if (!source.gameObject.activeInHierarchy)
            {
                source.gameObject.SetActive(true);
                return source;
            }
        }
        return null;
    }



    private AudioClip GetAudioClipFromPool(string clipName)
    {
        if (audioClipPool.TryGetValue(clipName, out Queue<AudioClip> pool) && pool.Count > 0)
        {
            return pool.Dequeue();
        }
        else
        {
            Debug.LogWarning("No available AudioClip in pool: " + clipName);

            //StartCoroutine(LoadAudioClipsAsync(clipName, 1)); // �첽����һ���������Ƶ����
            AudioClip newAudioClip = Resources.Load<AudioClip>(clipName);
            if (!audioClipPool.ContainsKey(clipName))
            {
                audioClipPool[clipName] = new Queue<AudioClip>();
            }
            audioClipPool[clipName].Enqueue(newAudioClip);

            return newAudioClip;
        }
    }




    private AudioSource CreateAudioSource(List<AudioSource> pool, bool is3D)
    {
        AudioSource source = new GameObject(is3D ? "PooledAudioSource3D" : "PooledAudioSource2D").AddComponent<AudioSource>();
        source.gameObject.SetActive(false);
        source.spatialBlend = is3D ? 1.0f : 0.0f; // 1.0f for 3D, 0.0f for 2D
        source.rolloffMode = AudioRolloffMode.Linear;
        source.transform.parent = transform;
        pool.Add(source);
        return source;
    }
}
