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

/// <summary>
/// Extension de List.
/// </summary>
public static class ListExtension
{
  /// <summary>
  /// Reordena al azar.
  /// </summary>
  public static void Shuffle<T>(this IList<T> list)
  {
    int n = list.Count;

    while (n > 1)
    {
      n--;
      int k = UnityEngine.Random.Range(0, n + 1);

      T value = list[k];
      list[k] = list[n];
      list[n] = value;
    }
  }

  /// <summary>
  /// Devuelve un item al azar o una excepcion.
  /// </summary>
  public static T RandomItem<T>(this IList<T> list)
  {
    if (list.Count == 0)
      Debug.LogException(new IndexOutOfRangeException(@"[Foundation.ListExtension] RandomItem() Cannot select a random item from an empty list."));

    return list[UnityEngine.Random.Range(0, list.Count)];
  }

  /// <summary>
  /// Borra, y devuelve, un item al azar.
  /// </summary>
  public static T RemoveRandom<T>(this IList<T> list)
  {
    if (list.Count == 0)
      Debug.LogException(new IndexOutOfRangeException(@"[Foundation.ListExtension] RemoveRandom() Cannot remove a random item from an empty list."));

    int index = UnityEngine.Random.Range(0, list.Count);
    T item = list[index];
    list.RemoveAt(index);

    return item;
  }

  /// <summary>
  /// Compara los elementos con otra lista sin tener en cuenta su orden. Los elementos deben ser IComparable.
  /// </summary>
  public static bool UnorderedEqual<T>(this IList<T> list, IList<T> other) where T : IComparable
  {
    UnityEngine.Assertions.Assert.IsNotNull(other, @"[Foundation.ListExtension] Null list.");

    if (list.Count != other.Count)
      return false;

    for (int i = 0; i < list.Count; ++i)
    {
      bool found = false;
      for (int j = 0; j < other.Count; ++j)
      {
        if (list[i].CompareTo(other[j]) == 0)
          found = true;
      }

      if (found == false)
        return false;
    }

    return true;
  }

  /// <summary>
  /// Reemplaza un elemento.
  /// </summary>
  public static void Replace<T>(this IList<T> list, T oldItem, T newItem)
  {
    var oldItemIndex = list.IndexOf(oldItem);

    list[oldItemIndex] = newItem;
  }

  /// <summary>
  /// Reemplaza un elemento.
  /// </summary>
  public static void Copy<T>(this IList<T> list, IList<T> listToCopy)
  {
    for (int i = 0; i < listToCopy.Count; i++)
      list.Add(listToCopy[i]);
  }

  /// <summary>
  /// Intercambia elementos.
  /// </summary>
  public static void Swap<T>(this IList<T> list, int indexA, int indexB)
  {
    T tmp = list[indexA];
    list[indexA] = list[indexB];
    list[indexB] = tmp;
  }
}