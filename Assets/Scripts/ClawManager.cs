using Interfaces;
using RootMotion.FinalIK;
using TMPro;
using UnityEngine;

public class ClawManager : MonoBehaviour
{
    [SerializeField] public GameObject[] Claws;
    [SerializeField] public GameObject Manipulator;
    [SerializeField] public TMP_Dropdown dropDownClaws;
    [SerializeField] public Manipulator.Manipulator ManipulatorClaw;
    private const int indexClawBone = 5;
    public GameObject ActiveClaw;
    private void Start()
    {
        dropDownClaws.onValueChanged.AddListener(delegate { ClawsChanged(); });
    }

    private void ClawsChanged()
    {
        var indexActiveClaw = dropDownClaws.value;
        var ccdik = Manipulator.GetComponent<CCDIK>();
        ccdik.solver.bones[indexClawBone] = new IKSolver.Bone(Claws[indexActiveClaw].GetComponent<IClaw>().GetTarget());
        foreach (var claw in Claws)
            claw.SetActive(false);
        Claws[indexActiveClaw].SetActive(true);
        ManipulatorClaw.Claw = Claws[indexActiveClaw];
    }
}