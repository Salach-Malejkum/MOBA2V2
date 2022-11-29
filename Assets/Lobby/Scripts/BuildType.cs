using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class BuildType : MonoBehaviour
{
    
    [Header("Build Type")]
    public Build chosenBuild = Build.RemoteClient;
    public bool debugBuild = true;
    public string buildId;

    public static BuildType singleton { get; private set; }

    void Start() {
        DontDestroyOnLoad(this.gameObject);

        if(singleton == null) {
            singleton = this;
        } else {
            Destroy(this.gameObject);
        }
    }

    public enum Build
    {
        RemoteServer,
        RemoteClient
    }
}
