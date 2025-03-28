using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class UI : MonoBehaviour
{

    [SerializeField] private GameObject characterUI;
    [SerializeField] private GameObject craftUI;
    [SerializeField] private GameObject optionsUI;
    [SerializeField] private GameObject inGameUI;
    [SerializeField] private GameObject villagersMenu;
    [SerializeField] private GameObject map;


    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_CraftWindow craftWindow;

    private void Start()
    {
        SwitchTo(inGameUI);
        Time.timeScale = 1f;
        Cursor.visible = false;
        

        statToolTip.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            SwitchWithKeyTo(characterUI);
            UIManager.Instance.ToggleRelationshipPanel();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitchWithKeyTo(craftUI);
            
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            SwitchWithKeyTo(optionsUI);
            UIManager.Instance.ToggleRelationshipPanel();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            SwitchWithKeyTo(map);
            UIManager.Instance.ToggleRelationshipPanel();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            SwitchWithKeyTo(villagersMenu);
            UIManager.Instance.ToggleRelationshipPanel();
          
           
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            CerrarVentana(map);
            CerrarVentana(craftUI);
            CerrarVentana(villagersMenu);
            CerrarVentana(characterUI);
            CerrarVentana(optionsUI);
        }


    }
    public void SwitchTo(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }

        if (_menu != null)
        {
            _menu.SetActive(true);
            
        }
        
    }

    public void SwitchWithKeyTo(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);
         
            CheckForIngameUI();
            

            return;
        }
        SwitchTo(_menu);
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
    }
    private void CheckForIngameUI()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).gameObject.activeSelf)

            return;
        }
        
            Time.timeScale = 1;
        Cursor.visible = false;
        SwitchTo(inGameUI);
    }

    public void CerrarVentana(GameObject _menu)
    {
        if (_menu != null && _menu.activeSelf)
        {
            _menu.SetActive(false);

            CheckForIngameUI();

            return;
        }
    }
}
