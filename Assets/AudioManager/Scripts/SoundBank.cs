///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// COPYRIGHT (C) TEACHLABS, SL - ALL RIGHTS RESERVED. UNAUTHORIZED COPYING OF THIS FILE, VIA ANY MEDIUM IS STRICTLY PROHIBITED.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

using UnityEngine;

namespace AudioManager
{
  /// <summary>
  /// Banco de sonidos.
  /// </summary>
  [System.Serializable]
  public class SoundBank
  {
    /// <summary>
    /// Clips de audio, tipo de sonido y volumen.
    /// </summary>
    [System.Serializable]
    public class Soundconfig
    {
      public AudioClip audioClip;
      public AudioManager.SoundType soundType;
      public string audioName;
      public float volume;

      /// <summary>
      /// Constructor asociado a un sonido.
      /// </summary>
      public Soundconfig(AudioClip audioClip, AudioManager.SoundType soundType, string name, float volume)
      {
        this.audioClip = audioClip;
        this.soundType = soundType;
        this.audioName = name;
        this.volume = volume;
      }
    }

    public List<Soundconfig> soundsToPlay = new List<Soundconfig>();

    /// <summary>
    /// Nombre del banco de sonidos.
    /// </summary>
    [SerializeField]
    public string audioBankName;

    /// <summary>
    /// Niveles asociados a este banco.
    /// </summary>
    [SerializeField]
    public string[] levels;

    /// <summary>
    /// Tipo de reproduccion de la musica de esta banco.
    /// </summary>
    [SerializeField]
    public AudioManager.PlayMusicMethod playMusicMethod;

    /// <summary>
    /// Constructor asociado a un nivel.
    /// </summary>
    public SoundBank(string[] levels, List<Tuple<AudioClip, AudioManager.SoundType, string, float>> audios, AudioManager.PlayMusicMethod playMusicMethod, string bankName)
    {
      this.levels = levels;

      for (int i = 0; i < audios.Count; ++i)
        soundsToPlay.Add(new Soundconfig(audios[i].Item1, audios[i].Item2, audios[i].Item3, audios[i].Item4));

      this.audioBankName = bankName;
      this.playMusicMethod = playMusicMethod;
    }
  }
}