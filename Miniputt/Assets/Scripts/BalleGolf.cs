using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Script gérant le déplacement de la balle de golf
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class BalleGolf : MonoBehaviour
{
    [SerializeField]
    private float forceBalle = 10.0f;
    [SerializeField]
    private PlayerInput controles;

    [SerializeField]
    private Rigidbody rigidBody;

    private Vector2 deplacements;
    /// <summary>
    /// Méthode lancée lors de l'initialisation de la balle de golf
    /// </summary>
    private void Awake()
    {
        //Obtenir le rigidbody de la balle de golf afin de gérer ses déplacements plus tard
        rigidBody = GetComponent<Rigidbody>();
    }
    /// <summary>
    /// Méthode lancée lors de la boucle de jeu, lorsque le gameobject est déjà initialisé et entrant en fonction
    /// </summary>
    void Start()
    {
        //Trouver et lier l'action à un événement C#
        InputAction actionBouger = controles.actions.FindAction("player/FrapperBalle");
        actionBouger.performed += CommencerBouger;
        //actionBouger.canceled += CommencerBouger;
    }
    /// <summary>
    /// Méthode lancée lorsque l'événement actionBouger est performée
    /// </summary>
    /// <param name="contexte">Contexte de l'action appelant l'événement</param>
    private void CommencerBouger(InputAction.CallbackContext contexte)
    {
        //Pour les gameobjects avec rigidbody non kinématique, il faut la fonction AddForce pour faire le déplacement
        rigidBody.AddForce(forceBalle, 0.0f, 0.0f);
        //Debug.Log("Le deplacement est supposé se faire");
    }
    //Méthode utilisée prédécement lorsque les contrôles faisaient rouler la balle, pas frapper la balle
    //private void ArreterBouger(InputAction.CallbackContext contexte)
    //{
    //    deplacements = Vector2.zero;
    //}
    /// <summary>
    /// Méthode lancée lorsque l'on détruit la balle.
    /// </summary>
    private void OnDestroy() //Éviter les fuites de mémoire!
    {
        //Désabonner notre méthode observateur de l'événement avant de détruire le gameobject (fuite de mémoire sinon)
        InputAction actionBouger = controles.actions.FindAction("player/FrapperBalle");
        actionBouger.performed -= CommencerBouger;
    }

    // Update is called once per frame
    void FixedUpdate() //parce que c'est un rigidbody.
    {
        
    }
}
