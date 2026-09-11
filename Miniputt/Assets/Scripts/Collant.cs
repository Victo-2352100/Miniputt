using UnityEngine;
/// <summary>
/// Script gérant les objets gluants du parcours de miniputt
/// </summary>
[RequireComponent (typeof(BoxCollider))]
public class Collant : MonoBehaviour
{
    [SerializeField, Header("Pourcentage seulement. 0 < pourcentageRestant < 100")]
    private float pourcentageRestant = 80;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /// <summary>
    /// Méthode qui se lance dès la première boucle. Avertis l'utilisateur en cas de mauvais assignation de la variable
    /// </summary>
    void Start()
    {
        if (pourcentageRestant < 0 || pourcentageRestant > 100)
        {
            Debug.LogError("[Sticky] Le pourcentage doit être plus grand que zéro et moins grand que 100.");
        }
    }
    /// <summary>
    /// Méthode qui se lance lorsque le collider détecte la présence d'un objet ayant un rigidbody
    /// </summary>
    /// <param name="other">Le gameObject ayant fait trigger</param>
    private void OnTriggerStay(Collider other)
    {
        //Si c'est bien la balle de golf,
        if (other.TryGetComponent(out BalleGolf balle) && other.TryGetComponent(out Rigidbody rb))
        {
            //Calculer le ralentissement pour que la vitesse soit le pourcentage choisi de la vitesse initiale
            //             X                              pourcentageRestant
            //    --------------------               =  ---------------
            //      velociteComplete                         100
            //Donc velociteComplete * 80 / 100 vvv
            Vector3 velociteReduite = (other.attachedRigidbody.linearVelocity * pourcentageRestant) / 100;
            other.attachedRigidbody.linearVelocity = velociteReduite;
        }
    }
    /// <summary>
    /// Méthode qui se lance lorsque le collant détecte le départ d'un objet de sa boîte à collisions
    /// </summary>
    /// <param name="other">Le gameObject ayant fait trigger</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out BalleGolf balle) && other.TryGetComponent(out Rigidbody rb))
        {
            //On inverse maintenant pour revenir à la vitesse initiale:
            //      VelocitePartielle                   pourcentageRestant
            //    --------------------               =  ---------------
            //              X                                 100
            //Donc la velocite de la balle * 100 / 80
            Vector3 velociteReduite = (other.attachedRigidbody.linearVelocity * 100) / pourcentageRestant;
            other.attachedRigidbody.linearVelocity = velociteReduite;
        }
    }
}
