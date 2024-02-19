using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HanoiTower : MonoBehaviour
{

    [SerializeField] private Transform peg1Transform;
    [SerializeField] private Transform peg2Transform;
    [SerializeField] private Transform peg3Transform;

    //height of each peg
    [SerializeField] public float pegHeight = 2.0f;
    //height of each disc
    [SerializeField] public float discHeight = 0.5f;
    //space between discs
    [SerializeField] public float discSpacing = 0.1f;

    //vars for positions for each spot on each peg
    private Vector3[] peg1Positions;
    private Vector3[] peg2Positions;
    private Vector3[] peg3Positions;

    [SerializeField] private int[] peg1 = { 1, 2, 3, 4 };
    [SerializeField] private int[] peg2 = { 0, 0, 0, 0 };
    [SerializeField] private int[] peg3 = { 0, 0, 0, 0 };

    [SerializeField] private int currentPeg = 1;

    private void Start()
    {
        //initialize the peg positions
        PegPositions();

    }

    private void PegPositions()
    {
        //set the positions for each peg
        peg1Positions = new Vector3[peg1.Length];
        peg2Positions = new Vector3[peg2.Length];
        peg3Positions = new Vector3[peg3.Length];

        float yOffset = 0.0f;
        for (int i = 0; i < peg1.Length; i++)
        {
            //define the top and bottom of each peg
            peg1Positions[i] = peg1Transform.position + Vector3.up * yOffset;
            peg2Positions[i] = peg2Transform.position + Vector3.up * yOffset;
            peg3Positions[i] = peg3Transform.position + Vector3.up * yOffset;
            yOffset += discHeight + discSpacing;
        }
    }

    private Vector3 pegPosition(Transform peg, int index)
    {
        //get possible positions for discs on each peg
        Vector3[] positions;
        float height;
        if (peg == peg1Transform)
        {
            positions = peg1Positions;
            height = peg1Transform.position.y;
        }

        else if (peg == peg2Transform)
        {
            positions = peg2Positions;
            height = peg2Transform.position.y;
        }

        else
        {
            positions = peg3Positions;
            height = peg3Transform.position.y;
        }

        if (index < 0 || index >= positions.Length)
        {
            Debug.LogError("Invalid disc index");
            return Vector3.zero;
        }

        //calculate vertical position of the disc
        float yOffset = height + index * (discHeight + discSpacing);
        //apply disc positions to positions in the array
        return new Vector3(positions[index].x, yOffset, positions[index].z);
    }

    [ContextMenu("Move Right")]
    public void MoveRight()
    {
        //check position of the current peg
        if (CanMoveRight() == false) return;

        //check pevious peg
        int[] fromArray = GetPeg(currentPeg);
        int fromIndex = GetTopNumberIndex(fromArray);

        //don't move if empty
        if (fromIndex == -1) return;

        //check for verticle position of disc on the peg
        int[] toArray = GetPeg(currentPeg + 1);
        int toIndex = GetBottomNumberIndex(toArray);

        //If the adjacent peg is FULL then we cannot move anything into it
        if (toIndex == -1 || !CanAddToPeg(fromArray[fromIndex], toArray)) return;

        ////verify the disc we are moving is not larger than the disc below it on the adjacent peg
        //if (CanAddToPeg(fromArray[fromIndex], toArray) == false) return;

        //if all checks pass then move disc to the adjacent peg
        MoveNumber(fromArray, fromIndex, toArray, toIndex);

        Transform disc = PopDiscFromCurrentPeg();
        Transform toPeg = GetPegTransform(currentPeg + 1);

        disc.SetParent(toPeg);

    }

    [ContextMenu("Move Left")]
    public void MoveLeft()
    {
        //check position of the current peg
        if (CanMoveLeft() == false) return;

        //check pevious peg
        int[] fromArray = GetPeg(currentPeg);
        int fromIndex = GetTopNumberIndex(fromArray);

        //don't move if empty
        if (fromIndex == -1) return;

        //check for verticle position of disc on the peg
        int[] toArray = GetPeg(currentPeg - 1);
        int toIndex = GetBottomNumberIndex(toArray);

        //If the adjacent peg is FULL then we cannot move anything into it
        if (toIndex == -1 || !CanAddToPeg(fromArray[fromIndex], toArray)) return;

        ////verify the disc we are moving is not larger than the disc below it on the adjacent peg
        //if (CanAddToPeg(fromArray[fromIndex], toArray) == false) return;

        //if all checks pass then move disc to the adjacent peg
        MoveNumber(fromArray, fromIndex, toArray, toIndex);

        Transform disc = PopDiscFromCurrentPeg();
        Transform toPeg = GetPegTransform(currentPeg - 1);

        disc.SetParent(toPeg);
    }

    public void SelectThisPeg(int pegNumber)
    {
        //set the peg number in unity editor then click this button to select that peg number
        currentPeg = pegNumber;

    }


    public void IncrementPegNumber()
    {
        currentPeg++;
    }

    public void DecrementPegNumber()
    {
        currentPeg--;
    }


    Transform PopDiscFromCurrentPeg()
    { 
    
        Transform currentPegTransform = GetPegTransform(currentPeg);
        int index = currentPegTransform.childCount - 1; 
        Transform disc = currentPegTransform.GetChild(index);
        return disc;
    
    }

    Transform GetPegTransform(int pegNumber)
    {
        if (pegNumber == 1) return peg1Transform;

        if (pegNumber == 2) return peg2Transform;

        return peg3Transform;
    }

    void MoveNumber(int[] fromArr, int fromIndex, int[] toArr, int toIndex)
    {
        int value = fromArr[fromIndex];
        fromArr[fromIndex] = 0;

        toArr[toIndex] = value;
    }

    bool CanMoveRight()
    {
        //if peg 1 or 2 then can move right
        return currentPeg < 3;
    }

    bool CanAddToPeg(int value, int[] peg)
    {
        int topNumberIndex = GetTopNumberIndex(peg);
        if(topNumberIndex == -1) return true;

        int topNumber = peg[topNumberIndex];
        return topNumber > value;
    }

    bool CanMoveLeft()
    {
        //if peg 2 or 3 then can move left
        return currentPeg > 1;
    }

    int[] GetPeg(int pegNumber)
    {
        if (pegNumber == 1) return peg1;

        if (pegNumber == 2) return peg2;

        return peg3;
    }

    int GetTopNumberIndex(int[] peg)
    {
        for (int i = 0; i < peg.Length; i++)
        {
            if (peg[i] != 0) return i;
        }

        return -1;
    }

    int GetBottomNumberIndex(int[] peg)
    {
        for (int i = peg.Length - 1; i >= 0; i--)
        {
            if (peg[i] == 0) return i;
        }

        return -1;
    }
}
