using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class TimeController : MonoBehaviour {

    [SerializeField] GameObject activitySlice;
    [SerializeField] Transform activityPos;
    [SerializeField] Text totalPercentageText;
    public InputField iField;
    public InputField timeField;
    public List<string> activitiesText = new List<string>();
    public GameObject[] textPlacements;
    private Color[] colors = { new Color32(230, 235, 249, 255), new Color32(182, 152, 198, 255), new Color32(130, 77, 153, 255), new Color32(79, 120, 196, 255), new Color32(87, 163, 171, 255), new Color32(126, 185, 117, 255), new Color32(208, 181, 65, 255), new Color32(230, 127, 50, 255), new Color32(206, 33, 33, 255), new Color32(93, 25, 20, 255) };
    private List<Image> activities = new List<Image>();

    private void Start() {
        LoadActivities();
        LoadSavedFillAmounts();
    }


    public void AddActivity() {
        //Check if the text input is not empty
        if(iField.text != "") {
            //Instantiate the image
            Image activity = Instantiate(activitySlice, activityPos).GetComponent<Image>();

            //Fill the right ammount and add to activities
            float activityTime = int.Parse(timeField.text);
            activity.fillAmount = (activityTime / 60) / 24 * 1;
            activity.transform.SetAsFirstSibling();
            totalPercentageText.text = Mathf.Round((activity.fillAmount * 100)).ToString() + "%";
            SaveActivity(iField.text + " " + timeField.text + " " + "min");
            activitiesText.Add(iField.text + " " +  timeField.text + " " + "min");
            activities.Add(activity);
            for (int i = 0; i < activities.Count; i++) {
                if (i > 0) {
                    if (i == activities.Count - 1) {
                        int j = i - 1;
                        activity.fillAmount += activities[j].fillAmount;
                        totalPercentageText.text = Mathf.Round((activity.fillAmount * 100)).ToString() + "%";
                    }
                }
            }

            for (int i = 0; i < activities.Count; i++) {
                activities[i].color = colors[i];
            }

            //Activate the activity object
            for (int i = 0; i < activitiesText.Count; i++) {
                textPlacements[i].SetActive(true);
                textPlacements[i].transform.GetChild(0).GetComponent<Image>().color = colors[i];
                textPlacements[i].transform.GetChild(1).GetComponent<Text>().text = activitiesText[i];
            }

            SaveFillAmount(activity.fillAmount);

            //Clear the fields
            iField.text = "";
            timeField.text = "";
        }
    }

    public void ClearActivities() {
        activities.Clear();
        ClearSavedFillAmounts();
        ClearSavedActivities();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SaveActivity(string activityText) {
        string filepath = Application.persistentDataPath + "/newsave.txt";
        StreamWriter sw = new StreamWriter(filepath, true);
        //Debug.Log(Application.persistentDataPath);
        sw.WriteLine(activityText);
        sw.Close();
    }

    public void LoadActivities() {
        string filepath = Application.persistentDataPath + "/newsave.txt";
        StreamReader sw = new StreamReader(filepath);
        sw.ReadToEnd();
        int numOfLines = File.ReadAllLines(filepath).Length;
        string[] lines = File.ReadAllLines(filepath);
        for (int i = 0; i < numOfLines; i++) {
            string activityText = lines[i];
            activitiesText.Add(activityText);
            textPlacements[i].SetActive(true);
            textPlacements[i].transform.GetChild(0).GetComponent<Image>().color = colors[i];
            textPlacements[i].transform.GetChild(1).GetComponent<Text>().text = activitiesText[i];
        }
        sw.Close();
    }

    public void ClearSavedActivities() {
        string filepath = Application.persistentDataPath + "/newsave.txt";
        File.WriteAllText(filepath, string.Empty);
        TextWriter sw = new StreamWriter(filepath, true);
        sw.Close();
    }

    public void SaveFillAmount(float fillAmount) {
        string filepath = Application.persistentDataPath + "/fillamount.txt";
        StreamWriter sw = new StreamWriter(filepath, true);
        sw.WriteLine(fillAmount);
        sw.Close();
    }

    public void LoadSavedFillAmounts() {
        string filepath = Application.persistentDataPath + "/fillamount.txt";
        StreamReader sw = new StreamReader(filepath);
        sw.ReadToEnd();
        int numOfLines = File.ReadAllLines(filepath).Length;
        string[] fillAmount = File.ReadAllLines(filepath);
        for (int i = 0; i < numOfLines; i++) {
            float fillAmounti = float.Parse(fillAmount[i]);
            Image activity = Instantiate(activitySlice, activityPos).GetComponent<Image>();
            activity.fillAmount = fillAmounti;
            activity.color = colors[i];
            activity.transform.SetAsFirstSibling();
            totalPercentageText.text = Mathf.Round((activity.fillAmount * 100)).ToString() + "%";
            activities.Add(activity);
        }
        sw.Close();
    }

    public void ClearSavedFillAmounts() {
        string filepath = Application.persistentDataPath + "/fillamount.txt";
        File.WriteAllText(filepath, string.Empty);
        TextWriter sw = new StreamWriter(filepath, true);
        sw.Close();
    }
}
