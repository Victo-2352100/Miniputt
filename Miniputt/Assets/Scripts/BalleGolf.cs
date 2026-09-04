using UnityEngine;
using UnityEngine.InputSystem;

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
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }
    void Start()
    {
        InputAction actionBouger = controles.actions.FindAction("player/FrapperBalle");
        actionBouger.performed += CommencerBouger;
        //actionBouger.canceled += CommencerBouger;
    }
    private void CommencerBouger(InputAction.CallbackContext contexte)
    {
        rigidBody.AddForce(forceBalle, 0.0f, 0.0f);
        Debug.Log("Le deplacement est supposé se faire");
    }
    //private void ArreterBouger(InputAction.CallbackContext contexte)
    //{
    //    deplacements = Vector2.zero;
    //}
    private void GererBouger()
    {
        
    }

    private void OnDestroy() //Éviter les fuites de mémoire!
    {
        InputAction actionBouger = controles.actions.FindAction("player/FrapperBalle");
        actionBouger.performed -= CommencerBouger;
    }

    // Update is called once per frame
    void FixedUpdate() //parce que c'est un rigidbody.
    {
        GererBouger();
    }
}
