using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameObject playerInstance;      // reference to the player GameObject
    private PlayerController playerController;
    private Weapon playerWeapon;
    public static GameController Instance; // The global reference

    public Armour armourClass = new Armour(); // run all damage through this armour deduction filter 
    

    [Header("Player")]
    public GameObject playerPrefab;

    public Transform playerSpawnPoint;

    [Header("Camera")]
    public CinemachineCamera virtualCamera;

    [Header("Weapons / Inventory")]
    public WeaponDatabaseGame weaponDataBaseGame;
    public Inventory inventory;

    public GameObject weaponBox;
    public GameObject healBox;

    [Header("UI")]
    public UIManager uiManager;


    // map grid size
    // 24 (h) x 44 (w)

    void Awake()
    {
        Instance = this; // Set the GameController reference 
    }

    void Start()
    {
        try
        {
            Debug.Log("Start in GameController.........................................");
            playerInstance = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity); // set GameObject for player
            Debug.Log($"playerinstance is: {playerInstance}");
            playerController = playerInstance.GetComponent<PlayerController>(); // set PlayerController for reference
            playerWeapon = playerInstance.GetComponentInChildren<Weapon>(); // set playerWeapon for reference
        }
        catch (System.Exception e)
        {
            Debug.Log($"Error in GameController Start: {e}");
            throw;
        }
    
        


        // Tell Cinemachine to track this new instance
        if (virtualCamera != null)
        {
            virtualCamera.Follow = playerInstance.transform;
            virtualCamera.LookAt = playerInstance.transform;
        }

        // equip player with starter weapon...
        WeaponInstance firstWeaponInstance = weaponDataBaseGame.GetStarterWeapon(); // get weapon instance
        inventory.WeaponPickUp(firstWeaponInstance); // add to inventory
        playerWeapon.Equip(firstWeaponInstance); // equip player with weapon
        uiManager.UpdateCurrentWeaponDisplay(firstWeaponInstance.weaponData.weaponSprite); // update ui
        
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
    public void AddPlayerDebuffUI(WeaponDebuffData data)
    {
        // give to UI Manager
        uiManager.AddEffect(data);
    }

    public void RemovePlayerDebuffUI(WeaponDebuffData data)
    {
        uiManager.RemoveEffect(data);
    }



    public void PlayerDamaged(float currentHealth)
    {
        Debug.Log($"Player took damage: {currentHealth}");
        // player is dead
        if(currentHealth <= 0f)    
        {
            Debug.Log("Player died.......................");
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

        // if heal, then heal player...
        if (weaponInstance.weaponData.weaponName == "PlayerHeal")
        {
            AudioManager.Instance.PlayAudioClip(AudioKey.HealCratePickUp);
            playerController.HealPlayer(); // player heals 20 health

        }
        // if weapon add to inventory
        else
        {
            AudioManager.Instance.PlayAudioClip(AudioKey.WeaponCratePickUp);
            inventory.WeaponPickUp(weaponInstance); // add new weapon instance to inventory...
        }

    }
}
