///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
// COPYRIGHT (C) TEACHLABS, SL - ALL RIGHTS RESERVED. UNAUTHORIZED COPYING OF THIS FILE, VIA ANY MEDIUM IS STRICTLY PROHIBITED.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR
// IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;

/// <summary>
/// Extensiones de int.
/// </summary>
public static class IntExtension
{
  /// <summary>
  /// Ajustar aun intervalo
  /// </summary>
  public static int Clamp(this int a, int min, int max)
  {
    if (a < min)
      return min;

    if (a > max)
      return max;

    return a;
  }

  /// <summary>
  /// Potencia
  /// </summary>
  public static int Pow(this int a, int exp)
  {
    int result = 1;

    for (int i = 0; i < exp; i++)
      result *= a;

    return result;
  }

  /// <summary>
  /// Es un divisor ?
  /// </summary>
  public static bool IsDivisor(this int a, int d)
  {
    if (a == 0)
      return false;

    return a % d == 0;
  }

  /// <summary>
  /// Es un entero primo ?
  /// </summary>
  public static bool IsPrime(this int prime)
  {
    bool result = true;

    if (prime <= 1 || prime != 2 && prime % 2 == 0)
      result = false;

    int squareRounded = (int)Mathf.Round(Mathf.Sqrt(prime));

    for (int i = 3; i <= squareRounded; i++)
    {
      if (prime % i == 0)
      {
        result = false;
        break;
      }
    }

    return result;
  }

  /// <summary>
  /// Calculo del MCD de varios numeros.
  /// </summary>
  public static int MCD(this int a, params int[] list)
  {
    int result = a;

    for (int i = 0; i < list.Length; i++)
      result = MCD(result, list[i]);

    return result;
  }

  private static int MCD(int a, int b)
  {
    int t;

    while (a > 0)
    {
      t = a;
      a = b % a;
      b = t;
    }

    return b;
  }

  /// <summary>
  /// Calculo del MCM de varios numeros.
  /// </summary>
  public static int MCM(this int a, params int[] list)
  {
    int result = a;

    for (int i = 0; i < list.Length; i++)
      result = MCM(result, list[i]);

    return result;
  }

  private static int MCM(int a, int b)
  {
    int mcd = a.MCD(b);

    return a * b / mcd;
  }

  /// <summary>
  /// Calculo de la descomposicion factorial de un numero, devuelto en un array de la forma
  /// [2,3,5,2] 2^3*5^2 (solo admite 6 factores distintos en la descomposicion)
  /// </summary>
  public static int[] GetFactorialDecomposition(this int a)
  {
    int[] resultList = new int[12]; //@NOTA: Para numeros con mas de 6 factores primos aumentar el array
    int arrayCount = 0;

    if (a == 1 || a == 0)
    {
      resultList[0] = a;
      resultList[1] = 1;
    }
    else
    {
      while (a != 1)
      {
        if (a.IsPrime() == true)
        {
          resultList[arrayCount] = a;
          resultList[arrayCount + 1] = 1;
          break;
        }
        for (int i = 2; i <= a / 2; i++)
        {
          if (a % i == 0 && i.IsPrime() == true)
          {
            resultList[arrayCount] = i;
            arrayCount++;
            int numberOfDivisions = 0;

            while (a % i == 0)
            {
              a = a / i;
              numberOfDivisions++;
            }
            resultList[arrayCount] = numberOfDivisions;
            arrayCount++;
            break;
          }
        }
      }
    }

    return resultList;
  }

  /// <summary>
  /// Potencia mas cercana a dos.
  /// </summary>
  public static int NearestPowerOfTwo(this int number)
  {
    number--;
    number |= number >> 1;
    number |= number >> 2;
    number |= number >> 4;
    number |= number >> 8;
    number |= number >> 16;
    number++;

    return number;
  }

  /// <summary>
  /// Calcula la mascara de la layer.
  /// </summary>
  public static int GetMask(this int layer)
  {
    return 1 << layer;
  }

  /// <summary>
  /// Calcula la mascara de un conjunto de layers.
  /// </summary>
  public static int GetMask(this int[] layers)
  {
    int layerMask = 0;
    for (int i = 0; i < layers.Length; ++i)
      layerMask |= layers[i].GetMask();

    return layerMask;
  }

  /// <summary>
  /// Layer incluida?
  /// </summary>
  public static bool IsInLayerMask(this int layer, LayerMask layermask)
  {
    return layermask == (layermask | (1 << layer));
  }

  /// <summary>
  /// Layer incluida?
  /// </summary>
  public static bool IsInLayerMask(this int layer, string layerName)
  {
    int layermask = LayerMask.NameToLayer(layerName);

    return layermask == (layermask | (1 << layer));
  }

  /// <summary>
  /// Devuelve la posicion del primer bit que es 1 .01000 -> 3
  /// </summary>
  public static int DetectFirstBitOne(this int self)
  {
    for (int i = 31; i >= 0; i--)
    {
      if ((self & (1 << i)) == (1 << i))
        return i + 1;
    }

    return 0;
  }

  /// <summary>
  /// Devuelve un entero con todos los bit a 1 desde la posicion from a la posicion to. 1-4 ->  ...000011110
  /// </summary>
  public static int ChangeBitsToOne(this int a, int fromBit, int toBit)
  {
    int ret = 0;

    for (int i = fromBit; i <= toBit; i++)
      ret = ret | 1 << i;

    return ret;
  }

  /// <summary>
  /// Devuelve un entero con el ultimo bit de la mascara que sea 0 transformado en 1.  0010 -> 0110
  /// </summary>
  public static int ChangeLastBitToOne(this int self)
  {
    int movingBit = DetectFirstBitOne(self);
    return self ^ (1 << movingBit - 1);
  }

  /// <summary>
  /// Devuelve un entero con el ultimo bit de la mascara que sea 1 transformado en 0.  0110 -> 0010
  /// </summary>
  public static int ChangeLastBitToZero(this int self)
  {
    int movingBit = DetectFirstBitOne(self);
    return self | (1 << movingBit);
  }
}