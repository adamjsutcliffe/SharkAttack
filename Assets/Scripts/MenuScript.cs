using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TweenAnimation.Models;
using TweenAnimation.Models.Animations;

public class MenuScript : MonoBehaviour {

    public GameObject StartPanel;
    public GameObject StartPanelSelection;
    public GameObject ExposurePanel;
    public GameObject ExposureSelection;
    public GameObject TimePanel;
    public GameObject TimePanelSelection;
    public GameObject SettingsPanel;
    public GameObject EndPanel;
    public GameObject ResetPanel;

    private Dictionary<string, GameObject> panels;

    private readonly List<TweenAnimation.Models.TweenAnimation> animations = new List<TweenAnimation.Models.TweenAnimation>();

    private void Awake()
    {
        panels = new Dictionary<string, GameObject>();

        //StartPanel.name = Constants.UIPanels.StartPanel;
        //StartPanelSelection.name = Constants.UIPanels.StartSelection;
        //ExposurePanel.name = Constants.UIPanels.ExposurePanel;
        //ExposureSelection.name = Constants.UIPanels.ExposureSelection;
        //TimePanel.name = Constants.UIPanels.TimePanel;
        //TimePanelSelection.name = Constants.UIPanels.TimeSelection;
        //SettingsPanel.name = Constants.UIPanels.SettingPanel;
        //EndPanel.name = Constants.UIPanels.EndPanel;
        //ResetPanel.name = Constants.UIPanels.ResetPanel;

        panels[Constants.UIPanels.StartPanel] = StartPanel;
        panels[Constants.UIPanels.StartSelection] = StartPanelSelection;
        panels[Constants.UIPanels.ExposurePanel] = ExposurePanel;
        panels[Constants.UIPanels.ExposureSelection] = ExposureSelection;
        panels[Constants.UIPanels.TimePanel] = TimePanel;
        panels[Constants.UIPanels.TimeSelection] = TimePanelSelection;
        panels[Constants.UIPanels.SettingPanel] = SettingsPanel;
        panels[Constants.UIPanels.ResetPanel] = ResetPanel;
        panels[Constants.UIPanels.EndPanel] = EndPanel;
    }

    public void ShowPanel(string panel)
    {
        //TODO: separate string by commas check if quantity is greaterthan zero
        var panelStrs = new List<string>(panel.Split(','));
        print("Animations count: " + animations.Count + " show panel: '" + panel +"'");
        TweenScript.Instance.Dequeue(animations);
        animations.Clear();

        HidePanel(panelStrs);

        foreach(string p in panelStrs)
        {
            if (panels.ContainsKey(p))
            {
                DisplayPanel(panels[p]);
            }
        }
    }

    private void DisplayPanel(GameObject panel)
    {
        print("Display panel: " + panel.name);
        animations.Add(
        Scale2D.On(panel)
                   .From(Vector3.zero)
                   .To(Vector3.one)
                   .For(0.5f)
            .Over(Curve.CubicEaseOut)
               .Start()
                   );
    }

    private void HidePanel(List<string> excluding)
    {
        foreach(KeyValuePair<string, GameObject> entry in panels)
        {
            if (!excluding.Contains(entry.Key) && entry.Value.activeSelf)
            {
                print("Hide panel: " + entry.Value.name);
                animations.Add(
                Scale2D.On(entry.Value)
                   .From(Vector3.one)
                   .To(Vector3.zero)
                   .For(0.5f)
                    .Over(Curve.CubicEaseOut)
                   .AndThen(() =>
            {
                //entry.Value.transform.localScale = Vector3.zero;
                entry.Value.SetActive(false);
            })
                       .Start()
                           );
            }
        }
    }
}
