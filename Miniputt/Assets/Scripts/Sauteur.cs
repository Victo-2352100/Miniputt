using UnityEngine;
/// <summary>
/// Script portant sur une plaque rebondissante lorsqu'une balle de miniputt touche au cercle à son milieu
/// </summary>
public class Sauteur : MonoBehaviour
{
    [SerializeField]
    private float forcePropulsion;
    /// <summary>
    /// Méthode qui se lance dès que la boîte de déchenchement liée au cercle de la plaque détecte un autre gameObject
    /// </summary>
    /// <param name="other">Le gameObject détecté</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BalleGolf balle))
        {
            //Propulser la balle par le haut de la plaque par la force désignée précédement
            other.attachedRigidbody.AddForce(transform.up * forcePropulsion, ForceMode.Impulse);
        }
    }
}
