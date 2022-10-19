using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuildType : MonoBehaviour
{
    
    [Header("Build Type")]
    [SerializeField] private Build chosenBuild = Build.RemoteClient;

    public enum Build
    {
        RemoteServer,
        RemoteClient
    }
}
