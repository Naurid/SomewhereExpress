using Cinemachine;
using Cinemachine.Editor;
using UnityEngine;

[RequireComponent(typeof(OneShotDialogPlayer))]
public class DialogTrigger : MonoBehaviour
{
   private void OnTriggerEnter(Collider other)
   {
      if (other.CompareTag("Player"))
      {
         GetComponent<OneShotDialogPlayer>().PlayDialog();
      }
   }
}
