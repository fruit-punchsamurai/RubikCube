using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kociemba;
using UnityEngine.UI;

public class KociembaSolve : MonoBehaviour
{
    public ReadCube readCube;
    public CubeState cubeState;
    public PythonListener pythonListener;
    private bool doOnce = true;
    bool displaySteps = false;
    string solutionString = "";
    public Text solutionSteps;
    public static List<string> solutionList = new List<string>() { };
    public static bool showSteps = false;
    public List<string> allMoves = new List<string>() {
        "U","D","F","B","L","R",
        "U'","D'","F'","B'","L'","R'",
        "U2","D2","F2","B2","L2","R2"
    };


    // Start is called before the first frame update
    void Start()
    {
        readCube = FindObjectOfType<ReadCube>();
        cubeState = FindObjectOfType<CubeState>();
        pythonListener = FindObjectOfType<PythonListener>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CubeState.started && doOnce)
        {
            doOnce = false;
            SolveFromKociemba();
        }

    }

    public void SolveFromKociemba()
    {
        readCube.ReadState();
       
        if(!CubeState.keyMove && !CubeState.autoRotateDrag && !CubeState.drag)
        {
            displaySteps = true;
            ShowSteps();
            string moveString = cubeState.GetStateString();
            string info = "";
            solutionString = Search.solution(moveString, out  info);
            //string solution = SearchRunTime.solution(moveString, out info, buildTables: true);
            List<string> solutionList = pythonListener.StringToList(solutionString);
            keyboardControl.kociembaSolveList = solutionList;
        }
    }

    public void ShowSteps()
    {
        displaySteps = !displaySteps;
        if (displaySteps && solutionString != null)
        {
            solutionList = RephraseSolutionList(pythonListener.StringToList(solutionString));
            solutionSteps.text += solutionString+"\n";
            int count = 1;
            foreach (string solution in solutionList)
            {
                solutionSteps.text += $"{count}. {solution}\n";
                count++;
            }
        }
        else
        {
            solutionSteps.text = "";
        }
    }
 

List<string> RephraseSolutionList(List<string> solutionList)
    {
        List<string> rephrasedSolutionList = new List<string>();

        foreach (string solution in solutionList)
        {
            if (solution == "F")
            {
                rephrasedSolutionList.Add("Rotate Front Face 90 degrees in clockwise direction");
            }
            else if (solution == "B")
            {
                rephrasedSolutionList.Add("Rotate Back Face 90 degrees in clockwise direction");
            }
            else if (solution == "U")
            {
                rephrasedSolutionList.Add("Rotate Upper Face 90 degrees in clockwise direction");
            }
            else if (solution == "D")
            {
                rephrasedSolutionList.Add("Rotate Down Face 90 degrees in clockwise direction");
            }
            else if (solution == "L")
            {
                rephrasedSolutionList.Add("Rotate Left Face 90 degrees in clockwise direction");
            }
            else if (solution == "R")
            {
                rephrasedSolutionList.Add("Rotate Right Face 90 degrees in clockwise direction");
            }
            else if (solution == "F'")
            {
                rephrasedSolutionList.Add("Rotate Front Face 90 degrees in anti-clockwise direction");
            }
            else if (solution == "B'")
            {
                rephrasedSolutionList.Add("Rotate Back Face 90 degrees in anti-clockwise direction");
            }
            else if (solution == "U'")
            {
                rephrasedSolutionList.Add("Rotate Upper Face 90 degrees in anti-clockwise direction");
            }
            else if (solution == "D'")
            {
                rephrasedSolutionList.Add("Rotate Down Face 90 degrees in anti-clockwise direction");
            }
            else if (solution == "L'")
            {
                rephrasedSolutionList.Add("Rotate Left Face 90 degrees in anti-clockwise direction");
            }
            else if (solution == "R'")
            {
                rephrasedSolutionList.Add("Rotate Right Face 90 degrees in anti-clockwise direction");
            }
            else if (solution == "F2")
            {
                rephrasedSolutionList.Add("Rotate Front Face 180 degrees in clockwise direction");
            }
            else if (solution == "B2")
            {
                rephrasedSolutionList.Add("Rotate Back Face 180 degrees in clockwise direction");
            }
            else if (solution == "U2")
            {
                rephrasedSolutionList.Add("Rotate Upper Face 180 degrees in clockwise direction");
            }
            else if (solution == "D2")
            {
                rephrasedSolutionList.Add("Rotate Down Face 180 degrees in clockwise direction");
            }
            else if (solution == "L2")
            {
                rephrasedSolutionList.Add("Rotate Left Face 180 degrees in clockwise direction");
            }
            else if (solution == "R2")
            {
                rephrasedSolutionList.Add("Rotate Right Face 180 degrees in clockwise direction");
            }
        }
        return rephrasedSolutionList;
    }

    public void Scramble()
    {
        if (!CubeState.keyMove)
        {
            displaySteps = true;
            ShowSteps();
            List<string> moves = new List<string>();
            int shuffleLength = Random.Range(10, 20);
            for (int i = 0; i < shuffleLength; i++)
            {
                int randomMove = Random.Range(0, allMoves.Count);
                moves.Add(allMoves[randomMove]);
            }
            keyboardControl.scrambleMoveList = moves;
        }
    }
}
