using System.Collections.Generic;
using System.Linq;
using Interfaces;
using UI;
using UnityEngine;

public class BlockHighLighter : MonoBehaviour, ILineHighlighter
{
    [SerializeField] public GameObject content;
    private List<CodeBlock> blocks = new List<CodeBlock>();


    public void SetLineColor(int lineIndex)
    {
        blocks[lineIndex].SetSelectionColor();
    }

    public void ResetColorLines()
    {
        foreach (var block in blocks)
            block.SetMainColor();
    }

    public void Refresh()
    {
        blocks = content.GetComponentsInChildren<CodeBlock>().ToList();
    }
}