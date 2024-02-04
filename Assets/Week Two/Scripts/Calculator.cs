using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GistLevelDesigner;
using static UnityEngine.Rendering.DebugUI;
using Unity.VisualScripting;
using UnityEngine.Windows;

public class Calculator : MonoBehaviour
{
    public TextMeshProUGUI printText;
    private float prevInput;
    private bool clearPrevInput;

    private EquationType equationType;

    private void Start()
    {
        Clear();
    }

    public void AddInput(string input)
    {
        if (clearPrevInput)
        {
            printText.text = string.Empty;
            clearPrevInput = false;
        }

        printText.text += input;
    }

    public void SetEquationAsAdd()
    {
        prevInput = float.Parse(printText.text);
        clearPrevInput = true;
        equationType = EquationType.ADD;
    }

    public void SetEquationAsSubtract()
    {
        prevInput = float.Parse(printText.text);
        clearPrevInput = true;
        equationType = EquationType.SUBTRACT;
    }

    public void SetEquationAsMultiply()
    {
        prevInput = float.Parse(printText.text);
        clearPrevInput = true;
        equationType = EquationType.MULTIPLY;
    }

    public void SetEquationAsDivide()
    {
        prevInput = float.Parse(printText.text);
        clearPrevInput = true;
        equationType = EquationType.DIVIDE;
    }

    public void Add()
    {
        float currentInput = float.Parse(printText.text);
        float result = prevInput + currentInput;
        printText.text = result.ToString();

    }

    public void Subtract()
    {
        float currentInput = float.Parse(printText.text);
        float result = prevInput - currentInput;
        printText.text = result.ToString();
    }

    public void Multiply()
    {
        float currentInput = float.Parse(printText.text);
        float result = prevInput * currentInput;
        printText.text = result.ToString();

    }

    public void Divide()
    {
        float currentInput = float.Parse(printText.text);
        if (currentInput != 0)
        {
            float result = prevInput / currentInput;
            printText.text = result.ToString();
        }
        else
        {
            //handle division by zero error
            printText.text = "Error";

        }

    }

    public void Clear()
    {
        printText.text = "0";
        clearPrevInput = true;
        prevInput = 0f;
        equationType = EquationType.None;
    }


    //function to reset clearPrevInput without clearing the displayed input
    public void Reset()
    {
        clearPrevInput = true;
        equationType = EquationType.None;
    }

    public void Calculate()
    {
        if (equationType == EquationType.ADD) Add();
        else if (equationType == EquationType.SUBTRACT) Subtract();
        else if (equationType == EquationType.MULTIPLY) Multiply();
        else if (equationType == EquationType.DIVIDE) Divide();
        Reset();

    }

    public enum EquationType
    {
        None = 0,
        ADD = 1,
        SUBTRACT = 2,
        MULTIPLY = 3,
        DIVIDE = 4
    }
}
