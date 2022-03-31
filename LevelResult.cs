using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelResult : MonoBehaviour
{
    public TMP_Text hitsNumber;

    public TMP_Text grade;

    public TMP_Text buttonText;

    public Button button;

    void Start()
    {
        Button levelResultButton = button.GetComponent<Button>();
        levelResultButton.onClick.AddListener(TaskOnClick);

        hitsNumber.text = Score.hitStudents.ToString();

        if (Score.hitStudents > 30)
        {
            grade.text = "PASS";
            buttonText.text = "Next Level";
        }
        else
        {
            grade.text = "FAIL";
            buttonText.text = "Play Again";
        }
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("MainScene 2");
    }


}
