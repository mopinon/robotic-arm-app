using System.Collections;
using System.Collections.Generic;
using System.IO;
using Interfaces;
using Parsers;
using SerialPortDirectory;
using SimpleFileBrowser;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FileManager : MonoBehaviour
{
    private BlockLoadEvent onBlockLoadEvent = new BlockLoadEvent();
    private CodeLoadEvent onCodeLoadEvent = new CodeLoadEvent();
    public UnityEvent onProgrammeSave = new UnityEvent(); 
    [SerializeField] public Button openButton;
    [SerializeField] public Button newBlockButton;
    [SerializeField] public Button newGCodeButton;
    [SerializeField] public GameObject blockTemplate;
    [SerializeField] public GameObject contentBlockTemplate;
    [SerializeField] public GameObject gcodeTemplate;
    [SerializeField] public TMP_InputField gcodeInput;
    [SerializeField] public TabControl tabControl;
    [SerializeField] public Button saveAsButton;
    [SerializeField] public Button saveAllButton;
    [SerializeField] public Button exitButton;
    [SerializeField] public List<GameObject> listBlocks;
    [SerializeField] public Sprite iconBlock;
    [SerializeField] public Sprite iconGcode;
    [SerializeField] public WriteSerial writeSerial;
    private Dictionary<int, GameObject> dictionaryBlock = new Dictionary<int, GameObject>();

    private int numberNewFiles = 1;

    private void Awake()
    {
        onBlockLoadEvent.AddListener((x, title, icon) => { onCodeLoadEvent.Invoke(x, title, icon); });

        onCodeLoadEvent.AddListener((x, title, icon) =>
        {
            var tab = tabControl.AddTab();
            tab.Title = title;
            tab.Icon = icon;
            var page = tabControl.AddPage();
            page.Hide();

            page.SetContent(x.GetComponent<RectTransform>());
            tabControl.BindPageToTap(tab, page);

            tab.Show().Select();
            page.Show();
            x.GetComponent<RectTransform>().localScale = Vector3.one;
        });
    }

    private struct ArrayDescription
    {
        public Description[] description;
    }

    private void Start()
    {
        FileBrowser.SetFilters(false,
            new FileBrowser.Filter("Gcode", ".gc"),
            new FileBrowser.Filter("Gblock", ".gb"));
        exitButton.onClick.AddListener(writeSerial.CloseSerialPort);
        openButton.onClick.AddListener(() => StartCoroutine(ShowLoad()));
        newBlockButton.onClick.AddListener(OpenBlock);
        newGCodeButton.onClick.AddListener(OpenGcode);
        saveAllButton.onClick.AddListener(() => StartCoroutine(SaveAll()));
        saveAsButton.onClick.AddListener(() => StartCoroutine(ShowSaveAs()));
        exitButton.onClick.AddListener(Quit);
        foreach (var block in listBlocks)
            dictionaryBlock.Add(block.GetComponent<ICodeBlock>().GetCommandID(), block);
    }

    private void OpenBlock()
    {
        var childs = contentBlockTemplate.GetChilds();
        foreach (var child in childs)
            Destroy(child.gameObject);
        var name = "NewFile";
        if (numberNewFiles != 1)
            name += numberNewFiles.ToString();
        numberNewFiles++;
        onBlockLoadEvent.Invoke(Instantiate(blockTemplate), name, iconBlock);
    }

    private void OpenGcode()
    {
        gcodeInput.text = "";
        var name = "NewFile";
        if (numberNewFiles != 1)
            name += numberNewFiles.ToString();
        numberNewFiles++;
        onCodeLoadEvent.Invoke(Instantiate(gcodeTemplate), name, iconGcode);
    }

    private IEnumerator ShowLoad()
    {
        FileBrowser.SetFilters(false,
            new FileBrowser.Filter("Gcode", ".gc"),
            new FileBrowser.Filter("Gblock", ".gb"));
        if (!FileBrowser.ShowLoadDialog(OnSuccessLoad, null, false, null, "Load"))
            yield break;
        yield return null;
    }

    private void OnSuccessLoad(string path)
    {
        var sw = new StreamReader(path);
        var str = sw.ReadToEnd();
        var nameFile = GetNameFileFromPath(path);
        if (path.EndsWith(".gb"))
        {
            var childs = contentBlockTemplate.GetChilds();
            foreach (var child in childs)
                Destroy(child.gameObject);

            var wrapper = JsonUtility.FromJson<ArrayDescription>(str);
            foreach (var description in wrapper.description)
            {
                var originalBlock = dictionaryBlock[description.id];
                var block = Instantiate(originalBlock, contentBlockTemplate.transform, true);
                block.GetComponent<ICodeBlock>().SetParameters(description.parameters);
                block.SetActive(true);
            }

            onBlockLoadEvent.Invoke(Instantiate(blockTemplate), nameFile, iconBlock);
        }
        else
        {
            gcodeInput.text = str;

            onCodeLoadEvent.Invoke(Instantiate(gcodeTemplate), nameFile, iconGcode);
        }

        sw.Close();
    }

    private IEnumerator ShowSaveAs()
    {
        if (tabControl.selectedTab != null)
        {
            var parser = tabControl.selectedPage.GetComponentInChildren<IParser>();
            if (parser is GCodeParser)
                FileBrowser.SetFilters(false,
                    new FileBrowser.Filter("Gcode", ".gc"));
            if (parser is BlockParser)
                FileBrowser.SetFilters(false,
                    new FileBrowser.Filter("Gblock", ".gb"));
            if (!FileBrowser.ShowSaveDialog((path)
                    => OnSuccessSaveAs(path, parser, tabControl.selectedTab),
                null, false, null, "Save " + tabControl.selectedTab.Title))
                yield break;
            yield return null;
        }

        yield return null;
    }

    private void OnSuccessSaveAs(string path, IParser page, Tab tab)
    {
        var sw = new StreamWriter(path, false);
        var parser = page;
        var text = parser.GetContentForSave();
        sw.Write(text);
        sw.Close();
        tab.Title = GetNameFileFromPath(FileBrowser.Result);
        onProgrammeSave.Invoke();
    }

    private IEnumerator SaveAll()
    {
        var pages = tabControl.pages;
        var tabs = tabControl.tabs;
        for (var i = 0; i < pages.Count; i++)
        {
            var page = pages[i];
            var tab = tabs[i];
            if (page != null)
            {
                var parser = page.GetComponentInChildren<IParser>();
                if (parser is GCodeParser)
                    FileBrowser.SetFilters(false,
                        new FileBrowser.Filter("Gcode", ".gc"));
                if (parser is BlockParser)
                    FileBrowser.SetFilters(false,
                        new FileBrowser.Filter("Gblock", ".gb"));
                yield return FileBrowser.WaitForSaveDialog(false, null, "Save " + tab.Title);
                OnSuccessSaveAs(FileBrowser.Result, parser, tab);
            }
        }
    }

    private void Quit() => Application.Quit();

    string GetNameFileFromPath(string path) =>
        path.Substring(path.LastIndexOf('\\') + 1, path.LastIndexOf('.') - path.LastIndexOf('\\') - 1);

    public class BlockLoadEvent : UnityEvent<GameObject, string, Sprite>
    {
    }

    public class CodeLoadEvent : UnityEvent<GameObject, string, Sprite>
    {
    }
}