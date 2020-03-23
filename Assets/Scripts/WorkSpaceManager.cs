using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using CognitiveReality.Source.UI;
using SimpleFileBrowser;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using UserInterface.Source.UI;

public class WorkSpaceManager : MonoBehaviour
{
    [SerializeField] public EnhancedToggleGroup toggleGroup;
    [SerializeField] public GameObject blockToggle;
    [SerializeField] public GameObject gcodeToggle;
    [SerializeField] public GameObject block;
    [SerializeField] public GameObject gcode;
    [SerializeField] public GameObject group;
    [SerializeField] public GameObject canvas;
    [SerializeField] public List<GameObject> listBlocks;
    private GameObject content;
    private Dictionary<int, GameObject> dictionaryBlock = new Dictionary<int, GameObject>();
    [SerializeField] public MultipanelController mpc;

    private struct ArrayDescription
    {
        public Description[] description;
    }

    [Button]
    public void OpenGcodeProgramme()
    {
        var newGcodeToggle = Instantiate(gcodeToggle, group.transform);
        var enhancedToggle = newGcodeToggle.GetComponent<EnhancedToggle>();
        enhancedToggle.onToggleClick.AddListener(() => mpc.SetActivePanel(mpc.panels.Length - 1));
        newGcodeToggle.SetActive(true);

        var newGcode = Instantiate(gcode, canvas.transform);
        // newGcode.SetActive(true);
        content = newGcodeToggle;
        mpc.AddPanel(newGcode.GetComponent<RectTransform>());
        //StartCoroutine(ShowLoadBlock());
    }
    
    [Button]
    public void OpenBlockProgramme()
    {
        var newBlockToggle = Instantiate(blockToggle, group.transform, true);
        var enhancedToggle = newBlockToggle.GetComponent<EnhancedToggle>();
        enhancedToggle.onToggleClick.AddListener(() => mpc.SetActivePanel(mpc.panels.Length - 1));
        newBlockToggle.SetActive(true);


        var newBlock = Instantiate(block, canvas.transform);
        content = newBlock;
        mpc.AddPanel(newBlock.GetComponent<RectTransform>());
        //StartCoroutine(ShowLoadBlock());
    }

    [Button]
    public void CloseProgramme(int index)
    {
        mpc.DeletePanel(index);
        var toggles = group.GetChilds();
        var programmes = canvas.GetChilds();
        Destroy(toggles[index].gameObject);
        Destroy(programmes[index].gameObject);
    }

    private void Start()
    {
        // FileBrowser.SetFilters(true, new FileBrowser.Filter("BlockProgramming", ".json"),
        //     new FileBrowser.Filter("Gcode", ".gc"));
        // FileBrowser.SetDefaultFilter(".gc");

        foreach (var value in listBlocks)
            dictionaryBlock.Add(value.GetComponent<ICodeBlock>().GetCommandID(), value);
    }
}