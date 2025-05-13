using UnityEngine;


public class BaseObjectSO : ScriptableObject
{
    public enum ObjectID {
        BREAD, 
        CABBAGE, 
        CHEESE, 
        FISH_RAW,
        FISH_COOKED, 
        FISH_BURNT, 
        MEAT_RAW, 
        MEAT_COOKED,
        MEAT_BURNT,
        SQUIRREL_FISH, 
        TOMATO
    }
    public GameObject baseObject; 
    public ObjectID objectID; 
    public Sprite sprite; 
}
