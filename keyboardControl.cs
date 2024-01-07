using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class keyboardControl : MonoBehaviour
{

    private CubeState cubeState;
    private ReadCube readCube;
    private List<string> moveBuffer = new List<string>() { };
    public static List<string> scrambleSteps = new List<string>() { };
    public static List<string> solveSteps = new List<string>() { };

    public static List<string> scrambleMoveList = new List<string>() { };

    public static List<string> kociembaSolveList = new List<string>() { };

    
    // Start is called before the first frame update
    void Start()
    {
        cubeState = FindObjectOfType<CubeState>();
        readCube = FindObjectOfType<ReadCube>();
    }

    // Update is called once per frame
    void Update()
    {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    moveBuffer.Add("U'");

                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    moveBuffer.Add("D'");
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    moveBuffer.Add("F'");
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    moveBuffer.Add("B'");
                }
                else if (Input.GetKeyDown(KeyCode.L))
                {
                    moveBuffer.Add("L'");
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    moveBuffer.Add("R'");
                }

            }
            else
            {
                if (Input.GetKeyDown(KeyCode.U))
                {
                    moveBuffer.Add("U");
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    moveBuffer.Add("D");
                }
                else if (Input.GetKeyDown(KeyCode.F))
                {
                    moveBuffer.Add("F");
                }
                else if (Input.GetKeyDown(KeyCode.B))
                {
                    moveBuffer.Add("B");
                }
                else if (Input.GetKeyDown(KeyCode.L))
                {
                    moveBuffer.Add("L");
                }
                else if (Input.GetKeyDown(KeyCode.R))
                {
                    moveBuffer.Add("R");
                }
            }
        if(CubeState.started && !CubeState.keyMove && !CubeState.autoRotateDrag && !CubeState.drag && moveBuffer.Count>0)
        {
            DoMove(moveBuffer[0]);
            moveBuffer.Remove(moveBuffer[0]);
        }
        if (CubeState.started && !CubeState.keyMove && !CubeState.autoRotateDrag && !CubeState.drag && scrambleSteps.Count > 0)
        {
            DoMove(scrambleSteps[0]);
            scrambleSteps.Remove(scrambleSteps[0]);
        }
        if (CubeState.started && !CubeState.keyMove && !CubeState.autoRotateDrag && !CubeState.drag && solveSteps.Count > 0)
        {
            DoMove(solveSteps[0]);
            solveSteps.Remove(solveSteps[0]);
        }
        if (CubeState.started && !CubeState.keyMove && !CubeState.autoRotateDrag && !CubeState.drag && scrambleMoveList.Count > 0)
        {
            DoMove(scrambleMoveList[0]);
            scrambleMoveList.Remove(scrambleMoveList[0]);
        }
        if (CubeState.started && !CubeState.keyMove && !CubeState.autoRotateDrag && !CubeState.drag && kociembaSolveList.Count > 0)
        {
            DoMove(kociembaSolveList[0]);
            kociembaSolveList.Remove(kociembaSolveList[0]);
        }

    }


    void DoMove(string move)
    {
        readCube.ReadState();
        CubeState.keyMove = true;
        if(move == "U")
        {
            RotateSide(cubeState.up, -90);
        }
        else if(move =="U'")
        {
            RotateSide(cubeState.up, 90);
        }
        else if (move == "D" )
        {
            RotateSide(cubeState.down, -90);
        }
        else if (move == "D'")
        {
            RotateSide(cubeState.down, +90);
        }
        else if (move == "F")
        {
            RotateSide(cubeState.front, -90);
        }
        else if (move == "F'")
        {
            RotateSide(cubeState.front, 90);
        }
        else if (move == "B")
        {
            RotateSide(cubeState.back, -90);
        }
        else if (move == "B'")
        {
            RotateSide(cubeState.back, 90);
        }
        else if (move == "L")
        {
            RotateSide(cubeState.left, -90);
        }
        else if (move == "L'")
        {
            RotateSide(cubeState.left, 90);
        }
        else if (move == "R")
        {
            RotateSide(cubeState.right, -90);
        }
        else if (move == "R'")
        {
            RotateSide(cubeState.right, 90);
        }
        else if(move == "U2")
        {
            RotateSide(cubeState.up, -180);
        }
        else if (move == "D2")
        {
            RotateSide(cubeState.down, -180);
        }
        else if (move == "F2")
        {
            RotateSide(cubeState.front, -180);
        }
        else if (move == "B2")
        {
            RotateSide(cubeState.back, -180);
        }
        else if (move == "L2")
        {
            RotateSide(cubeState.left, -180);
        }
        else if (move == "R2")
        {
            RotateSide(cubeState.right, -180);
        }
    }

    void RotateSide(List<GameObject> side, float angle)
    {
        PivotRotation pr = side[4].transform.parent.GetComponent<PivotRotation>();
        pr.keyRotate(side, angle);
    }

   

}
