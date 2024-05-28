using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
public class Manager : MonoBehaviour
{

    public GameObject[] Levels;
    public GameObject ResetScreen,End;


    public TMP_Text endText,withNoText,withYesText;
    int currentLevel;
    int[] wrongAnswersByLevel; // อาร์เรย์เพื่อเก็บระดับที่ผิด
    private string[] correctAnswers; // อาร์เรย์เพื่อเก็บเฉลย
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;
    string[] englishWords;
    string[] thaiWords;
    public DictionaryManager dictionaryManager;
    private string[] currentEnglishWords;
    private string[] currentThaiWords;

    private int wintotal;

    public GameObject withNo, withYes;

    public AudioSource Source;
    public AudioClip AnswerYes, AnswerNo;

    public int MoneyNow,ScoreNow;
    public TMP_Text MOneyNowText;

    private void Start()
    {
        englishWords = dictionaryManager.englishWords;
        thaiWords = dictionaryManager.thaiWords;

        wrongAnswersByLevel = new int[Levels.Length];
        correctAnswers = new string[Levels.Length];
        currentEnglishWords = new string[Levels.Length];
        currentThaiWords = new string[Levels.Length];

        for (int i = 0; i < Levels.Length; i++)
        {
            int randomIndex = GetUniqueRandomIndex(thaiWords, currentEnglishWords);

            correctAnswers[i] = englishWords[randomIndex];
            currentEnglishWords[i] = englishWords[randomIndex];
            currentThaiWords[i] = thaiWords[randomIndex];

            GameObject yesObject = GetGameObjectWithTag("Yes", Levels[i]);
            GameObject questionObject = GetGameObjectWithTag("Question", Levels[i]);
            GameObject noObject = GetGameObjectWithTag("No", Levels[i]);
            GameObject noObject2 = GetGameObjectWithTag("No2", Levels[i]);
            GameObject noObject3 = GetGameObjectWithTag("No3", Levels[i]);

            TextMeshProUGUI yesText = yesObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI questionText = questionObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI noText = noObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI noText2 = noObject2.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI noText3 = noObject3.GetComponent<TextMeshProUGUI>();

            yesText.text = currentThaiWords[i];
            questionText.text = currentEnglishWords[i];
            noText.text = thaiWords[GetUniqueRandomIndex(thaiWords, currentEnglishWords)];
            noText2.text = thaiWords[GetUniqueRandomIndex(thaiWords, currentEnglishWords)];
            noText3.text = thaiWords[GetUniqueRandomIndex(thaiWords, currentEnglishWords)];

        }
    }

    public void wrongAnswer()
    {
/*        withNo.SetActive(true);*/
        StartCoroutine(ShowNo(0.5f));
        withNoText.text = currentEnglishWords[currentLevel] + " คือ " + currentThaiWords[currentLevel];
        Source.PlayOneShot(AnswerNo, 0.7F);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void correctAnswer()
    {
        /*        AdvanceToNextLevel();
                wintotal++;
                int playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
                PlayerPrefs.SetInt("PlayerCoins", playerCoins + (50 * wintotal));*/

        /*      withYes.SetActive(true);*/


        int playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
        PlayerPrefs.SetInt("PlayerCoins", playerCoins + (50));
        Source.PlayOneShot(AnswerYes, 0.7F);
        StartCoroutine(ShowYes(0.5f));
        wintotal++;
        MoneyNow += (50);
        ScoreNow += 100;
        MOneyNowText.text = ScoreNow.ToString();
        withYesText.text = currentEnglishWords[currentLevel] + " คือ " + currentThaiWords[currentLevel];
    }

    public void GoNextWithYes()
    {
        withYes.SetActive(false);
        AdvanceToNextLevel();
        

    }
    public void GoNext()
    {
        withNo.SetActive(false);
        AdvanceToNextLevel();
    }

    private IEnumerator ShowNo(float duration)
    {
        yield return new WaitForSeconds(duration);
        withNo.SetActive(true);
    }

    private IEnumerator ShowYes(float duration)
    {
        yield return new WaitForSeconds(duration);
        withYes.SetActive(true);
    }
    private void AdvanceToNextLevel()
    {
        if (currentLevel + 1 != Levels.Length)
        {
            Levels[currentLevel].SetActive(false);
            currentLevel++;
            Levels[currentLevel].SetActive(true);

            int randomIndex = GetUniqueRandomIndex(thaiWords, currentEnglishWords);
            correctAnswers[currentLevel] = englishWords[randomIndex];
            currentEnglishWords[currentLevel] = englishWords[randomIndex];
            currentThaiWords[currentLevel] = thaiWords[randomIndex];

            GameObject yesObject = GetGameObjectWithTag("Yes", Levels[currentLevel]);
            GameObject questionObject = GetGameObjectWithTag("Question", Levels[currentLevel]);
            GameObject noObject = GetGameObjectWithTag("No", Levels[currentLevel]);
            GameObject noObject2 = GetGameObjectWithTag("No2", Levels[currentLevel]);
            GameObject noObject3 = GetGameObjectWithTag("No3", Levels[currentLevel]);

            TextMeshProUGUI yesText = yesObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI questionText = questionObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI noText = noObject.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI noText2 = noObject2.GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI noText3 = noObject3.GetComponent<TextMeshProUGUI>();

            yesText.text = currentThaiWords[currentLevel];
            questionText.text = currentEnglishWords[currentLevel];
            noText.text = thaiWords[GetUniqueRandomIndex(thaiWords, currentEnglishWords)];
            noText2.text = thaiWords[GetUniqueRandomIndex(thaiWords, currentEnglishWords)];
            noText3.text = thaiWords[GetUniqueRandomIndex(thaiWords, currentEnglishWords)];


        }
        else
        {
            End.SetActive(true);
            DisplayAnswers();
            remainingTime = 0;
            Levels[currentLevel].SetActive(false);
        }
    }

    public int[] GetWrongAnswersByLevel()
    {
        return wrongAnswersByLevel;
    }

    private void DisplayAnswers()
    {

        endText.text = "Congratulations เก่งมาก!! \n คะแนนที่ได้: " + ScoreNow + " คะแนน \n เงินที่คุณได้รับ: " + MoneyNow.ToString();
        Debug.Log("เงินเก็บ: "+PlayerPrefs.GetInt("PlayerCoins", 0));
    }

    private void Update()
    {
        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            ResetScreen.gameObject.SetActive(true);
            Levels[currentLevel].SetActive(false);
        }

        if (remainingTime <= 10)
        {
            timerText.color = Color.red;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    private int GetUniqueRandomIndex(string[] array, string[] excludeWords)
    {
        int randomIndex = UnityEngine.Random.Range(0, array.Length);
        while (ArrayContains(excludeWords, array[randomIndex]))
        {
            randomIndex = UnityEngine.Random.Range(0, array.Length);
        }
        return randomIndex;
    }

    private bool ArrayContains(string[] array, string word)
    {
        foreach (var w in array)
        {
            if (w == word)
            {
                return true;
            }
        }
        return false;
    }

    private GameObject GetGameObjectWithTag(string tag, GameObject parent)
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(tag);
        foreach (var obj in objects)
        {
            if (obj.transform.IsChildOf(parent.transform))
            {
                return obj;
            }
        }
        return null;
    }

}
