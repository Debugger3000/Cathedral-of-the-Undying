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

    public GameObject weaponBox;

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
        uiManager.UpdateCurrentWeaponDisplay(firstWeaponInstance.weaponData.weaponSprite); // update ui
        // equip shotgun too for testing
        // WeaponInstance shotgunInstance = weaponDataBaseGame.GetShotgun();
        // inventory.WeaponPickUp(shotgunInstance);
        // playerWeapon.Equip(shotgunInstance); // equip player with weapon
        // uiManager.UpdateCurrentWeaponDisplay(shotgunInstance.weaponData.weaponSprite);
    }

    // Weapon functions
    public void SwitchWeapon()
    {
        // rotate inventory ahead by 1
        WeaponInstance newWeapon = inventory.SwapWeapons(); // rotate inventory / weapon
        playerWeapon.Equip(newWeapon); // equip player with new weapon
        uiManager.UpdateCurrentWeaponDisplay(newWeapon.weaponData.weaponSprite); // update UI with new weapon sprite
    }


    // UI
    public void PlayerDamaged(float currentHealth)
    {
        // player is dead
        if(currentHealth <= 0f)    
        {
            Destroy(playerInstance); // kill player model...
            // player dead
            // have a nice delay and the enemies swarming..
            // cue death music / text saying player has died...
            // then pop up end game menu...
        }
        uiManager.UpdatePlayerHealth(currentHealth); // update player UI health bar
    }

    // weapon box drop
    public WeaponName GetWeaponBoxDropName()
    {
        // get level and get a weapon Name ( ID )
        int level = PointMultiplier.Instance.multiplierLevel;
        return weaponDataBaseGame.GetWeaponDrop(level); // pass weapon id back to box
    }

    public void PickUpWeaponBox(WeaponName weaponName)
    {
        Debug.Log($"Player is picking up weaon in GAMECONTROLLER: {weaponName}");
        WeaponInstance weaponInstance = weaponDataBaseGame.GetWeaponPickUp(weaponName); // get weapon instance..
        inventory.WeaponPickUp(weaponInstance); // add new weapon instance to inventory...
    }
}
