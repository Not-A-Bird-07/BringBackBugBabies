using System;
using BepInEx;
using BepInEx.Configuration;
using HarmonyLib;
using UnityEngine;
using Random = UnityEngine.Random;

namespace BringBackBugBabies;

[BepInPlugin(Plugin_Info.GUID, Plugin_Info.NAME, Plugin_Info.VERSION)]
public class Plugin : BaseUnityPlugin
{
    private ConfigEntry<int> _amount;
    public void Awake()
    {
        _amount = Config.Bind("Settings", "Amount", 5, "Amount of babies, min is 5");
        GorillaTagger.OnPlayerSpawned(Init);
    }

    private void Init()
    {
        if (_amount.Value < 5)
            _amount.SetSerializedValue("5");
        
        GameObject babiesParent = GameObject.Find("Environment Objects/05Maze_PersistentObjects/BugBabies");//eww GameObject.Find
        babiesParent.SetActive(true);

        for (int i = babiesParent.transform.childCount; i < _amount.Value; i++)
        {
            bool thing2 = Convert.ToBoolean(Random.Range(0, 2));//gets the blue or tan bug
            
            GameObject bug = Instantiate(babiesParent.transform.GetChild(thing2 ? 0 : 3).gameObject, babiesParent.transform, false);
            bug.transform.GetChild(0).localPosition = new Vector3(0, 0, Random.Range(-1, 0));
            
            AccessTools.Field(typeof(PredicatableRandomRotation), "rot")
                .SetValue(bug.GetComponent<PredicatableRandomRotation>(), RandomVector3(0.3f, -0.3f));
        }
    }

    private Vector3 RandomVector3(float max, float min) => new Vector3(Random.Range(min, max), Random.Range(min, max), Random.Range(min, max));
}
