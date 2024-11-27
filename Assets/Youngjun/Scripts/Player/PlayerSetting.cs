using InnerSight_Seti;
using UnityEngine;


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

    private void Awake()
    {
        playerStates = new(this);
        // 유틸리티 인스턴스화
        cursorUtility = new(this);
        playerUse = GetComponent<PlayerUse>();
        playerTrade = GetComponent<PlayerTrade>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
