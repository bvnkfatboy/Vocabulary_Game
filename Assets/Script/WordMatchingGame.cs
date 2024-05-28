using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using Random = UnityEngine.Random;

public class WordMatchingGame : MonoBehaviour
{
    public TextMeshProUGUI[] englishWordTexts;
    public TextMeshProUGUI[] thaiWordTexts;

    public Button[] englishWordButtons;
    public Button[] thaiWordButtons;

    public DictionaryManager dictionaryManager;

    public int numberOfWordsToMatch = 9;
    public int currentLevel = 1;
    public int totalLevels = 12; // จำนวนด่านทั้งหมด
    public TMP_Text LevelText, endText,endText2;
    
    private List<string> englishWords;
    private List<string> thaiWords;

    private List<string> selectedEnglishWords;
    private List<string> selectedThaiWords;
    private List<int> matchingEnglishIndices;
    private List<int> matchingThaiIndices;

    public int CurrentEngPick = -1;
    public int CurrentThaiPick = -1;
    public int CurrentMatch = 0;
    
    public bool StateEngPick, StateThaiPick;

    private Button currentEngButton;
    private Button currentThaiButton;

    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] float remainingTime;

    public GameObject TimeOut, Cons;
    public TextMeshProUGUI Eng_Pick_Text, Thai_Pick_Text;

    public Sprite EngIMGPick, ThaiIMGPick;

    public AudioSource AudioSources;
    public AudioClip MatchSFX, NoMatchSFX;

    public float MoneyNow;
    public TMP_Text MOneyNowText;
    public float curRank;

    public float ScoreNow;
    private void Start()
    {
        englishWords = new List<string>(dictionaryManager.englishWords);
        thaiWords = new List<string>(dictionaryManager.thaiWords);

        matchingThaiIndices = new List<int>();
        matchingEnglishIndices = new List<int>();

        // Start the first level
        StartLevel(currentLevel);
        MOneyNowText.text = "0";
    }

    private void StartLevel(int level)
    {
        // Reset the game state and any other necessary parameters for the new level
        ResetGame();
        LevelText.text = currentLevel + " / " + totalLevels;
        // You can add level-specific parameters here
        // For example, you can change the number of words to match based on the level
        // Example: if (level == 1) numberOfWordsToMatch = 5;
        // Example: if (level == 2) numberOfWordsToMatch = 7;

        SelectMatchingWords();
        ShuffleLists(selectedEnglishWords);
        ShuffleLists(selectedThaiWords);
        DisplayWords();
    }

    private void SelectMatchingWords()
    {
        selectedEnglishWords = new List<string>();
        selectedThaiWords = new List<string>();

        for (int i = 0; i < numberOfWordsToMatch; i++)
        {
            int randomIndex = GetUniqueRandomIndex(thaiWords, selectedThaiWords);
            selectedEnglishWords.Add(englishWords[randomIndex]);
            selectedThaiWords.Add(thaiWords[randomIndex]);
        }
    }

    private void DisplayWords()
    {
        for (int i = 0; i < numberOfWordsToMatch; i++)
        {
            string selectedEnglishWord = selectedEnglishWords[i];
            string selectedThaiWord = selectedThaiWords[i];

            int matchingEnglishIndex = englishWords.IndexOf(selectedEnglishWord);
            int matchingThaiIndex = thaiWords.IndexOf(selectedThaiWord);

            if (matchingEnglishIndex != -1 && matchingThaiIndex != -1)
            {
                englishWordTexts[i].text = selectedEnglishWord;
                thaiWordTexts[i].text = selectedThaiWord;

                matchingEnglishIndices.Add(matchingEnglishIndex);
                matchingThaiIndices.Add(matchingThaiIndex);

                var englishButtonData = englishWordButtons[i].GetComponent<ButtonData_Eng>();
                var thaiButtonData = thaiWordButtons[i].GetComponent<ButtonData_Thai>();

                englishButtonData.matchingEnglishIndex = matchingEnglishIndex;
                thaiButtonData.matchingThaiIndex = matchingThaiIndex;
            }
        }
    }

    public void OnButtonClick_Eng(ButtonData_Eng buttonData)
    {
        int matchingEnglishIndex = buttonData.matchingEnglishIndex;
        Debug.Log("Matching English Index: " + matchingEnglishIndex);
        CurrentEngPick = matchingEnglishIndex;
        StateEngPick = true;
        StartCoroutine(DelayedMatchCheck());
        // ลูปผ่าน englishWords เพื่อค้นหา matchingEnglishIndex

        RestoreOriginalSprites(englishWordButtons);

        buttonData.buttonImage.sprite = EngIMGPick; 

        foreach (string englishWord in englishWords)
        {
            if (englishWords.IndexOf(englishWord) == matchingEnglishIndex)
            {
                // แสดงข้อความที่คลิกเลือกใน Eng_Pick_Text
                Eng_Pick_Text.text = englishWord;
                break; // หากคุณพบข้อความแล้วก็ออกจากการลูป
            }
        }

    }

    public void OnButtonClick_Thai(ButtonData_Thai buttonData)
    {
        int matchingThaiIndex = buttonData.matchingThaiIndex;
        CurrentThaiPick = matchingThaiIndex;
        Debug.Log("Matching Thai Index: " + matchingThaiIndex);
        StateThaiPick = true;
        StartCoroutine(DelayedMatchCheck());
        RestoreOriginalSprites(thaiWordButtons);

        buttonData.buttonImage.sprite = ThaiIMGPick;

        foreach (string thaiWord in thaiWords)
        {
            if (thaiWords.IndexOf(thaiWord) == matchingThaiIndex)
            {
                // แสดงข้อความที่คลิกเลือกใน Thai_Pick_Text
                Thai_Pick_Text.text = thaiWord;
                break; // หากคุณพบข้อความแล้วก็ออกจากการลูป
            }
        }

    }

    void RestoreOriginalSprites(Button[] buttons)
    {
        foreach (Button button in buttons)
        {
            if (button == null) continue;
            ButtonData_Eng buttonData = button.GetComponent<ButtonData_Eng>();
            if (buttonData != null)
            {
                buttonData.buttonImage.sprite = buttonData.originalSprite;
            }
            ButtonData_Thai buttonDataThai = button.GetComponent<ButtonData_Thai>();
            if (buttonDataThai != null)
            {
                buttonDataThai.buttonImage.sprite = buttonDataThai.originalSprite;
            }
        }
    }

    private void DisplayAnswers()
    {

        endText.text = "Congratulations เก่งมาก!! \n คะแนนที่ได้: " + ScoreNow.ToString() + " คะแนน \n เงินที่คุณได้รับ: " + MoneyNow.ToString();
        Debug.Log("เงินเก็บ: " + PlayerPrefs.GetInt("PlayerCoins", 0));
    }

    private void DisplayAnswers2()
    {
        endText2.text = "Congratulations เก่งมาก!! \n คะแนนที่ได้: " + ScoreNow.ToString() + " คะแนน \n เงินที่คุณได้รับ: " + MoneyNow.ToString();
        Debug.Log("เงินเก็บ: " + PlayerPrefs.GetInt("PlayerCoins", 0));
    }
    

    private void Update()
    {
/*        if (CurrentEngPick == 1)
        {
            englishWordTexts[CurrentThaiPick].color = Color.black;
        }
        if(CurrentThaiPick == 1)
        {
            thaiWordTexts[CurrentThaiPick].color = Color.black;
        }*/
/*        if (StateEngPick && StateThaiPick)
        {
            if (CurrentEngPick != -1 && CurrentThaiPick != -1)
            {
                if (CurrentEngPick == CurrentThaiPick)
                {
                    print("Yes");
                    Thai_Pick_Text.text = "";
                    Eng_Pick_Text.text = "";
                    StateThaiPick = false;
                    StateEngPick = false;
                    DisableButton_Eng(CurrentEngPick);
                    DisableButton_Thai(CurrentThaiPick);
                    CurrentMatch += 1;
                    if (CurrentMatch == 9)
                    {
                        if (currentLevel == totalLevels)
                        {
                            Debug.Log("Congratulations! You completed all levels.");
                            Cons.gameObject.SetActive(true);
                            remainingTime = 0;
                        }
                        else
                        {
                            currentLevel++;
                            LoadNextLevel();
                            int playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
                            PlayerPrefs.SetInt("PlayerCoins", playerCoins + 100);
                        }
                    }
                    return;
                }
                if (CurrentEngPick != CurrentThaiPick)
                {
                    print("No");
                    StateThaiPick = false;
                    StateEngPick = false;
                    return;
                }
            }
        }*/

        if (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
        }
        else if (remainingTime < 0)
        {
            remainingTime = 0;
            /*            ResetScreen.gameObject.SetActive(true);
                        Levels[currentLevel].SetActive(false);*/
            TimeOut.gameObject.SetActive(true);
            DisplayAnswers2();
        }

        if (remainingTime <= 10)
        {
            timerText.color = Color.red;
        }
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);



    }


    private IEnumerator DelayedMatchCheck()
    {
        yield return new WaitForSeconds(0.3f); // รอ 3 วินาที

        if (StateEngPick && StateThaiPick)
        {
            if (CurrentEngPick != -1 && CurrentThaiPick != -1)
            {
                if (CurrentEngPick == CurrentThaiPick)
                {
                    print("Yes");
                    int playerCoins = PlayerPrefs.GetInt("PlayerCoins", 0);
                    MoneyNow += (50 * curRank);
                    ScoreNow += (100 * curRank);
                    MOneyNowText.text = ScoreNow.ToString();
                    PlayerPrefs.SetInt("PlayerCoins", (int)(playerCoins + MoneyNow));
                    print(playerCoins);
                    AudioSources.PlayOneShot(MatchSFX, 0.7F);
                    Thai_Pick_Text.text = "";
                    Eng_Pick_Text.text = "";
                    StateThaiPick = false;
                    StateEngPick = false;
                    DisableButton_Eng(CurrentEngPick);
                    DisableButton_Thai(CurrentThaiPick);
                    RestoreOriginalSprites(englishWordButtons);
                    RestoreOriginalSprites(thaiWordButtons);
                    CurrentMatch += 1;
                    if (CurrentMatch == 9)
                    {
                        if (currentLevel == totalLevels)
                        {
                            Debug.Log("Congratulations! You completed all levels.");
                            Cons.gameObject.SetActive(true);
                            DisplayAnswers();
                            remainingTime = 0;
                        }
                        else
                        {
                            currentLevel++;
                            LoadNextLevel();

                        }
                    }
                }
                else
                {
                    print("No");
                    AudioSources.PlayOneShot(NoMatchSFX, 0.7F);
                    StateThaiPick = false;
                    StateEngPick = false;
                    RestoreOriginalSprites(englishWordButtons);
                    RestoreOriginalSprites(thaiWordButtons);
                }
            }
        }
    }

    public void ReMapGame()
    {
        ResetGame();
        StartLevel(currentLevel);
        for (int i = 0; i < numberOfWordsToMatch; i++)
        {
            englishWordButtons[i].gameObject.SetActive(true);
            thaiWordButtons[i].gameObject.SetActive(true);
        }
    }

    private void LoadNextLevel()
    {
        ResetGame();
        LevelText.text = currentLevel + " / " + totalLevels;
        StartLevel(currentLevel);
        for (int i = 0; i < numberOfWordsToMatch; i++)
        {
            englishWordButtons[i].gameObject.SetActive(true);
            thaiWordButtons[i].gameObject.SetActive(true);
        }
    }


    private void ResetGame()
    {
        // Reset any game-specific parameters or variables here
        matchingEnglishIndices.Clear();
        matchingThaiIndices.Clear();
        CurrentEngPick = -1;
        CurrentThaiPick = -1;
        StateEngPick = false;
        StateThaiPick = false;
        RestoreOriginalSprites(englishWordButtons);
        RestoreOriginalSprites(thaiWordButtons);
    }

    private void DisableButton_Thai(int index)
    {
        for (int i = 0; i < numberOfWordsToMatch; i++)
        {
            var thaiButtonData = thaiWordButtons[i].GetComponent<ButtonData_Thai>();
            if (thaiButtonData.matchingThaiIndex == index)
            {
                thaiWordButtons[i].gameObject.SetActive(false);
                thaiWordTexts[i].color = Color.black;
                break;
            }
        }
    }

    private void DisableButton_Eng(int index)
    {
        for (int i = 0; i < numberOfWordsToMatch; i++)
        {
            var englishButtonData = englishWordButtons[i].GetComponent<ButtonData_Eng>();
            if (englishButtonData.matchingEnglishIndex == index)
            {
                englishWordButtons[i].gameObject.SetActive(false);
                englishWordTexts[i].color = Color.black;
                break;
            }
        }
    }

    private void ShuffleLists(List<string> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            int k = Random.Range(0, n--);
            string temp = list[n];
            list[n] = list[k];
            list[k] = temp;
        }
    }

    private int GetUniqueRandomIndex(List<string> list, List<string> excludeWords)
    {
        int randomIndex = Random.Range(0, list.Count);
        while (excludeWords.Contains(list[randomIndex]))
        {
            randomIndex = Random.Range(0, list.Count);
        }
        return randomIndex;
    }
}
