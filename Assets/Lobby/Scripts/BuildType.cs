using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuildType : MonoBehaviour
{
    
    [Header("Build Type")]
    public Build chosenBuild = Build.RemoteClient;
    public bool debugBuild = true;
    public string buildId;

    public enum Build
    {
        RemoteServer,
        RemoteClient
    }
}
