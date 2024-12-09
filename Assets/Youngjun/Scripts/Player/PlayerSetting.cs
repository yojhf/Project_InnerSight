using InnerSight_Seti;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class PlayerSetting : MonoBehaviour
{
    public PlayerStates playerStates;
    private PlayerTrade playerTrade;
    private PlayerUse playerUse;
    // 유틸리티
    private CursorUtility cursorUtility;
    private PlayerInteraction playerInteraction;
    public PlayerUse PlayerUse => playerUse;
    public PlayerTrade PlayerTrade => playerTrade;
    public CursorUtility CursorUtility => cursorUtility;
    public PlayerInteraction PlayerInteraction => playerInteraction;

    public XRRayInteractor rayInteractor;
        
    private void Awake()
    {
        playerStates = new(this);
        // 유틸리티 인스턴스화
        cursorUtility = new(this);
        playerUse = GetComponent<PlayerUse>();
        playerTrade = GetComponent<PlayerTrade>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (transform.GetChild(0).position.y <= 1.35f)
        {
            PlayerPos();
        }
    }

    void PlayerPos()
    {
        transform.GetChild(0).position = new Vector3(0f, 1.36f, 0f);
    }


}
