using System.Collections.Generic;
using Sirenix.OdinInspector;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UserInterface.Source.UI;

public class TabsManager : MonoBehaviour
{
    [SerializeField] public GameObject prefabBlock;
    [SerializeField] public GameObject prefabBlockTabs;
    [SerializeField] public GameObject prefabCode;
    [SerializeField] public GameObject prefabCodeTabs;
    [SerializeField] public GameObject tabsSpace;
    [SerializeField] public GameObject plusTabsButton;
    [SerializeField] public MultipanelController panelsController;
    [SerializeField] public GameObject programmeSpace; 
    private Button closeTab;
    public List<GameObject> listTabs = new List<GameObject>();
    public List<GameObject> listProgramme = new List<GameObject>();

    [Button]
    public void CreateTabsBlock()
    {
        var newTabs = Instantiate(prefabBlockTabs, tabsSpace.transform);
        newTabs.SetActive(true);
        plusTabsButton.transform.SetAsLastSibling();
        

        var toggle = newTabs.GetComponent<TogglePlus>();
        toggle.group = tabsSpace.GetComponent<ToggleGroup>();
        var index = panelsController.panels.Length;
        toggle.onToggleClick.AddListener(() => panelsController.SetActivePanel(index));


        var newBlock = Instantiate(prefabBlock, programmeSpace.transform);
        newBlock.SetActive(false);
        panelsController.AddPanel(newBlock.GetComponent<RectTransform>());
        panelsController.ResetOffset(newBlock.GetComponent<RectTransform>());
        
        listProgramme.Add(newBlock);
        listTabs.Add(newTabs);

        var index2 = listTabs.Count - 1;
        closeTab = newTabs.GetComponentInChildren<Button>();
        closeTab.onClick.AddListener(() => CloseTab(index2));
    }
    
    [Button]
    public void CreateTabsCode()
    {
        var newTabs = Instantiate(prefabCodeTabs, tabsSpace.transform);
        newTabs.SetActive(true);
        plusTabsButton.transform.SetAsLastSibling();

        var toggle = newTabs.GetComponent<TogglePlus>();
        toggle.group = tabsSpace.GetComponent<ToggleGroup>();
        var index = panelsController.panels.Length;
        toggle.onToggleClick.AddListener(() => panelsController.SetActivePanel(index));


        var newCode = Instantiate(prefabCode, programmeSpace.transform);
        newCode.SetActive(false);
        panelsController.AddPanel(newCode.GetComponent<RectTransform>());
        panelsController.ResetOffset(newCode.GetComponent<RectTransform>());
        
        listProgramme.Add(newCode);
        listTabs.Add(newTabs);
        
        closeTab = newTabs.GetComponentInChildren<Button>();
        var index2 = listTabs.Count - 1;
        Debug.Log(index2);
        closeTab.onClick.AddListener(() => CloseTab(index2));
    }

    public void CloseTab(int index)
    {
        Destroy(listProgramme[index]);
        Destroy(listTabs[index]);
        listProgramme.RemoveAt(index);
        listTabs.RemoveAt(index);
        index += 2;
        panelsController.DeletePanel(index);
    }
    
}