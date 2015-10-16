using UnityEngine;
using System.Collections;

public class TutorialPanelManager : Singleton<TutorialPanelManager>
{
    [SerializeField]
    private UIWindow tutorialBackgroundPanel;
    [SerializeField]
    private UIWindow tutorialPanels;

    private UIWindow currentTutorialPanel;

    private bool isShowingTutorial = false;
    public bool IsShowingTutorial { get { return isShowingTutorial; } }

    public void Show(UIWindow tutorialPanel)
    {
        tutorialBackgroundPanel.Show();
        tutorialPanels.Show();

        currentTutorialPanel = tutorialPanel;
        currentTutorialPanel.Show();

        isShowingTutorial = true;

        float timeToWait = tutorialBackgroundPanel.uiFadeTime;

        if (tutorialPanels.uiFadeTime > timeToWait)
            timeToWait = tutorialPanels.uiFadeTime;

        if (currentTutorialPanel.uiFadeTime > timeToWait)
            timeToWait = tutorialPanels.uiFadeTime;

        StartCoroutine(WaitToPauseCoroutine(timeToWait));
    }

    public void Hide()
    {
        Time.timeScale = 1;

        tutorialBackgroundPanel.Hide();
        tutorialPanels.Hide();

        if (currentTutorialPanel != null)
            currentTutorialPanel.Hide();

        isShowingTutorial = false;
    }

    public IEnumerator WaitToPauseCoroutine(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        Time.timeScale = 0;
    }
}
