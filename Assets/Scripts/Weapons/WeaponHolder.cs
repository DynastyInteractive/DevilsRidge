using Unity.Netcode;
using UnityEngine;

public class WeaponHolder : NetworkBehaviour
{
    [SerializeField] Player _player;
    [SerializeField] string _fpsOverlayLayer;

    PlayerInput.PlayerActions Input;

    int currentWeaponIndex = 0;

    void Awake()
    {
        Debug.Log("Weapon Holder: " + Time.time);
        Input = new PlayerInput().Player;
        Input.Enable();
    }

    void Start()
    {
        if (!IsOwner) return;

        Invoke(nameof(SubscribeToInventory), .2f);

        foreach (Transform weapon in transform)
        {
            weapon.gameObject.SetActive(false);
        }

        SelectWeapon(0);
    }

    void SubscribeToInventory()
    {
        Inventory.Instance.OnWeaponAdded += EquipWeapon;
        Inventory.Instance.OnWeaponRemoved += UnequipWeapon;
    }

    void Update()
    {
        if (!IsOwner) return;

        if (Input.WeaponSlot1.WasPressedThisFrame()) SelectWeapon(0);
        else if (Input.WeaponSlot2.WasPressedThisFrame()) SelectWeapon(1);
        else if (Input.WeaponScroll.ReadValue<float>() < 0)
        {
            if (currentWeaponIndex <= 0)
                currentWeaponIndex = transform.childCount - 1;
            else
                currentWeaponIndex--;
            SelectWeapon(currentWeaponIndex);
        }
        else if (Input.WeaponScroll.ReadValue<float>() > 0)
        {
            if (currentWeaponIndex >= transform.childCount - 1)
                currentWeaponIndex = 0;
            else
                currentWeaponIndex++;
            SelectWeapon(currentWeaponIndex);
        }

    }

    void SelectWeapon(int selectedWeapon)
    {
        //Debug.Log(selectedWeapon);
        if (selectedWeapon < 0 || selectedWeapon > transform.childCount || transform.GetChild(selectedWeapon).gameObject.activeSelf || transform.GetChild(selectedWeapon) == null) return;

        int i = 0;
        foreach (Transform weapon in transform)
        {
            Debug.Log(i);
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
                if (weapon.TryGetComponent(out Weapon weaponComponent)) weaponComponent.Equip(_player);
            }
            else
            {
                weapon.gameObject.SetActive(false);
                if (weapon.TryGetComponent(out Weapon weaponComponent)) weaponComponent.Unequip(_player);
            }
            i++;
        }
    }

    public void EquipWeapon(WeaponItem weapon, bool wasDropped)
    {
        var weaponGO = Instantiate(weapon.WeaponPrefab, transform);
        if (weaponGO.TryGetComponent(out Weapon weaponComponent)) weaponComponent.WeaponData = weapon;
        if (IsOwner) weaponGO.layer = LayerMask.NameToLayer(_fpsOverlayLayer);
        weaponGO.transform.localPosition = Vector3.zero;
        weaponGO.transform.localRotation = Quaternion.identity;
        SelectWeapon(transform.childCount - 1);        
    }

    public void UnequipWeapon(WeaponItem weaponItem, int weaponSlot)
    {
        Debug.Log("Unequip");
        if (transform.GetChild(weaponSlot + 1).TryGetComponent(out Weapon weaponComponent)) weaponComponent.Unequip(_player);
        Destroy(transform.GetChild(weaponSlot+1).gameObject);
    }
}
