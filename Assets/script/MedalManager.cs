using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MedalManager : MonoBehaviour
{
    [HideInInspector] public MedalGui medalGui;

    public MedalList medalList;

#if NEWGROUND 
    io.newgrounds.core ngio_core;


    private void Start() {
        ngio_core = GetComponent<io.newgrounds.core>();
        if (ngio_core == null)
            Debug.Log("WTF2");
    }


    // call this method whenever you want to unlock a medal.
    void unlockMedal(int medal_id) {
        if (ngio_core == null)
        {
            Debug.Log("WTF no core in unlockMedal");
            return ;
        }
        MedalList.Medal medal = medalList.list.FirstOrDefault(m => m.id == medal_id);
        if (medal == null || medal.unlocked)
            return ;
        // create the component
        io.newgrounds.components.Medal.unlock medal_unlock = new io.newgrounds.components.Medal.unlock();

        // set required parameters
        medal_unlock.id = medal_id;

        // call the component on the server, and tell it to fire onMedalUnlocked() when it's done.
        medal.unlocked = true;
        GameManager.instance.medalManager.medalGui.ActivateMedal(medal.realName, medalList.list.FirstOrDefault(m => m.id == medal.id).sprite);
        medal_unlock.callWith(ngio_core);
    }

    public void TryToUnlockMedal(string medalName)
    {
        MedalList.Medal medal = medalList.list.FirstOrDefault(m => m.name == medalName);
        Debug.Log("trying to Unlock medal " + medalName);
        if (medal != null)
            unlockMedal(medal.id);
        else
            Debug.LogWarning("medal: " + medalName + " not found");
    }
#endif

// #if Kongregate

    public void TryToUnlockMedal(string medalName)
    {
        MedalList.Medal medal = medalList.list.FirstOrDefault(m => m.name == medalName);

        if (medal == null || medal.unlocked)
            return ;

        medal.unlocked = true;
        GameManager.instance.medalManager.medalGui.ActivateMedal(medal.realName, medalList.list.FirstOrDefault(m => m.id == medal.id).sprite);

        Application.ExternalCall("kongregate.stats.submit", medalName, 1);
    }


// #endif


}