using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        ReceptionWaiting,
        ReceptionReady,
        InterviewIntroduction,
        InterviewBegin,
        InterviewQuestion,
        InterviewAnswer,
        InterviewEnd
    }

    // Hold GameState
    private GameState myGameState;

    // Private Variables
    private float gameTime;
    private float InterviewReadyTime;
    private float cutsceneTime;
    private float responseTime;
    private float InterviewResponseTime;
    readonly string intro = "Good afternoon, I'm glad you could make it today. It's nice to meet you. We can begin when you are ready.";
    private float questionsAnswered;
    readonly float questionsTotal = 3;
    private float tts_elapsed_time;
    private float tts_total_time;

    // Public Objects 
    public GameObject ProceedToInterview;
    public PlayableDirector StartCutscene;
    public GameObject StartTheInterview;
    public StreamingGame stt;
    public TextToSpeechGame tts;
    public GameObject QuestionBox;
    public GameObject ResponseTime;
    public GameObject ResponseBox;
    public RandomGenerator QuestionGenerator;

    // Start is called before the first frame update
    void Start()
    {
        SetGameState(GameState.ReceptionWaiting);
        gameTime = 0.0f;
        InterviewReadyTime = Random.Range(7, 13); //7-13
        cutsceneTime = 0.0f;
        responseTime = 0.0f;
        InterviewResponseTime = 15.0f; //30
        tts_elapsed_time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (GetGameState() == GameState.ReceptionWaiting)
        {
            gameTime += Time.deltaTime;
            if (gameTime > InterviewReadyTime)
            {
                ProceedToInterview.gameObject.SetActive(true);
                SetGameState(GameState.ReceptionReady);
            }
        }
        else if (GetGameState() == GameState.ReceptionReady)
        {
            if (OVRInput.Get(OVRInput.Button.One) || Input.GetMouseButtonDown(0)) //Press A or Left Mouse
            {
                ProceedToInterview.gameObject.SetActive(false);
                StartCutscene.Play();
            }
            if (StartCutscene.state == PlayState.Playing)
            {
                cutsceneTime += Time.deltaTime;
                if (cutsceneTime > StartCutscene.duration)
                {
                    SetGameState(GameState.InterviewIntroduction);
                }
            }
        }
        else if (GetGameState() == GameState.InterviewIntroduction)
        {
            tts.SetTextToSpeech(intro);
        }
        else if (GetGameState() == GameState.InterviewBegin)
        {
            if (tts_elapsed_time < tts_total_time)
            {
                stt.StopRecording();
                tts_elapsed_time += Time.deltaTime;
            }
            else
            {
                StartTheInterview.GetComponent<Text>().text = "You will have " + InterviewResponseTime.ToString("f0") + " seconds to respond to each question.\n\nPress A to start the interview.";
                StartTheInterview.gameObject.SetActive(true);
                if (OVRInput.Get(OVRInput.Button.One) || Input.GetMouseButton(0)) //Press A or Left Mouse
                {
                    StartTheInterview.gameObject.SetActive(false);
                    QuestionGenerator.RandomGenerate();
                    SetGameState(GameState.InterviewQuestion);
                }
            }
        }
        else if (GetGameState() == GameState.InterviewQuestion)
        {
            tts_elapsed_time = 0.0f;
            tts.SetTextToSpeech(QuestionBox.GetComponent<Text>().text);
            ResponseBox.gameObject.GetComponent<Text>().text = "";
            ResponseTime.gameObject.GetComponent<Text>().text = "";
        }
        else if (GetGameState() == GameState.InterviewAnswer)
        {
            if (tts_elapsed_time < tts_total_time)
            {
                stt.StopRecording();
                tts_elapsed_time += Time.deltaTime;
            }
            else
            {
                stt.StartRecording();
                ResponseBox.gameObject.SetActive(true);
                ResponseTime.gameObject.SetActive(true);
                responseTime += Time.deltaTime;
                ResponseTime.GetComponent<Text>().text = responseTime.ToString("f0");
                if (responseTime > InterviewResponseTime)
                {
                    stt.StopRecording();
                    responseTime = 0.0f;
                    ResponseBox.gameObject.SetActive(false);
                    ResponseTime.gameObject.SetActive(false);
                    questionsAnswered++;
                    if (questionsAnswered < questionsTotal)
                    {
                        QuestionGenerator.RandomGenerate();
                        SetGameState(GameState.InterviewQuestion);
                    }
                    else
                    {
                        SetGameState(GameState.InterviewEnd);
                    }
                }
            }
        }
        else if (GetGameState() == GameState.InterviewEnd)
        {
            QuestionBox.gameObject.SetActive(false);
            ResponseBox.gameObject.SetActive(true);
            ResponseBox.gameObject.GetComponent<Text>().text = "Congratulations! You got the job!";
        }
    }

    //GameState getter
    public GameState GetGameState()
    {
        return myGameState;
    }

    //GameState setters
    public void SetGameState(GameState gameState)
    {
        myGameState = gameState;
    }

    public void SetTTStime(AudioSource source)
    {
        tts_total_time = source.clip.length;
    }

}
