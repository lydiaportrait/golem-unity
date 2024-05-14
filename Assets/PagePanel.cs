using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PagePanel : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI pageLabel;
    int currentPage = 1;
    [SerializeField] List<GameObject> pages = new List<GameObject>();
    GameObject CurrentlyShown;
    public void LeftButton()
    {
        currentPage--;
        UpdatePages();
    }
    public void RightButton()
    {
        currentPage++;
        UpdatePages();
    }
    void UpdatePages()
    {
        if (currentPage < 1)
            currentPage = 1;
        if (currentPage > pages.Count)
            currentPage = pages.Count;
        if (pages[currentPage - 1] == CurrentlyShown)
            return;
        pageLabel.text = currentPage.ToString();
        CurrentlyShown.SetActive(false);
        CurrentlyShown = pages[currentPage - 1];
        CurrentlyShown.SetActive(true);
    }
    private void Awake()
    {
        if (CurrentlyShown == null)
            CurrentlyShown = pages[currentPage - 1];
        foreach (GameObject page in pages)
            if (page != CurrentlyShown)
                page.SetActive(false);
            else
                page.SetActive(true);
    }
}
