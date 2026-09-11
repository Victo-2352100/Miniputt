using UnityEngine;

/// <summary>
/// Script portant sur une plaque d'accélération pour balle de miniputt
/// </summary>
public class Accelerateur : MonoBehaviour
{
    [SerializeField]
    private float forceAccelerateur;
    /// <summary>
    /// Méthode appelée lorsque ce gameObject détecte un objet entrant dans sa zone de trigger
    /// </summary>
    /// <param name="other">Objet détecté dans la zone de trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BalleGolf balle) && other.TryGetComponent(out Rigidbody rb))
        {
            //On va pas changer le déplacement de force, seulement ajouter plus de vitesse vers un axe.
            //Puisque les flèches de l'accélérateur pointent vers l'inverse de l'axe Z du gameobject, on va négativiser l'accélération sur Z.
            other.attachedRigidbody.AddForce(transform.forward * -forceAccelerateur, ForceMode.Impulse);
        }
    }
}
