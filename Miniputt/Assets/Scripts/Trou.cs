using UnityEngine;
/// <summary>
/// Script gérant les collisions dans le trou/but du parcours de miniputt.
/// </summary>
public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField, Tooltip("Point de départ de la balle")]
    private GameObject pointDepart;

    private Vector3 positionDepart;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { //Trouver la position du point de départ
        positionDepart = pointDepart.transform.position;
    }
    /// <summary>
    /// Fonction déclenchée lorsqu'un objet atteint le trou. Permet de guider la balle vers son prochain point.
    /// </summary>
    /// <param name="other">L'objet atteignant le trou.</param>
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger déclenché, on essait de téléporter le gameObject...");
        //Si la balle est dans le trou, on la téléporte au point de départ
        if (other.TryGetComponent(out BalleGolf balle) && other.TryGetComponent(out Rigidbody rigidbody))
        {
            
            //Selon Google Gemini, généré le 4 Septembre 2026
            other.attachedRigidbody.transform.position = positionDepart;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
