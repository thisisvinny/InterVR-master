using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class RandomGenerator : MonoBehaviour
{
    public GameObject TextBox; //PointBox;
    private int QuestionType;
    private int QuestionNumber;
    const int QTypes = 5;

    //JSON
    private string path;
    private string json;
    private Questions questions;

    public void Start()
    {
        //JSON
        path = "Assets/Questions/questions.json";
        json = File.ReadAllText(path);
        questions = JsonUtility.FromJson<Questions>(json);
    }

    public void RandomGenerate(){
        QuestionType = Random.Range(0, QTypes);
        if (QuestionType == 0)
        {
            QuestionNumber = Random.Range(0, questions.type_1.Length);
            TextBox.GetComponent<Text>().text = questions.type_1[QuestionNumber].question;
        }
        else if (QuestionType == 1)
        {
            QuestionNumber = Random.Range(0, questions.type_2.Length);
            TextBox.GetComponent<Text>().text = questions.type_2[QuestionNumber].question;
        }
        else if (QuestionType == 2)
        {
            QuestionNumber = Random.Range(0, questions.type_3.Length);
            TextBox.GetComponent<Text>().text = questions.type_3[QuestionNumber].question;
        }
        else if (QuestionType == 3)
        {
            QuestionNumber = Random.Range(0, questions.type_4.Length);
            TextBox.GetComponent<Text>().text = questions.type_4[QuestionNumber].question;
        }
        else if (QuestionType == 4)
        {
            QuestionNumber = Random.Range(0, questions.type_5.Length);
            TextBox.GetComponent<Text>().text = questions.type_5[QuestionNumber].question;
        }
    }

    //JSON class
    [System.Serializable]
    public class Questions
    {
        public Question[] type_1;
        public Question[] type_2;
        public Question[] type_3;
        public Question[] type_4;
        public Question[] type_5;
    }

    //JSON class
    [System.Serializable]
    public class Question
    {
        public int type;
        public string question;
        public string sound;
        public string[] choices;
        public int[] points;
    }
}
