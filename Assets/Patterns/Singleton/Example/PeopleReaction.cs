
using UnityEngine;

namespace SingletonExample
{
  public class PeopleReaction : MonoBehaviour
  {
    void OnEnable()
    {
      EventManager.StartListening("Shoot", Shoot);
    }

    void OnDisable()
    {
      EventManager.StopListening("Shoot", Shoot);
    }

    void Shoot()
    {
      Debug.Log("People start screaming !");
    }

    void Update()
    {
      if (Input.GetKeyDown("s"))
        EventManager.TriggerEvent("Shoot");
    }
  }
}
