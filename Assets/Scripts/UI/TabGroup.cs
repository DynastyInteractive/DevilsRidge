using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabGroup : MonoBehaviour
{
    [SerializeField] List<TabButton> _tabs;

    [SerializeField] List<GameObject> pages;

    TabButton _currentSelectedTab;

    void Start()
    {
        foreach (TabButton tab in _tabs)
        {
            tab.OnTabSelected += OnTabSelected;
        }

        OnTabSelected(_tabs[0]);
    }

    public void OnTabSelected(TabButton tab)
    {
        _currentSelectedTab = tab;

        int tabIndex = _tabs.IndexOf(tab);
        for (int i = 0; i < pages.Count; i++)
        {
            if (i == tabIndex) pages[i].gameObject.SetActive(true);
            else pages[i].gameObject.SetActive(false);
        }
    }
}
