using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Script assurant la gestion du déplacement de la caméra-dieu
/// </summary>
public class DeplacementCamera : MonoBehaviour
{
    //Vitesses modifiables de la caméra
    [SerializeField]
    private float vitesseDeplacement = 5.0f;
    [SerializeField]
    private float vitesseRotationX = 15.0f;
    [SerializeField]
    private float vitesseRotationY = 15.0f;
    [SerializeField]
    private float vitesseZoom = 5.0f;
    //Contrôles et confins de la caméra assignables
    [SerializeField]
    private PlayerInput controles;
    [SerializeField, Tooltip("Volume qui confine la camera")]
    private BoxCollider collider;
    //Limitations des déplacements de la caméra
    [SerializeField, Tooltip("Limites de rotation selon les angles (x, y, z)")]
    private Vector2 LimitesRotation;
    private Vector2 deplacement;
    [SerializeField]
    private Vector2 limitesZoom;

    [SerializeField, Tooltip("La caméra-dieu")] //La camera suivi ne devrait pas être nécessaire pour le zoom ou rotation
    private CinemachineCamera cameraGereeDieu;

    //Angle de la rotation
    private float rotationX;
    private float rotationY;

    //Valeur actuelle du zoom de la caméra (distance comparé à cible)
    private float zoom;
    /// <summary>
    /// Méthode lancée lorsque le jeu est lancé.
    /// </summary>
    void Start()
    {
        //Initialisation de l'action de déplacement de la caméra et création des événements C# liés
        InputAction actionDeplacement = controles.actions.FindAction("player/DeplacerCamera");
        actionDeplacement.performed += CommencerDeplacement;
        actionDeplacement.canceled += ArreterDeplacement;
        //Initialisation du contrôle de rotation de l'axe des X (inclinaison) et création des événements liés
        InputAction actionRotationX = controles.actions.FindAction("player/RotationX");
        actionRotationX.performed += CommencerRotationX;
        actionRotationX.canceled += ArreterRotationX;
        //Initialisation du contrôle de rotation de l'axe des Y et création des événements liés
        InputAction actionRotationY = controles.actions.FindAction("player/RotationY");
        actionRotationY.performed += CommencerRotationY;
        actionRotationY.canceled += ArreterRotationY;
        //Initialisation du contrôles de zoom et création des événements liés
        InputAction actionZoomCamera = controles.actions.FindAction("player/ZoomCamera");
        actionZoomCamera.performed += CommencerZoom;
        actionZoomCamera.canceled += ArreterZoom;
    }

    /// <summary>
    /// Méthode lancée lorsqu'un action lié au déplacement de la caméra est performés
    /// </summary>
    /// <param name="contexte">Action liée au déplacement qui a déclenché l'événement</param>
    private void CommencerDeplacement(InputAction.CallbackContext contexte)
    {
        //On mets les valeurs de base du déplacement (seront multiplié par la vitesse et le temps)
        deplacement = vitesseDeplacement * contexte.ReadValue<Vector2>().normalized;
    }
    /// <summary>
    /// Méthode lancée lorsque l'action gérant le déplacement de la caméra est annulé (relâchement de la touche)
    /// </summary>
    /// <param name="contexte"></param>
    private void ArreterDeplacement(InputAction.CallbackContext contexte)
    {
        //On mets toutes les valeurs de déplacement à zéro.
        deplacement = Vector2.zero;
    }

    /// <summary>
    /// Méthode lancée lorsque l'une des touches liés à la rotation sur l'axe des X est appuyée
    /// </summary>
    /// <param name="contexte"></param>
    private void CommencerRotationX(InputAction.CallbackContext contexte)
    {
        rotationX = vitesseRotationX * contexte.ReadValue<float>();
    }
    /// <summary>
    /// Méthode lancée lorsqu'on relâche les touches liés au contrôle de la rotation de l'axe des X de la caméra
    /// </summary>
    /// <param name="contexte"></param>
    private void ArreterRotationX(InputAction.CallbackContext contexte)
    {
        rotationX = 0.0f;
    }
    /// <summary>
    /// Méthode lancée lorsqu'une des touches liée à la rotation de la caméra sur l'axe des Y est appuyée
    /// </summary>
    /// <param name="contexte"></param>
    private void CommencerRotationY(InputAction.CallbackContext contexte)
    {
        rotationY = vitesseRotationY * contexte.ReadValue<float>();
    }
    /// <summary>
    /// Méthode lancée lorsqu'on relâche les touches liées à la rotation sur l'axe des Y
    /// </summary>
    /// <param name="contexte"></param>
    private void ArreterRotationY(InputAction.CallbackContext contexte)
    {
        rotationY = 0.0f;
    }
    /// <summary>
    /// Méthode qui se lance lorsque l'utilisateur fait rouler la roulette de sa souris pour effectuer un zoom
    /// </summary>
    /// <param name="contexte">Valeur engendrée par le roulement de la roulette (positif si roule par le haut, négatif si c'est le bas)</param>
    private void CommencerZoom(InputAction.CallbackContext contexte)
    {
        zoom = vitesseZoom * contexte.ReadValue<float>();
    }
    /// <summary>
    /// Méthode qui se lance lorsque l'utilisateur cesse de faire rouler la roulette de sa souris pour mettre fin au zoom
    /// </summary>
    /// <param name="contexte">Valeur engendrée par la roulette de la souris</param>
    private void ArreterZoom(InputAction.CallbackContext contexte)
    {
        zoom = 0.0f;
    }
    /// <summary>
    /// Méthode lancée pour effectuer les déplacements sur la caméra selon des coordonnées
    /// </summary>
    private void GererDeplacement() {
        if (deplacement.sqrMagnitude > 0.0f) //Si on se déplace réellement
        {
            //Variable calculant la différence de déplacement
            Vector3 positionDeplacement = transform.forward * deplacement.y * Time.deltaTime
            + transform.right * deplacement.x * Time.deltaTime;
            deplacement.Normalize();
            deplacement *= vitesseDeplacement; //Multiplier par la vitesse de déplacement
            //Variable pour calculer la prochaine position de la cible de la caméra
            Vector3 nouvellePosition = transform.position + positionDeplacement; 
            //Vérifier que la cible est dans les limites de terrain définies avant de déplacer ou non la caméra
            if (collider.bounds.Contains(nouvellePosition))
            {
                //Debug.Log("Voila");
                transform.position += positionDeplacement;
            }
            
        }
    }
    /// <summary>
    /// Méthode s'occupant d'appliquer la rotation de l'axe des X à la position de la caméra
    /// </summary>
    private void EffectuerRotationX()
    {
        transform.Rotate(new Vector3(0.0f, rotationX * Time.deltaTime, 0.0f), Space.World); //La rotation sur X se basera sur l'axe de l'environnement
        //Ainsi, si l'objet est incliné (/), la rotation à 180° le transformera en (\)
    }
    /// <summary>
    /// Méthode s'occupant d'appliquer la rotation de l'axe des Y à la position de la caméra
    /// </summary>
    private void EffectuerRotationY()
    {
        float angleRotationY = (transform.localEulerAngles.x + rotationY * Time.deltaTime) % 360; //(%360 pour par que la valeur de rotation dépasse 360°

        if (angleRotationY < LimitesRotation.x || angleRotationY > LimitesRotation.y)
        {
            transform.Rotate(new Vector3(rotationY * Time.deltaTime, 0.0f, 0.0f), Space.Self);
        }
    }
    /// <summary>
    /// Méthode s'occupant d'appliquer le zoom et donc de réduire ou augmenter la distance entre la cible et la caméra-dieu
    /// </summary>
    private void EffectuerZoom()
    {
        CinemachinePositionComposer positionComposer = cameraGereeDieu.GetComponent<CinemachinePositionComposer>();
        Vector3 offsetCamera = positionComposer.TargetOffset + positionComposer.TargetOffset.normalized * zoom; //On a besoin du vecteur entre les deux position. Vector = destination - départ
		float distanceOffset = offsetCamera.magnitude; //(.magnitude pour avoir seulement la distance, pas les vecteurs)

		if (distanceOffset < limitesZoom.x || distanceOffset > limitesZoom.y)
        {
            positionComposer.TargetOffset = offsetCamera;//(un + puisqu'il faut que si une des valeur du vecteur est négative, elle puisse s'appliquer normalement)

        }
    }
    /// <summary>
    /// Méthode lancée lors de la suppression de la caméra-dieu.
    /// </summary>
    private void OnDestroy()
    {
        //Vérifier si les contrôles sont déjà supprimé avant de procéder à la suppression des événements
        if (controles == null) return;
        //Trouver toutes les actions liés à des événements
        InputAction actiondeplacement = controles.actions.FindAction("player/DeplacerCamera");
        InputAction actionRotationX = controles.actions.FindAction("player/RotationX");
        InputAction actionRotationY = controles.actions.FindAction("player/RotationY");
        InputAction actionZoomCamera = controles.actions.FindAction("player/ZoomCamera");
        //Suppression des événements de mouvement
        actiondeplacement.performed -= CommencerDeplacement;
        actiondeplacement.canceled -= ArreterDeplacement;
        //Suppression des événement de rotation à l'axe X (ou inclinaison)
        actionRotationX.performed -= CommencerRotationX;
        actionRotationX.canceled -= ArreterRotationX;
        //Suppression des événements de rotation à l'axe Y
        actionRotationY.performed -= CommencerRotationY;
        actionRotationY.canceled -= ArreterRotationY;
        //Suppression des événements de zoom
        actionZoomCamera.performed -= CommencerZoom;
        actionZoomCamera.canceled -= ArreterZoom;
    }
    /// <summary>
    /// Méthode lancée à chaque image (ou frame) lors du déroulement du jeu 
    /// </summary>
    void Update()
    {
        GererDeplacement();
        EffectuerRotationX();
        EffectuerRotationY();
        EffectuerZoom();
    }
}
