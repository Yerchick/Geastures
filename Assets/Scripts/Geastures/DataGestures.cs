using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using PDollarGestureRecognizer;
using System.Collections.Generic;
using System.Xml;
using System;
using System.IO;

public class DataGestures : MonoBehaviour {

    public string[] requiredGestureNames = new string[] { };
    int currentGestureIndex = 0;

    public int accuracy = 2;

    public Gesture[] gestureExamples;

	private GameManager m_GameManager;

    void Start()
    {
        gestureExamples = LoadGesturesData();
		m_GameManager = FindObjectOfType<GameManager>();
    }

    Gesture[] LoadGesturesData()
    {
        List<Gesture> gestures = new List<Gesture>();

		string[] gestureFiles = new string[] { };
		UnityEngine.Object[] gFiles = Resources.LoadAll<TextAsset>("TextData/GestureSet");
		gestureFiles = new string[gFiles.Length];
		for (int i = 0; i < gFiles.Length; i++)
			gestureFiles[i] = (gFiles[i] as TextAsset).text;
        foreach (string data in gestureFiles)
            gestures.Add(ReadGestureDataString(data));
        return gestures.ToArray();
    }

    public static Gesture ReadGestureDataString(string data)
    {
        List<Point> points = new List<Point>();
        XmlTextReader xmlReader = null;
        int currentStrokeIndex = -1;
        string gestureName = "";
        try
        {
            System.IO.StringReader stringReader = new System.IO.StringReader(data);

            xmlReader = new XmlTextReader(stringReader);
            while (xmlReader.Read())
            {
                if (xmlReader.NodeType != XmlNodeType.Element) continue;
                switch (xmlReader.Name)
                {
                    case "Gesture":
                        gestureName = xmlReader["Name"];
                        if (gestureName.Contains("~")) // '~' character is specific to the naming convention of the MMG set
                            gestureName = gestureName.Substring(0, gestureName.LastIndexOf('~'));
                        if (gestureName.Contains("_")) // '_' character is specific to the naming convention of the MMG set
                            gestureName = gestureName.Replace('_', ' ');
                        break;
                    case "Stroke":
                        currentStrokeIndex++;
                        break;
                    case "Point":
                        points.Add(new Point(
                            float.Parse(xmlReader["X"]),
                            float.Parse(xmlReader["Y"]),
                            currentStrokeIndex
                        ));
                        break;
                }
            }
        }
        catch (Exception Ex)
        {
            Debug.Log("EXCEPTION: " + Ex.GetType().ToString());
            Debug.Log(Ex.Message);
        }
        finally
        {
            if (xmlReader != null)
                xmlReader.Close();
        }
        return new Gesture(points.ToArray(), gestureName);
    }

    public void RecognizeGesture(List<Point> currentGesturePoints)
    {
        //Calculates the point between the left most and right most points that you draw
        //This value can be used to instantiate objects at the point that you draw a gesture
        float minX = float.MaxValue;
        float maxX = 0;
        foreach(Point p in currentGesturePoints)
        {
            if(p.X < minX)
            {
                minX = p.X;
            }
            if(p.X > maxX)
            {
                maxX = p.X;
            }
        }
        float averageX = (minX + maxX) / 2;


        Gesture candidate = new Gesture(currentGesturePoints.ToArray());
        string gestureClass = PointCloudRecognizer.Classify(candidate, gestureExamples);

        IsRequiredGestureRecognized(gestureClass, averageX);
    }

	public bool IsRequiredGestureRecognized(string recognizedGestureName, float averageX)
    {
		string l = m_GameManager.GetCurrentQuestShape();
		if(recognizedGestureName.Replace(' ', '_') == m_GameManager.GetCurrentQuestShape())
		{
			Debug.Log("!!!1");
			m_GameManager.GenerateNextLevel();
			m_GameManager.AddScorePoint();
			return true;
		}else
		{
			//No pattern was recognized
			Debug.Log("Error! = " + recognizedGestureName);
			return false;
		}

    }
}
