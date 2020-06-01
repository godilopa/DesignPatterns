///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// COPYRIGHT (C) TEACHLABS, SL - ALL RIGHTS RESERVED. UNAUTHORIZED COPYING OF THIS FILE, VIA ANY MEDIUM IS STRICTLY PROHIBITED.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace AudioManager
{
  /// <summary>
  /// Editor de banco de sonidos.
  /// </summary>
  [CustomEditor(typeof(SoundBank))]
  public class SoundBankEditor : Editor
  {
    private ReorderableList list;

    private void OnEnable()
    {
      list = new ReorderableList(serializedObject, serializedObject.FindProperty("audioClips"), true, true, true, true);

      list.drawHeaderCallback = (Rect rect) =>
      {
        //EditorGUI.LabelField(new Rect(rect.x + rect.width - 300, rect.y, 50, EditorGUIUtility.singleLineHeight), "ID");
        EditorGUI.LabelField(new Rect(rect.x + rect.width - 300, rect.y, 45, EditorGUIUtility.singleLineHeight), "Priority");
        //EditorGUI.LabelField(new Rect(rect.x + 215, rect.y, rect.width - 300, EditorGUIUtility.singleLineHeight), "Name");
        EditorGUI.LabelField(new Rect(rect.x + 80, rect.y, rect.width - EditorGUIUtility.currentViewWidth + 180, EditorGUIUtility.singleLineHeight), "Audio Clips");
        EditorGUI.LabelField(new Rect(rect.x + 15, rect.y, 60, EditorGUIUtility.singleLineHeight), "Type");
      };

      list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
      {
        SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(index);

        EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("type"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(rect.x + 65, rect.y, rect.width - EditorGUIUtility.currentViewWidth + 300, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("audioClip"), GUIContent.none);
        //EditorGUI.PropertyField(new Rect(rect.x + 205, rect.y, rect.width - 300, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("name"), GUIContent.none);
        EditorGUI.PropertyField(new Rect(rect.x + rect.width - 300, rect.y, 45, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("priority"), GUIContent.none);
        //EditorGUI.PropertyField(new Rect(rect.x + rect.width - 250, rect.y, 30, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("id"), GUIContent.none);
      };
    }

    public override void OnInspectorGUI()
    {
      serializedObject.Update();
      list.DoLayoutList();
      serializedObject.ApplyModifiedProperties();
    }
  }
}