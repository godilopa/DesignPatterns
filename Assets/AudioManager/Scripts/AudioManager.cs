///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// COPYRIGHT (C) TEACHLABS, SL - ALL RIGHTS RESERVED. UNAUTHORIZED COPYING OF THIS FILE, VIA ANY MEDIUM IS STRICTLY PROHIBITED.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AudioManager
{
  /// <summary>
  /// Clase que controla los audios del juego
  /// </summary>
  public sealed class AudioManager : Singleton<AudioManager>
  {
    /// <summary>
    /// Tipo de reproduccion de las canciones
    /// </summary>
    public enum PlayMusicMethod
    {
      ContinuousPlayThrough,
      ContinuousPlayThroughWithDelay,
      OncePlayThrough,
      OncePlayThroughWithDelay,
      ShufflePlayThrough,
      ShufflePlayThroughWithDelay,
      Manual
    }

    /// <summary>
    /// Tipo de sonidos
    /// </summary>
    public enum SoundType
    {
      Music,
      Effects
    }

    /// <summary>
    /// Tiempo de fade entre canciones
    /// </summary>
    public float FadeDuration
    {
      get
      {
        return fadeDuration;
      }

      set
      {
        fadeDuration = value;
      }
    }

    /// <summary>
    /// Delay entre canciones
    /// </summary>
    public float MusicDelay
    {
      get
      {
        return musicDelay;
      }

      set
      {
        musicDelay = value;
      }
    }

    /// <summary>
    /// Volumen efectos
    /// </summary>
    public float EffectsVolume
    {
      set
      {
        effectsVolumeMultiplier = value;

        for (int i = 0; i < elements.Count; i++)
        {
          string element = elements[i];

          if (allEffectsVolumes.ContainsKey(element) && fixedEffectsVolumes.ContainsKey(element))
            allEffectsVolumes[element] = value * fixedEffectsVolumes[element];
        }

        for (int i = 0; i < effectsActive.Count; i++)
        {
          if (fixedEffectsVolumes.ContainsKey(effectsActive[i].clip.name))
            effectsActive[i].volume = value * fixedEffectsVolumes[effectsActive[i].clip.name];
        }
      }

      get
      {
        return effectsVolumeMultiplier;
      }
    }

    /// <summary>
    /// Volumen musica
    /// </summary>
    public float MusicVolume
    {
      //@TODO:Pausar si es cero ?
      set
      {
        musicVolumeMultiplier = value;

        for (int i = 0; i < musicAudioSources.Length; i++)
        {
          if (musicAudioSources[i].clip != null)
            musicAudioSources[i].volume = value * fixedMusicVolumes[musicAudioSources[i].clip.name];
        }
      }

      get
      {
        return musicAudioSources[0].volume;
      }
    }

    /// <summary>
    /// Si no usa sceneloader carga directamente los sonidos
    /// </summary>
    public bool UseSceneLoader
    {
      get
      {
        return useSceneLoader;
      }

      set
      {
        useSceneLoader = value;
      }
    }

    [SerializeField]
    private bool useSceneLoader = true;

    private Dictionary<string, AudioClip> allEffects = new Dictionary<string, AudioClip>();

    private Dictionary<string, float> allEffectsVolumes = new Dictionary<string, float>();

    private List<AudioClip> allMusic = new List<AudioClip>();

    private Dictionary<string, float> allMusicVolumes = new Dictionary<string, float>();

    private AudioSource[] musicAudioSources = new AudioSource[2];

    private bool[] audioPaused = new bool[2];

    private Dictionary<string, float> fixedEffectsVolumes = new Dictionary<string, float>();

    private Dictionary<string, float> fixedMusicVolumes = new Dictionary<string, float>();

    //@NOTA: Para modificar los volumenes de lso efectos
    private List<string> elements;

    private AudioClip audioClipAsync;

    private const string rootName = @"Root";

    /// <summary>
    /// Bancos de sonidos, publicos para el editor
    /// </summary>
    [SerializeField, HideInInspector]
    public List<SoundBank> soundsBank = new List<SoundBank>();

#if UNITY_EDITOR
    //@NOTA: Para el editor que se queden guardado los foldout d las listas de sonido y niveles
    [System.Serializable]
    public class ShowBankSoundLevelOrSounds
    {
      public bool showBankSoundLevel;
      public bool showBankSoundSounds;

      public ShowBankSoundLevelOrSounds(bool a, bool b)
      {
        showBankSoundLevel = a;
        showBankSoundSounds = b;
      }
    }

    [HideInInspector]
    public List<ShowBankSoundLevelOrSounds> showBankSoundLevelOrSounds = new List<ShowBankSoundLevelOrSounds>();

    //@NOTA: Para el editor que se vean la info de musica y efectos reproduciendose
    public AudioSource[] MusicAudioSources
    {
      get
      {
        return musicAudioSources;
      }

      set
      {
        musicAudioSources = value;
      }
    }

    public List<AudioSource> EffectsActive
    {
      get
      {
        return effectsActive;
      }

      set
      {
        effectsActive = value;
      }
    }
#endif

    private ObjectPool audioEffectsPool;

    [SerializeField]
    private Transform[] audiosPool;

    private float fadeDuration = 1;

    private float musicDelay = 2;

    private float musicVolumeMultiplier = 1;

    private float effectsVolumeMultiplier = 1;

    private int musicIndex;

    private SoundBank lastBankLoaded;

    private int deleteLastMusicIndex;

    private int deleteLastEffectIndex;

    private PlayMusicMethod playMusicMethod = PlayMusicMethod.ContinuousPlayThroughWithDelay;

    private IEnumerator musicCoroutine;

    private List<AudioSource> effectsActive = new List<AudioSource>();

    private float duration;

    private struct EffectsPaused
    {
      public AudioSource audioSource;
      public bool paused;
    }

    private Dictionary<AudioSource, bool> effectsPaused = new Dictionary<AudioSource, bool>();

    //NOTA: Para guardar en el update momentaneamente la referencia en el bucle
    private AudioSource source;

    /// <summary>
    /// Devolver sonidos al padre pool
    /// </summary>
    public void ReturnAudiosToPool()
    {
      for (int i = 0; i < audiosPool.Length; i++)
      {
        if (audiosPool[i] != null && audioEffectsPool.transform != null)
          audiosPool[i].parent = audioEffectsPool.transform;
      }
    }

    /// <summary>
    /// Reproducir musica a partir del nombre
    /// </summary>
    public void PlayMusic(string musicName, bool loop = false)
    {
      for (int i = 0; i < allMusic.Count; i++)
      {
        if (string.Equals(musicName, allMusic[i].name) == true)
        {
          PlayMusic(allMusic[i], loop);
          return;
        }
      }
    }

    /// <summary>
    /// Reproducir musica a partir del audioclip
    /// </summary>
    public void PlayMusic(AudioClip audioClip, bool loop = false)
    {
      if (musicAudioSources[0].isPlaying == true)
      {
        musicAudioSources[1].clip = audioClip;
        musicAudioSources[1].loop = loop;
        musicAudioSources[1].volume = allMusicVolumes[audioClip.name] * musicVolumeMultiplier;
        StartCoroutine(FadeMusic(musicAudioSources[0], false, true));
        StartCoroutine(FadeMusic(musicAudioSources[1], true, false));
      }
      else if (musicAudioSources[1].isPlaying == true)
      {
        musicAudioSources[0].clip = audioClip;
        musicAudioSources[0].loop = loop;
        musicAudioSources[0].volume = allMusicVolumes[audioClip.name] * musicVolumeMultiplier;
        StartCoroutine(FadeMusic(musicAudioSources[1], false, true));
        StartCoroutine(FadeMusic(musicAudioSources[0], true, false));
      }
      else
      {
        musicAudioSources[0].clip = audioClip;
        musicAudioSources[0].loop = loop;
        musicAudioSources[0].volume = allMusicVolumes[audioClip.name] * musicVolumeMultiplier;
        StartCoroutine(FadeMusic(musicAudioSources[0], true, false));
      }
    }

    /// <summary>
    /// Reproducir efecto a partir del audioclip
    /// </summary>
    public float PlayEffect(AudioClip audioClip)
    {
      return PlayEffectExtended(audioClip);
    }

    /// <summary>
    /// Reproducir efecto a partir del audioclip con opciones extra
    /// </summary>
    public float PlayEffectExtended(AudioClip audioClip, float audioVolume = 1.0f, bool loop = false, Transform position = null, float minDist = 1.0f, float maxDist = 4.5f, bool bypassListenerEffects = false)
    {
      UnityEngine.Assertions.Assert.IsNotNull(audioClip);

      GameObject poolElement = audioEffectsPool.PullFromPool("cambiar");
      duration = 0.0f;

      if (poolElement != null)
      {
        AudioSource audioSource = poolElement.GetComponent<AudioSource>();
        effectsActive.Add(audioSource);

        //@NOTA:Desactiva los efectos aplicados al audiolistener si son voces
        if (bypassListenerEffects == true)
        {
          audioSource.bypassListenerEffects = true;
          audioSource.bypassReverbZones = true;
        }
        else
        {
          audioSource.bypassListenerEffects = false;
          audioSource.bypassReverbZones = false;
        }

        if (effectsPaused.ContainsKey(audioSource) == false)
          effectsPaused.Add(audioSource, false);

        if (position != null)
        {
          poolElement.transform.parent = position;
          poolElement.transform.localPosition = Vector3.zero;
          //Sonido 3D
          audioSource.spatialBlend = 1.0f;
          audioSource.minDistance = minDist;
          audioSource.maxDistance = maxDist;
        }
        else
          audioSource.spatialBlend = 0.0f;

        UnityEngine.Assertions.Assert.IsNotNull(audioSource, @"Los prefab de la piscina no tiene Audio Source");
        audioSource.clip = audioClip;
        audioSource.loop = loop;

        if (bypassListenerEffects == false)
          audioSource.volume = audioVolume * effectsVolumeMultiplier;
        else
          audioSource.volume = audioVolume;

        audioSource.Play();
        duration = audioClip.length;
      }
      else
        Debug.LogWarningFormat("[AudioManager.PlayEffect] No quedan elementos en la pool");

      return duration;
    }

    /// <summary>
    /// Reproducir efecto a partir del nombre en el banco
    /// </summary>
    public float PlayEffect(string audioName)
    {
      UnityEngine.Assertions.Assert.IsNotNull(audioName);

      if (allEffects.ContainsKey(audioName) == true)
        return PlayEffectExtended(allEffects[audioName], allEffectsVolumes[audioName]);
      else
        Debug.LogWarningFormat("[AudioManager.PlayEffect] No hay audio con el nombre '{0}'.", audioName);

      return 0.0f;
    }

    /// <summary>
    /// Reproducir efecto a partir del nombre en el banco con opciones extra
    /// </summary>
    public float PlayEffectExtended(string audioName, float volume = 1.0f, bool loop = false, Transform position = null, float minDist = 1.0f, float maxDist = 4.5f)
    {
      UnityEngine.Assertions.Assert.IsNotNull(audioName);

      if (allEffects.ContainsKey(audioName) == true)
        return PlayEffectExtended(allEffects[audioName], volume, loop, position, minDist, maxDist);
      else
        Debug.LogWarningFormat("[AudioManager.PlayEffectExtended] No hay audio con el nombre '{0}'.", audioName);

      return 0.0f;
    }

    /// <summary>
    /// Reproduce un audio en Resources cargandolo asincronamente.
    /// Los ficheros de audio deben estar marcado como 'Streaming'.
    /// </summary>
    public void PlayEffectFromResources(string audioPath, string audioPathAlt = @"", float volume = 1.0f, Transform position = null, bool oneInstance = true, Action endCallback = null)
    {
      UnityEngine.Assertions.Assert.IsNotNull(audioPath, @"'audioFile' null or empty");
      UnityEngine.Assertions.Assert.IsTrue(volume >= 0.0f && volume <= 1.0f, @"'volume' out of range");

      StartCoroutine(PlayAsyncFromResorcesCoroutine(audioPath, audioPathAlt, volume, oneInstance, endCallback));
    }

    /// <summary>
    /// Parar la musica.
    /// </summary>
    public void StopMusic()
    {
      if (musicAudioSources[0].isPlaying == true)
        StartCoroutine(FadeMusic(musicAudioSources[0], false, false));
      else if (musicAudioSources[1].isPlaying == true)
        StartCoroutine(FadeMusic(musicAudioSources[1], false, false));
    }

    /// <summary>
    /// Parar un efecto concreto a partir del audioclip
    /// </summary>
    public void StopEffect(AudioClip audioClip)
    {
      for (int i = 0; i < effectsActive.Count; i++)
      {
        AudioSource sourceActive = effectsActive[i];

        if (sourceActive != null && sourceActive.clip == audioClip)
        {
          sourceActive.Stop();
          audioEffectsPool.PushToPool(sourceActive.gameObject);
          effectsPaused.Remove(sourceActive);
          effectsActive.RemoveAt(i);
          break;
        }
      }
    }

    /// <summary>
    /// Parar un efecto a partir de su nombre en el banco de sonidos
    /// </summary>
    public void StopEffect(string audioName)
    {
      if (allEffects.ContainsKey(audioName) == true)
        StopEffect(allEffects[audioName]);
    }

    /// <summary>
    /// Parar todos los efectos
    /// </summary>
    public void StopAllEffects()
    {
      while (effectsActive.Count != 0)
      {
        if (effectsActive[0] != null)
          StopEffect(effectsActive[0].clip);
      }
    }

    /// <summary>
    /// Parar todos los sonidos
    /// </summary>
    public void StopAllSounds()
    {
      StopAllEffects();
      StopMusic();
    }

    /// <summary>
    /// Pausar la musica
    /// </summary>
    public void PauseMusic()
    {
      if (musicAudioSources[0].isPlaying == true)
      {
        musicAudioSources[0].Pause();
        audioPaused[0] = true;
      }
      else if (musicAudioSources[1].isPlaying == true)
      {
        musicAudioSources[1].Pause();
        audioPaused[1] = true;
      }
    }

    /// <summary>
    /// Pausar un efecto concreto a partir del audioclip
    /// </summary>
    public void PauseEffect(AudioClip audioClip)
    {
      for (int i = 0; i < effectsActive.Count; i++)
      {
        AudioSource sourceActive = effectsActive[i];

        if (sourceActive.clip == audioClip)
        {
          effectsPaused[sourceActive] = true;
          sourceActive.Pause();
        }
      }
    }

    /// <summary>
    /// Pausar un efecto a partir de su nombre en el banco de sonidos
    /// </summary>
    public void PauseEffect(string audioName)
    {
      if (allEffects.ContainsKey(audioName) == true)
        PauseEffect(allEffects[audioName]);
    }

    /// <summary>
    /// Pausar todos los efectos
    /// </summary>
    public void PauseAllEffects()
    {
      for (int i = 0; i < effectsActive.Count; i++)
        PauseEffect(effectsActive[i].clip);
    }

    /// <summary>
    /// Pausar todos los sonidos
    /// </summary>
    public void PauseAllSounds()
    {
      PauseAllEffects();
      PauseMusic();
    }

    /// <summary>
    /// Reanudar la musica
    /// </summary>
    public void ResumeMusic()
    {
      if (audioPaused[0] == true)
      {
        musicAudioSources[0].UnPause();
        audioPaused[0] = false;
      }
      else if (audioPaused[1] == true)
      {
        musicAudioSources[1].UnPause();
        audioPaused[1] = false;
      }
    }

    /// <summary>
    /// Reanudar efecto
    /// </summary>
    public void ResumeEffect(AudioClip audioClip)
    {
      foreach (var element in effectsPaused.Keys)
      {
        if (element.clip == audioClip && effectsPaused[element] == true)
        {
          effectsPaused[element] = false;
          element.UnPause();
          break;
        }
      }
    }

    /// <summary>
    /// Reanudar un efecto a partir de su nombre en el banco de sonidos
    /// </summary>
    public void ResumeEffects(string audioName)
    {
      if (allEffects.ContainsKey(audioName) == true)
        ResumeEffect(allEffects[name]);
    }

    /// <summary>
    /// Reanudar todos los efectos
    /// </summary>
    public void ResumeAllEffects()
    {
      for (int i = 0; i < effectsActive.Count; i++)
        ResumeEffect(effectsActive[i].clip);
    }

    /// <summary>
    /// Reanudar todos los sonidos
    /// </summary>
    public void ResumeAllSounds()
    {
      ResumeAllEffects();
      ResumeMusic();
    }

    /// <summary>
    /// Reproducir la musica del banco de sonidos
    /// </summary>
    public void PlayAllMusic()
    {
      StartCoroutine(musicCoroutine);
    }

    /// <summary>
    /// Esta sonando el efecto ?
    /// </summary>
    public bool IsPlayingEffect(AudioClip audioClip)
    {
      for (int i = 0; i < effectsActive.Count; i++)
      {
        if (effectsActive[i].clip == audioClip)
          return effectsActive[i].isPlaying;
      }

      return false;
    }

    /// <summary>
    /// Esta sonando el efecto ?
    /// </summary>
    public bool IsPlayingEffect(string audioName)
    {
      if (allEffects.ContainsKey(audioName) == true)
        return IsPlayingEffect(allEffects[audioName]);

      return false;
    }

    /// <summary>
    /// Para un efecto reproducido con PlayEffectFromResources.
    /// </summary>
    public void StopEffectFromResources()
    {
      if (audioClipAsync != null)
        StopEffect(audioClipAsync);
    }

    private IEnumerator DelayedCallback(float time, Action callback)
    {
      UnityEngine.Assertions.Assert.IsTrue(time.CompareTo(0.0f) > 0, "");
      UnityEngine.Assertions.Assert.IsNotNull(callback);

      yield return new WaitForSeconds(time);

      callback();
    }

    private IEnumerator FadeMusic(AudioSource audio, bool fadeIn, bool fadeOut)
    {
      float fadeMultiplier = 0;

      if (fadeDuration > 0)
        fadeMultiplier = 1 / fadeDuration;

      float volume = audio.volume;
      float multiplier = 1 / audio.volume;

      if (fadeMultiplier > 0 && (fadeIn == true || fadeIn == true))
      {
        if (fadeIn == true)
        {
          audio.volume = 0f;
          audio.Play();

          while (audio.volume < volume)
          {
            audio.volume += Time.deltaTime * fadeMultiplier * multiplier;
            yield return null;
          }
        }
        else if (fadeOut == true)
        {
          while (audio.volume > 0)
          {
            audio.volume -= Time.deltaTime * fadeMultiplier * multiplier;
            yield return null;
          }

          audio.Stop();
        }
      }
      else
      {
        audio.volume = 0;
        audio.Stop();
      }
    }

    private IEnumerator StartPlayingMusic()
    {
      bool exit = false;
      bool finishCoroutine = false;
      bool loop = false;

      StopMusic();

      switch (playMusicMethod)
      {
        case PlayMusicMethod.ContinuousPlayThroughWithDelay:
          {
            if (musicDelay > 0)
              yield return musicDelay;
          }
          break;
        case PlayMusicMethod.ContinuousPlayThrough:
          {
            //@HACK:
            if (allMusic.Count == 1)
              loop = true;
          }
          break;
        case PlayMusicMethod.OncePlayThrough:
          {
            if (musicIndex == allMusic.Count - 1)
              finishCoroutine = true;
          }
          break;
        case PlayMusicMethod.OncePlayThroughWithDelay:
          {
            if (musicDelay > 0)
              yield return musicDelay;

            if (musicIndex == allMusic.Count - 1)
              finishCoroutine = true;
          }
          break;
        case PlayMusicMethod.ShufflePlayThrough:
          {
            //@TODO:
          }
          break;
        case PlayMusicMethod.ShufflePlayThroughWithDelay:
          {
            if (musicDelay > 0)
              yield return musicDelay;
          }
          break;
        case PlayMusicMethod.Manual:
          {
            exit = true;
            finishCoroutine = true;
          }
          break;
      }

      if (exit == false)
      {
        PlayMusic(allMusic[musicIndex % allMusic.Count], loop);

        yield return allMusic[musicIndex].length;
        musicIndex++;
      }

      if (finishCoroutine == true)
        yield return null;
      else
        PlayAllMusic();
    }

    private IEnumerator PlayAsyncFromResorcesCoroutine(string audioPath, string audioPathAlt, float volume, bool oneInstance, Action endCallback)
    {
      UnityEngine.Assertions.Assert.IsNotNull(audioPath, @"'audioFile' null or empty");
      UnityEngine.Assertions.Assert.IsTrue(volume >= 0.0f && volume <= 1.0f, @"'volume' out of range");

      ResourceRequest request = Resources.LoadAsync<AudioClip>(audioPath);
      if (request != null)
      {
        while (request.isDone == false)
          yield return null;

        // @HACK: Si ya existe un audio anterior y lo cortamos por 'oneInstance', tampoco llamamos a su callback.
        bool oneInstanceCallback = true;

        if (oneInstance == true && audioClipAsync != null)
        {
          oneInstanceCallback = false;

          StopEffect(audioClipAsync);
        }

        audioClipAsync = (AudioClip)request.asset;
        if (audioClipAsync != null)
        {
          duration = PlayEffectExtended(audioClipAsync, volume, false, null, 1, 4, true);

          if (endCallback != null && oneInstanceCallback == true)
            StartCoroutine(DelayedCallback(audioClipAsync.length, endCallback));
        }
        else if (string.IsNullOrEmpty(audioPathAlt) == false)
          PlayEffectFromResources(audioPathAlt, string.Empty, volume, null, oneInstance, endCallback);
        else
          Debug.LogWarningFormat("[AudioManager.LoadAsyncFromResorces] Invalid audio clip '{0}'.", audioPath);
      }
      else
        Debug.LogWarningFormat("[AudioManager.LoadAsyncFromResorces] '{0}' not found.", audioPath);
    }

    //@NOTA:Creamos dos nuevos diccionarios, los rellenamos hasta el indice guardado y los sustituimos por los anteriores
    private void DeleteLastSounds()
    {
      Dictionary<string, AudioClip> newEffects = new Dictionary<string, AudioClip>();
      Dictionary<string, float> newEffectsVolume = new Dictionary<string, float>();

      int counter = 0;

      foreach (var element in allEffects.Keys)
      {
        newEffects.Add(element, allEffects[element]);
        newEffectsVolume.Add(element, allEffectsVolumes[element]);
        counter++;

        if (counter == deleteLastEffectIndex)
          break;
      }

      allEffects = newEffects;
      allEffectsVolumes = newEffectsVolume;

      int start = allMusic.Count - 1;

      for (int i = start; i > deleteLastMusicIndex; i--)
      {
        if (allMusicVolumes.ContainsKey(allMusic[i].name) == true)
          allMusicVolumes.Remove(allMusic[i].name);

        allMusic.RemoveAt(i);
      }
    }

    private void LoadDictionaries()
    {
      SoundBank soundBank = null;

      if (soundsBank.Count == 0)
        Debug.LogWarningFormat("[AudioManager.LoadDictionaries] No hay bancos de sonido en el audio manager");

      for (int i = 0; i < soundsBank.Count; i++)
      {
        for (int j = 0; j < soundsBank[i].levels.Length; j++)
        {
          if (string.Equals(soundsBank[i].levels[j], SceneManager.GetActiveScene().name) == true)
          {
            soundBank = soundsBank[i];
            break;
          }
          else
            continue;
        }
      }

      if (soundBank != null)
      {
        //@NOTA:Si el banco de esta escena es el mismo que antes ahorramos la carga
        if (soundBank == lastBankLoaded)
          return;

        lastBankLoaded = soundBank;
        playMusicMethod = soundBank.playMusicMethod;

        //@NOTA:Antes de cargar los nuevos sonidos quitamos los de la escena anterior
        DeleteLastSounds();

        for (int j = 0; j < soundBank.soundsToPlay.Count; j++)
        {
          SoundBank.Soundconfig audio = soundBank.soundsToPlay[j];

          if (audio.soundType == SoundType.Music)
          {
            allMusic.Add(audio.audioClip);
            allMusicVolumes.Add(audio.audioClip.name, audio.volume);
            fixedMusicVolumes.Add(audio.audioClip.name, audio.volume);
          }
          else
          {
            allEffects.Add(audio.audioName, audio.audioClip);
            allEffectsVolumes.Add(audio.audioName, audio.volume);
            //@HACK
            if (fixedEffectsVolumes.ContainsKey(audio.audioClip.name) == false)
              fixedEffectsVolumes.Add(audio.audioClip.name, audio.volume);
            else
              Debug.LogWarningFormat("[AudioManager.LoadDictionaries] Hay audios duplicados {0}", audio.audioClip.name);
          }
        }

        //NOTA:Los sonidos de la escena root quedan fijos
        if (string.Equals(rootName, SceneManager.GetActiveScene().name) == true)
        {
          deleteLastMusicIndex = allMusic.Count - 1;
          deleteLastEffectIndex = allEffects.Count - 1;
        }

        elements = new List<string>(allEffectsVolumes.Keys);
      }
      else
        Debug.LogWarningFormat("[AudioManager.LoadDictionaries] No hay banco asociado a esta escena");
    }

    protected override void Awake()
    {
      base.Awake();

      audioEffectsPool = GetComponent<ObjectPool>();
      //sUnityEngine.Assertions.Assertions.IsNotNull(audioEffectsPool != null, "[AudioManager.Awake] El audio manager no tiene piscina");

      for (int i = 0; i < musicAudioSources.Length; i++)
      {
        musicAudioSources[i] = gameObject.AddComponent<AudioSource>();
        //Sonido 2D, componente oculto en la jerarquia
        musicAudioSources[i].hideFlags = HideFlags.HideInInspector;
        musicAudioSources[i].playOnAwake = false;
        musicAudioSources[i].spatialBlend = 0;
        musicAudioSources[i].ignoreListenerVolume = true;
      }
    }

    private void OnEnable()
    {
      //SceneLoader.OnBeforeLoad += LoadDictionaries;
    }

    private void OnDisable()
    {
      //SceneLoader.OnBeforeLoad -= LoadDictionaries;
    }

    private void Start()
    {
      if (useSceneLoader == false)
      {
        LoadDictionaries();
        musicCoroutine = StartPlayingMusic();
      }
      else
      {
        if (allMusic.Count != 0)
          musicCoroutine = StartPlayingMusic();
      }
    }

    private void Update()
    {
      for (int i = 0; i < effectsActive.Count; i++)
      {
        source = effectsActive[i];

        if (source != null && source.isPlaying == false && effectsPaused[source] == false && source.spatialBlend != 1.0f)
        {
          audioEffectsPool.PushToPool(source.gameObject);
          effectsActive.RemoveAt(i);
          effectsPaused.Remove(source);
          i--;
        }
      }
    }
  }
}