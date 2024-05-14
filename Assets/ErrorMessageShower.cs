using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorMessageShower : Singleton<ErrorMessageShower>
{
    public void ShowError(string message, ErrorMessageImage image)
    {
        //TODO: implement this
    }
    public void ShowError(string message)
    {
        ShowError(message, ErrorMessageImage.normal);
    }
    public enum ErrorMessageImage
    {
        normal,
        niceCat,
        badDog
    }
}
