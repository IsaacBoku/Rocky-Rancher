using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interacion : MonoBehaviour
{
    Player_Move player;
    public ItemData_Oficial _item;
    #region Selected
    Wood_Select selectedLand = null;
    Land_Farm selectedDirt = null;
    Interacion_Objetos selectedObjetc = null;
    #endregion
    private void Start()
    {
        player = transform.parent.GetComponent<Player_Move>();

    }
    private void Update()
    {
        RaycastHit hitWood;
        if (Physics.Raycast(transform.position, Vector3.down, out hitWood, 1))
            OnInteractableWood(hitWood);
        RaycastHit hitDirt;
        if (Physics.Raycast(transform.position, Vector3.down, out hitDirt, 1))
            OnInteractableDirt(hitDirt);
    }
    #region Interactable
    private void OnInteractableDirt(RaycastHit dirtHit)
    {
        Collider other = dirtHit.collider;
        if (other.tag == "Dirt")
        {
            Land_Farm dirts = other.GetComponent<Land_Farm>();
            SelectedDirt(dirts);
            return;
        }
        if (other.tag == "Item")
        {
            selectedObjetc = other.GetComponent<Interacion_Objetos>();
            return;
        }
        if (selectedObjetc != null)
        {
            selectedObjetc = null;return;
        }
        if (selectedDirt != null)
        {
            selectedDirt.SelectDirt(false);
            selectedDirt = null;
        }
    }
    private void OnInteractableWood(RaycastHit woodHit)
    {
        Collider other = woodHit.collider;

        if (other.tag == "Wood")
        {
            Wood_Select woods = other.GetComponent<Wood_Select>();
            SelectWood(woods);
            return;

        }
        if (selectedLand != null)
        {
            selectedLand.Select(false);
            selectedLand = null;
        }
    }
    #endregion
    #region Selected
    public void SelectedDirt(Land_Farm dirt)
    {
        if (selectedDirt != null)
            selectedDirt.SelectDirt(false);
        selectedDirt = dirt;
        dirt.SelectDirt(true);
    }
    public void SelectWood(Wood_Select wood)
    {
        if (selectedLand != null)
            selectedLand.Select(false);
        selectedLand = wood;
        wood.Select(true);
    }
    #endregion
    #region Interacts
    public void InteractItems()
    {
        if (selectedLand != null)
        {
            selectedLand.InteractWood(); return;
        }
        if (selectedDirt != null)
        {
            selectedDirt.InteractDirt(); return;
        }
    }
    public void InteractSeed()
    {
        if (selectedDirt != null)
        {
            selectedDirt.SeedToolsCrop(); return;
        }
    }
    public void InteractObject()
    {
        if (selectedObjetc != null)
            selectedObjetc.Pickup();
    }
    #endregion
}
