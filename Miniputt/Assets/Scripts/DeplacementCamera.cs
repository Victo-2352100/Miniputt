using UnityEngine;
using UnityEngine.InputSystem;

public class DeplacementCamera : MonoBehaviour
{
    [SerializeField]
    private float vitesseDeplacement = 5.0f;
    [SerializeField]
    private PlayerInput controles;

    private Vector2 deplacement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("ALLOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
        InputAction actionDeplacement = controles.actions.FindAction("Player/DeplacerCamera");
        actionDeplacement.performed += CommencerDeplacement;
        actionDeplacement.canceled += ArreterDeplacement;
    }
    private void CommencerDeplacement(InputAction.CallbackContext contexte)
    {
        deplacement = contexte.ReadValue<Vector2>().normalized;
    }
    private void ArreterDeplacement(InputAction.CallbackContext contexte)
    {
        deplacement = Vector2.zero;
    }

    private void GererDeplacement() {
        if (deplacement.sqrMagnitude > 0.0f) //Si on se déplace réellement
        {
            Debug.Log("Deplacements en cours(?)");
            Vector3 positionDeplacement = transform.forward * deplacement.y * Time.deltaTime
            + transform.right * deplacement.x * Time.deltaTime;
            deplacement.Normalize();
            deplacement *= vitesseDeplacement * Time.deltaTime;
            transform.position += positionDeplacement;
        }
    }

    private void OnDestroy()
    {
        if (controles == null) return;
        InputAction actiondeplacement = controles.actions.FindAction("Player/DeplacerCamera");
        actiondeplacement.performed -= CommencerDeplacement;
        actiondeplacement.canceled -= ArreterDeplacement;

    }
    // Update is called once per frame
    void Update()
    {
        GererDeplacement();
    }
}
