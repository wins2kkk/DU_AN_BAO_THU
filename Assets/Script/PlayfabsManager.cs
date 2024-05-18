using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using Unity.VisualScripting;
using UnityEngine.UI;
using System;

public class PlayfabsManager : MonoBehaviour
{

    [Header("UI")]
    public Text messageText;
    [SerializeField] InputField emailInput;
    public InputField passwordInput;
    // đăng nhập
    public void RegisterButton()
    {
        var request = new RegisterPlayFabUserRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };
        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }
    void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        messageText.text = "Registered and Logged in!";
    }
    

    
    public void LoginButton() 
    { 

    }
    public void ResetPasswordButton()
    {
        
    }
    //void OnPasswordReset(SendAccountRecoveryEmailRequest result)
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }

   void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true,
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Đăng nhập tạo/tài khoản thành công!");
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Đăng nhập / tạo tài khoản thất bại!");
        Debug.Log(error.GenerateErrorReport());
    }
    
    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlatformScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        throw new NotImplementedException();
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsRequest result)
    {
        Debug.Log("Successfull leaderbord sent");
    }
    
}
