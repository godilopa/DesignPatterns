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
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace AudioManager
{
  /// <summary>
  /// Editor del audio manager.
  /// </summary>
  [CustomEditor(typeof(AudioManager))]
  public class AudioManagerEditor : Editor
  {
    AudioManager audioManager;

    private bool showInfo;

    private bool showAddSoundBanks;

    private bool showSoundBanks;

    private bool showAudioClips;

    private bool showList;

    private string[] levelsToAdd;

    private int levelIndex;

    private List<Tuple<AudioClip, AudioManager.SoundType, string, float>> soundsToAdd = new List<Tuple<AudioClip, AudioManager.SoundType, string, float>>();

    private AudioManager.PlayMusicMethod playMethodToAdd;

    private string soundBankName;

    private AudioManager.SoundType soundTypeToAdd;

    private AudioClip clipToAddFromSelector;

    private string clipNameToAdd;

    private float volumeToAdd = 1f;

    private int flags = 0;

    private bool modifiyingABank = false;

    private GUIStyle style = new GUIStyle();

    private int maxMask;

    private int minMask;

    public override void OnInspectorGUI()
    {
      GUIStyle foldoutStyle = CreateFoldoutGUI();

      EditorGUI.indentLevel++;
      EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true));
      {
        EditorGUILayout.BeginHorizontal();
        {
          GUI.color = Color.white;
          showInfo = EditorGUILayout.Foldout(showInfo, new GUIContent(@"Info", @"Escena actual y canciones que se estan reproduciendo."), foldoutStyle);
        }
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndVertical();
      EditorGUI.indentLevel--;

      EditorPrefs.SetBool("showInfo", showInfo);
      if (showInfo == true)
      {
        EditorGUILayout.BeginVertical();
        {
          ShowInfo();
        }
        EditorGUILayout.EndVertical();
      }
      EditorGUILayout.Separator();

      EditorGUI.indentLevel++;
      EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true));
      {
        EditorGUILayout.BeginHorizontal();
        {
          GUI.color = Color.white;
          showAddSoundBanks = EditorGUILayout.Foldout(showAddSoundBanks, new GUIContent(@"Ver Bancos de sonidos", @"Bancos de sonidos del juego."), foldoutStyle);
        }
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndVertical();
      EditorGUI.indentLevel--;

      EditorPrefs.SetBool("showAddSoundBanks", showAddSoundBanks);
      if (showAddSoundBanks == true)
      {
        EditorGUILayout.BeginVertical();
        {
          ShowAddSoundBankList();
        }
        EditorGUILayout.EndVertical();
      }
      EditorGUILayout.Separator();

      EditorGUI.indentLevel++;
      EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true));
      {
        EditorGUILayout.BeginHorizontal();
        {
          GUI.color = Color.white;
          showSoundBanks = EditorGUILayout.Foldout(showSoundBanks, new GUIContent(@"Añadir Banco de sonidos", @"Banco de sonidos para una escena determinada, root si esta en todas."), foldoutStyle);
        }
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndVertical();
      EditorGUI.indentLevel--;

      EditorPrefs.SetBool("showSoundBanks", showSoundBanks);
      if (showSoundBanks == true)
      {
        EditorGUILayout.BeginVertical();
        {
          AddSoundBanks();
        }
        EditorGUILayout.EndVertical();
      }
      EditorGUILayout.Separator();

      EditorGUI.indentLevel++;
      EditorGUILayout.BeginVertical(EditorStyles.objectFieldThumb, GUILayout.ExpandWidth(true));
      {
        EditorGUILayout.BeginHorizontal();
        {
          GUI.color = Color.white;
          showAudioClips = EditorGUILayout.Foldout(showAudioClips, new GUIContent(@"Añadir Sonidos"), foldoutStyle);
        }
        EditorGUILayout.EndHorizontal();
      }
      EditorGUILayout.EndVertical();
      EditorGUI.indentLevel--;

      EditorPrefs.SetBool("showAudioClips", showAudioClips);
      if (showAudioClips == true)
      {
        EditorGUILayout.BeginVertical();
        {
          AddAudioClips();
        }
        EditorGUILayout.EndVertical();
      }
      EditorGUILayout.Separator();

      DrawDefaultInspector();
      EditorUtility.SetDirty(target);
    }

    private GUIStyle CreateFoldoutGUI()
    {
      GUIStyle myFoldoutStyle = new GUIStyle(EditorStyles.foldout);
      Color myStyleColor = Color.white;
      myFoldoutStyle.fontStyle = FontStyle.Bold;
      myFoldoutStyle.normal.textColor = myStyleColor;
      myFoldoutStyle.onNormal.textColor = myStyleColor;
      myFoldoutStyle.hover.textColor = myStyleColor;
      myFoldoutStyle.onHover.textColor = myStyleColor;
      myFoldoutStyle.focused.textColor = myStyleColor;
      myFoldoutStyle.onFocused.textColor = myStyleColor;
      myFoldoutStyle.active.textColor = myStyleColor;
      myFoldoutStyle.onActive.textColor = myStyleColor;

      return myFoldoutStyle;
    }

    private void ShowInfo()
    {
      EditorGUI.indentLevel++;
      {
        EditorGUILayout.BeginHorizontal();
        {
          EditorGUILayout.LabelField(@"<b>Escena actual:</b>", style);
          EditorGUILayout.LabelField(EditorSceneManager.GetActiveScene().name);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        {
          EditorGUILayout.LabelField(@"<b>Duracion del fade:</b>", style);
          audioManager.FadeDuration = EditorGUILayout.FloatField(audioManager.FadeDuration);
        }
        EditorGUILayout.EndHorizontal();

        if (audioManager.FadeDuration < 0)
          audioManager.FadeDuration = 0f;

        int audioSize = 0;

        if (audioManager.MusicAudioSources != null && audioManager.MusicAudioSources[0] != null && audioManager.MusicAudioSources[1] != null)
          audioSize = audioManager.MusicAudioSources.Length;

        style.alignment = TextAnchor.MiddleCenter;
        EditorGUILayout.LabelField(@"<b>Musica reproduciendose</b>", style);
        //Musica reproduciendo
        if (audioSize != 0)
        {
          string name1, name2;
          name1 = name2 = @"No Song";

          GUI.color = Color.green;
          if (audioManager.MusicAudioSources[0] != null && audioManager.MusicAudioSources[0].clip != null)
            name1 = audioManager.MusicAudioSources[0].clip.name;
          else
            GUI.color = Color.red;

          Rect rect = GUILayoutUtility.GetRect(28, 28, @"TextField");
          if (name1 != @"No Song")
            name1 += "\n" + System.TimeSpan.FromSeconds(audioManager.MusicAudioSources[0].time).ToString().Split('.')[0] + @"/" + System.TimeSpan.FromSeconds(audioManager.MusicAudioSources[0].clip.length).ToString().Split('.')[0];
          EditorGUI.ProgressBar(rect, audioManager.MusicAudioSources[0].volume, name1);

          GUI.color = Color.green;
          if (audioManager.MusicAudioSources[1] != null && audioManager.MusicAudioSources[1].clip != null)
            name2 = audioManager.MusicAudioSources[1].clip.name;
          else
            GUI.color = Color.red;

          Rect rect2 = GUILayoutUtility.GetRect(28, 28, @"TextField");
          if (name2 != @"No Song")
            name2 += @"\n" + System.TimeSpan.FromSeconds(audioManager.MusicAudioSources[1].time).ToString().Split('.')[0] + @"/" + System.TimeSpan.FromSeconds(audioManager.MusicAudioSources[1].clip.length).ToString().Split('.')[0];
          EditorGUI.ProgressBar(rect2, audioManager.MusicAudioSources[1].volume, name2);

          GUI.color = Color.white;
          Repaint();
        }
        else
        {
          GUI.color = Color.red;
          Rect rect = GUILayoutUtility.GetRect(28, 28, @"TextField");
          EditorGUI.ProgressBar(rect, 0f, @"Standing By...");

          Rect rect2 = GUILayoutUtility.GetRect(28, 28, @"TextField");
          EditorGUI.ProgressBar(rect2, 0f, @"Standing By...");
          GUI.color = Color.white;
        }
      }

      EditorGUILayout.LabelField(@"<b>Efectos reproduciendose</b>", style);
      //Efectos reproduciendo
      if (audioManager.EffectsActive.Count != 0)
      {
        int length = audioManager.EffectsActive.Count;
        for (int i = 0; i < length; i++)
        {
          string name1 = audioManager.EffectsActive[i].clip.name;
          GUI.color = Color.green;
          Rect effectRect = GUILayoutUtility.GetRect(28, 28, @"TextField");
          name1 += "\n" + System.TimeSpan.FromSeconds(audioManager.EffectsActive[i].time).ToString().Split('.')[0] + @"/" + System.TimeSpan.FromSeconds(audioManager.EffectsActive[i].clip.length).ToString().Split('.')[0];
          EditorGUI.ProgressBar(effectRect, audioManager.EffectsActive[i].volume, name1);
        }
        GUI.color = Color.white;
        Repaint();
      }

      style.alignment = TextAnchor.MiddleLeft;
      EditorGUI.indentLevel--;
      EditorGUILayout.Space();
    }

    private void ShowAddSoundBankList()
    {
      bool delete = false;
      int indexToDelete = -1;

      EditorGUI.indentLevel++;
      {
        for (int i = 0; i < audioManager.soundsBank.Count; i++)
        {
          ShowSoundBank(audioManager.soundsBank[i], i, ref delete);
          if (delete == true)
          {
            indexToDelete = i;
            break;
          }
        }
      }
      EditorGUI.indentLevel--;

      if (indexToDelete != -1)
        audioManager.soundsBank.RemoveAt(indexToDelete);

      EditorGUILayout.Space();
    }

    private void ShowSoundBank(SoundBank soundBank, int index, ref bool delete)
    {
      EditorGUILayout.BeginHorizontal();
      {
        EditorGUILayout.LabelField(string.Format("<b>Nombre: </b> {0}", soundBank.audioBankName), style);

        GUI.color = Color.red;
        if (GUILayout.Button(@"Eliminar", GUILayout.Width(80f)))
        {
          delete = true;
          return;
        }

        GUI.color = Color.green;
        if (GUILayout.Button(@"Modificar", GUILayout.Width(80f)))
        {
          LoadSoundBankDataToModify(soundBank);
          delete = true;
          return;
        }

        GUI.color = Color.white;
      }
      EditorGUILayout.EndHorizontal();

      EditorGUILayout.LabelField(string.Format("<b>Modo de reproduccion:</b> {0}", soundBank.playMusicMethod), style);

      GUIStyle foldoutStyle = new GUIStyle(EditorStyles.foldout);
      foldoutStyle.richText = true;

      audioManager.showBankSoundLevelOrSounds[index].showBankSoundLevel = EditorGUILayout.Foldout(audioManager.showBankSoundLevelOrSounds[index].showBankSoundLevel, new GUIContent(@"<b>Ver escenas</b>"), foldoutStyle);
      if (audioManager.showBankSoundLevelOrSounds[index].showBankSoundLevel == true)
      {
        for (int i = 0; i < soundBank.levels.Length; i++)
        {
          EditorGUI.indentLevel++;
          EditorGUILayout.BeginVertical();
          {
            EditorGUILayout.LabelField(string.Format("Escena {1}: {0}", soundBank.levels[i], i.ToString()));
          }
          EditorGUILayout.EndVertical();
          EditorGUI.indentLevel--;
        }
      }
      EditorGUILayout.Separator();

      audioManager.showBankSoundLevelOrSounds[index].showBankSoundSounds = EditorGUILayout.Foldout(audioManager.showBankSoundLevelOrSounds[index].showBankSoundSounds, new GUIContent(@"<b>Ver audios</b>"), foldoutStyle);

      if (audioManager.showBankSoundLevelOrSounds[index].showBankSoundSounds == true)
      {
        GUIStyle textStyle = new GUIStyle(EditorStyles.textField);
        foldoutStyle.font = EditorStyles.boldFont;

        for (int i = 0; i < soundBank.soundsToPlay.Count; i++)
        {
          EditorGUI.indentLevel++;
          EditorGUILayout.BeginVertical();
          {
            EditorGUILayout.LabelField(string.Format("Nombre: {0}", soundBank.soundsToPlay[i].audioName), textStyle);
            EditorGUILayout.LabelField(string.Format("Volumen: {0}", soundBank.soundsToPlay[i].volume.ToString()));
            if (soundBank.soundsToPlay[i].audioClip != null)
              EditorGUILayout.LabelField(string.Format("Clip: {0}", soundBank.soundsToPlay[i].audioClip.name));
            EditorGUILayout.LabelField(string.Format("Tipo de sonido: {0}", soundBank.soundsToPlay[i].soundType.ToString()));
          }
          EditorGUILayout.EndVertical();
          EditorGUI.indentLevel--;
        }
      }
    }

    private void LoadSoundBankDataToModify(SoundBank soundBank)
    {
      soundBankName = soundBank.audioBankName;
      levelsToAdd = soundBank.levels;
      playMethodToAdd = soundBank.playMusicMethod;

      soundsToAdd.Clear();

      for (int i = 0; i < soundBank.soundsToPlay.Count; i++)
        soundsToAdd.Add(new Tuple<AudioClip, AudioManager.SoundType, string, float>(soundBank.soundsToPlay[i].audioClip, soundBank.soundsToPlay[i].soundType, soundBank.soundsToPlay[i].audioName, soundBank.soundsToPlay[i].volume));

      modifiyingABank = true;
    }

    private void AddSoundBanks()
    {
      ShowSoundBankConfigData();
      ShowAddSongList();
      EditorGUILayout.Space();
      ShowAddSoundBankButton();
      EditorGUILayout.Space();
    }

    private void AddAudioClips()
    {
      DropAreaGUI();
      ShowAddClipSelector();
      EditorGUILayout.Space();
    }

    private void ShowAddSoundBankButton()
    {
      GUILayout.BeginHorizontal();
      {
        GUILayout.FlexibleSpace();
        {
          GUI.color = Color.green;
          if (GUILayout.Button(@"Añadir banco de sonido", GUILayout.MaxWidth(250f)))
          {
            AddSoundBank();
          }
          GUI.color = Color.white;
        }
        GUILayout.FlexibleSpace();
      }
      GUILayout.EndHorizontal();
    }

    private void AddSoundBank()
    {
      if (soundBankName != string.Empty && CheckSoundBankName(soundBankName) == true)
      {
        if (soundsToAdd.Count > 0)
        {
          Undo.RecordObject(audioManager, @"Add SoundBank");

          List<Tuple<AudioClip, AudioManager.SoundType, string, float>> sounds = new List<Tuple<AudioClip, AudioManager.SoundType, string, float>>(soundsToAdd.Count);
          //Copiamos los sonidos en la nueva lista
          soundsToAdd.Copy(sounds);

          SoundBank soundBank = new SoundBank(levelsToAdd, sounds, playMethodToAdd, soundBankName);

          if (modifiyingABank == false)
            audioManager.soundsBank.Add(soundBank);
          else
          {
            if (EditorUtility.DisplayDialog(@"Añadir banco", @"¿Seguro que quieres modificarlo?", @"Si", @"No") == true)
            {
              audioManager.soundsBank.Add(soundBank);
              modifiyingABank = false;
            }
            else
              return;
          }
          //Reiniciamos variables
          levelsToAdd = null;
          soundBankName = string.Empty;
          soundsToAdd.Clear();
          playMethodToAdd = AudioManager.PlayMusicMethod.ContinuousPlayThrough;

          //NOTA:Añadimos a la lista de foldaout para la hora de visualizar el banco
          audioManager.showBankSoundLevelOrSounds.Add(new AudioManager.ShowBankSoundLevelOrSounds(false, false));
          SceneView.RepaintAll();
        }
        else
          EditorUtility.DisplayDialog(@"Error al añadir el banco", @"No hay sonidos", @"Entendido");
      }
      else
        EditorUtility.DisplayDialog(@"Error al añadir el banco", @"Falta poner un nombre o el nombre esta repetido", @"Entendido");
    }

    private bool CheckSoundBankName(string bankName)
    {
      for (int i = 0; i < audioManager.soundsBank.Count; i++)
      {
        if (string.Compare(audioManager.soundsBank[i].audioBankName, bankName) == 0)
          return false;
      }

      return true;
    }

    private void CheckVolume(ref float volume)
    {
      if (volume < 0)
      {
        volume = 0;
        Repaint();
      }

      if (volume > 1)
      {
        volume = 1;
        Repaint();
      }
    }

    private void ShowAddClipSelector()
    {
      EditorGUILayout.BeginVertical();
      {
        clipToAddFromSelector = EditorGUILayout.ObjectField(@"Elige un clip de sonido:", clipToAddFromSelector, typeof(AudioClip), false) as AudioClip;
        clipNameToAdd = EditorGUILayout.TextField(@"Nombre:", clipNameToAdd);

        volumeToAdd = EditorGUILayout.FloatField(@"Volumen:", volumeToAdd);
        CheckVolume(ref volumeToAdd);

        soundTypeToAdd = (AudioManager.SoundType)EditorGUILayout.EnumPopup(@"Tipo de sonido", soundTypeToAdd);
      }
      EditorGUILayout.EndVertical();

      EditorGUILayout.Space();

      GUILayout.BeginHorizontal();
      {
        GUILayout.FlexibleSpace();
        {
          GUI.color = Color.green;
          if (GUILayout.Button(@"Añadir audio", GUILayout.MaxWidth(250f)))
          {
            if (clipNameToAdd != string.Empty && (CheckAudioName(clipNameToAdd) == true))
            {
              if (clipToAddFromSelector != null)
              {
                Tuple<AudioClip, AudioManager.SoundType, string, float> tuple = new Tuple<AudioClip, AudioManager.SoundType, string, float>(clipToAddFromSelector, soundTypeToAdd, clipNameToAdd, volumeToAdd);
                soundsToAdd.Add(tuple);
                //Reiniciamos variables
                clipToAddFromSelector = null;
                clipNameToAdd = string.Empty;
                volumeToAdd = 1;
                soundTypeToAdd = AudioManager.SoundType.Music;
              }
              else
                EditorUtility.DisplayDialog(@"Error al añadir el audio", @"Falta el audio clip", @"Entendido");
            }
            else
              EditorUtility.DisplayDialog(@"Error al añadir el audio", @"Falta poner un nombre o esta repetido", @"Entendido");
          }

          GUI.color = Color.white;
          GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
      }
    }

    private bool CheckAudioName(string name)
    {
      for (int i = 0; i < soundsToAdd.Count; i++)
      {
        if (string.Compare(soundsToAdd[i].Item3, name) == 0)
          return false;
      }

      return true;
    }

    private void ShowAddSongList()
    {
      int soundToRemove = -1;
      EditorGUILayout.LabelField(@"Lista de sonidos:");
      EditorGUI.indentLevel++;
      showList = EditorGUILayout.Foldout(showList, new GUIContent(@"Ver audios"));

      if (showList == true)
      {
        for (int i = 0; i < soundsToAdd.Count; i++)
        {
          EditorGUILayout.BeginHorizontal();
          {
            EditorGUILayout.LabelField(string.Format("Sonido {0}:", i.ToString()));

            GUI.color = Color.red;
            if (GUILayout.Button(@"-", GUILayout.Width(20f)))
            {
              soundToRemove = i;
              break;
            }
            GUI.color = Color.white;
          }
          EditorGUILayout.EndHorizontal();

          EditorGUI.indentLevel++;
          EditorGUILayout.BeginVertical();
          {
            EditorGUILayout.LabelField(string.Format("Nombre: {0}", soundsToAdd[i].Item3));
            EditorGUILayout.LabelField(string.Format("Volumen: {0}", soundsToAdd[i].Item4.ToString()));
            if (soundsToAdd[i].Item1 != null)
              EditorGUILayout.LabelField(string.Format("Clip: {0}", soundsToAdd[i].Item1.name));
            EditorGUILayout.LabelField(string.Format("Tipo de sonido: {0}", soundsToAdd[i].Item2.ToString()));
          }
          EditorGUILayout.EndVertical();
          EditorGUI.indentLevel--;
        }
        if (soundToRemove >= 0)
        {
          soundsToAdd.RemoveAt(soundToRemove);
          Repaint();
          return;
        }
      }
      EditorGUI.indentLevel--;
    }

    private string[] GetLevelNames()
    {
      List<string> availableScenes = new List<string>();

      for (int i = 0; i < EditorBuildSettings.scenes.Length; i++)
      {
        EditorBuildSettingsScene scene = EditorBuildSettings.scenes[i];
        if (scene.enabled == true)
        {
          string sceneName = scene.path.Substring(scene.path.LastIndexOf('/') + 1, (scene.path.LastIndexOf('.') - scene.path.LastIndexOf('/')) - 1);
          //Buscar si ya hay algun banco en esa escena
          bool add = true;
          for (int j = 0; j < audioManager.soundsBank.Count; j++)
          {
            for (int k = 0; k < audioManager.soundsBank[j].levels.Length; k++)
            {
              if (string.Compare(audioManager.soundsBank[j].levels[k], sceneName) == 0)
              {
                add = false;
                break;
              }
            }
          }

          if (add == true)
            availableScenes.Add(sceneName);
        }
      }

      return availableScenes.ToArray();
    }

    private void ShowSoundBankConfigData()
    {
      string[] availableNames = GetLevelNames();
      List<string> selectedOptions = new List<string>();

      if (availableNames.Length == 0)
        EditorGUILayout.LabelField(@"Todas las escenas tienen un sound bank asociado.");

      EditorGUILayout.BeginHorizontal();
      {
        EditorGUILayout.LabelField(@"Añadir quitar escena al final ->", GUILayout.Width(180));

        GUI.color = Color.red;
        if (GUILayout.Button(@"-", GUILayout.Width(20f)))
          flags = flags.ChangeLastBitToOne();

        GUI.color = Color.green;
        if (GUILayout.Button(@"+", GUILayout.Width(20f)))
          flags = flags.ChangeLastBitToZero();
      }

      GUILayout.Space(20);

      GUI.color = Color.white;
      EditorGUILayout.BeginVertical();
      {
        EditorGUILayout.LabelField(@"Maximo y minimo->", GUILayout.Width(120));

        if (GUILayout.Button(@"Establecer", GUILayout.Width(100f)))
        {
          if (minMask <= maxMask)
            flags = flags.ChangeBitsToOne(minMask, maxMask);
          else
            EditorUtility.DisplayDialog(@"Error al añadir escenas ", @" EL maximo no puede ser menor que el minimo ", @"Entendido ");
        }
      }
      EditorGUILayout.EndVertical();

      EditorGUILayout.BeginVertical();
      {
        minMask = EditorGUILayout.IntSlider(minMask, 0, availableNames.Length - 1, GUILayout.Width(150));
        maxMask = EditorGUILayout.IntSlider(maxMask, 0, availableNames.Length - 1, GUILayout.Width(150));
      }
      EditorGUILayout.EndVertical();

      EditorGUILayout.EndHorizontal();

      flags = EditorGUILayout.MaskField(@"Elige escenas: ", flags, availableNames);

      soundBankName = EditorGUILayout.TextField(@"Nombre del banco: ", soundBankName, GUILayout.MinWidth(100), GUILayout.ExpandWidth(true));

      GUI.color = Color.white;

      for (int i = 0; i < availableNames.Length; i++)
      {
        if ((flags & (1 << i)) == (1 << i))
          selectedOptions.Add(availableNames[i]);
      }

      levelsToAdd = selectedOptions.ToArray();
      playMethodToAdd = (AudioManager.PlayMusicMethod)EditorGUILayout.EnumPopup(@"Modo de reproduccion de la musica: ", playMethodToAdd, EditorStyles.popup);

      if (playMethodToAdd == AudioManager.PlayMusicMethod.ContinuousPlayThroughWithDelay || playMethodToAdd == AudioManager.PlayMusicMethod.OncePlayThroughWithDelay ||
        playMethodToAdd == AudioManager.PlayMusicMethod.ShufflePlayThroughWithDelay)
        audioManager.MusicDelay = EditorGUILayout.FloatField(@"Select Delay: ", audioManager.MusicDelay);
    }

    private int DetectFirstBitOne(int flag)
    {
      for (int i = 31; i >= 0; i--)
      {
        if ((flag & (1 << i)) == (1 << i))
          return i + 1;
      }

      return 0;
    }

    private int ChangeBitsToOne(int fromBit, int toBit)
    {
      int ret = 0;

      for (int i = fromBit; i <= toBit; i++)
        ret = ret | 1 << i;

      return ret;
    }

    private void DropAreaGUI()
    {
      GUI.color = Color.gray;
      EditorGUILayout.BeginVertical();
      {
        Event evt = Event.current;

        Rect dropArea = GUILayoutUtility.GetRect(0f, 50f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, @"Drag AudioClip(s)Here ");

        switch (evt.type)
        {
          case EventType.DragUpdated:
          case EventType.DragPerform:
            if (!dropArea.Contains(evt.mousePosition))
              break;

            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            if (evt.type == EventType.DragPerform)
            {
              DragAndDrop.AcceptDrag();

              for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
              {
                AudioClip aC = DragAndDrop.objectReferences[i] as AudioClip;
                if (!aC || aC.GetType() != typeof(AudioClip))
                  continue;

                clipToAddFromSelector = aC;
              }
            }
            Event.current.Use();
            break;
        }
      }
      EditorGUILayout.EndVertical();
      GUI.color = Color.white;
    }

    private void OnEnable()
    {
      style.richText = true;
      showSoundBanks = EditorPrefs.GetBool("showSoundBanks ");
      showInfo = EditorPrefs.GetBool("showInfo ");
      showAddSoundBanks = EditorPrefs.GetBool("showAddSoundBanks ");
      showAudioClips = EditorPrefs.GetBool("showAudioClips ");
      audioManager = target as AudioManager;
    }
  }
}