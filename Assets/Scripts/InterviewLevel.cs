using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InterviewLevel : MonoBehaviour
{	
	//Used to hold an interview level
	
	//Possible values for this level
	//PHASE 1 
    private List<int> phase1_distractions; //tiers
    private int phase1_wait_time; //minutes to wait in reception area
    
    //PHASE 2
    private List<int> phase2_distractions; //tiers
    private List<int> question_types; //numbers between 1-5
    private int interviewer_type; //random number between 1-7
    private int num_interviewers; //one to two interviewers
    private int num_questions;
    private int time_limit; //time limit per question
    private int min_score; //minimum score to achieve to get the job

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
