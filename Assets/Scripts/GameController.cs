using Unity.Cinemachine;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject playerInstance;      // reference to the player GameObject
    private PlayerController playerController;
    private Weapon playerWeapon;
    public static GameController Instance; // The global reference
    

    [Header("Player")]
    public GameObject playerPrefab;

    public Transform playerSpawnPoint;

    [Header("Camera")]
    public CinemachineCamera virtualCamera;

    [Header("Weapons / Inventory")]
    public WeaponDatabaseGame weaponDataBaseGame;
    public Inventory inventory;

    [Header("UI")]
    public UIManager uiManager;

    void Awake()
    {
        Instance = this; // Set the GameController reference 
    }

    void Start()
    {
        Debug.Log("Start in GameController.........................................");
        playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity); // set GameObject for player
        playerController = playerInstance.GetComponent<PlayerController>(); // set PlayerController for reference
        playerWeapon = playerInstance.GetComponentInChildren<Weapon>(); // set playerWeapon for reference


        // Tell Cinemachine to track this new instance
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerInstance.transform;
            virtualCamera.LookAt = playerInstance.transform;
        }

        // equip player with starter weapon...
            // add to inventory
            // equip to weapon
        WeaponInstance firstWeaponInstance = weaponDataBaseGame.GetStarterWeapon();
        Debug.Log($"first weapon instance: {firstWeaponInstance}");
        inventory.WeaponPickUp(firstWeaponInstance);
        playerWeapon.Equip(firstWeaponInstance); // equip player with weapon

        // equip shotgun too for testing
        WeaponInstance shotgunInstance = weaponDataBaseGame.GetShotgun();
        inventory.WeaponPickUp(shotgunInstance);
        playerWeapon.Equip(shotgunInstance); // equip player with weapon
        uiManager.UpdateCurrentWeaponDisplay(shotgunInstance.weaponData.weaponSprite);
    }

    // Weapon functions
    public void SwitchWeapon()
    {
        // rotate inventory ahead by 1
        WeaponInstance newWeapon = inventory.SwapWeapons(); // rotate inventory / weapon
        playerWeapon.Equip(newWeapon); // equip player with new weapon
        uiManager.UpdateCurrentWeaponDisplay(newWeapon.weaponData.weaponSprite); // update UI with new weapon sprite
    }
}
