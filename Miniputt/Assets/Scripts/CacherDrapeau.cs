using UnityEngine;
/// <summary>
/// Script concernant la visibilité du drapeau lorsque la balle s'en approche
/// </summary>
public class CacherDrapeau : MonoBehaviour
{
    [SerializeField, Tooltip("Le drapeau qui apparait ou disparait")]
    private GameObject drapeau;
    /// <summary>
    /// Méthode qui se lance dès la première boucle de jeu
    /// </summary>
    private void Start()
    {
        drapeau.SetActive(true);
    }
    /// <summary>
    /// Méthode qui se lance dès qu'une entrée est détectée dans un collider en trigger
    /// </summary>
    /// <param name="other">Le gameObject qui est entré dans la zone de trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out BalleGolf balle))
        {
            drapeau.SetActive(false);
        }
    }
    /// <summary>
    /// Méthode qui se lance lorsqu'un gameObject sors d'une zone de collision en trigger
    /// </summary>
    /// <param name="other">Le gameObject sortant de la zone de collision en trigger</param>
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out BalleGolf balle))
        {
            drapeau.SetActive(true);
            //Debug.Log("le drapeau doit être visible maintenant");
        }
    }
}
