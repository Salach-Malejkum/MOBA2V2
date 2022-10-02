using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

public class ServerConnections
{
    static private string uri = "http://127.0.0.1:8080";

    static public bool Login(string mail, string pass)
    {
        string loginInfo = "{'username': '" + mail + "', 'password': '" + pass + "'}";
        JObject json = JObject.Parse(loginInfo);


        try
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri + "/user/login");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Debug.Log(result.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }

        return true;
    }

    static public bool Register(string mail, string username, string pass)
    {
        string registerInfo = "{'username': '" + mail + "', 'password': '" + pass + "'}"; // potem rozszerzyæ o username
        JObject json = JObject.Parse(registerInfo);


        try
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri + "/user/register");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(json);
            }

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Debug.Log(result.ToString());
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
            return false;
        }

        return true;
    }
}
