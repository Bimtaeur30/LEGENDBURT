using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class UGSInitializer : MonoSingleton<UGSInitializer> // 싱글톤으로 변경
{
    protected override void Awake()
    {
        base.Awake(); // MonoSingleton의 중복 인스턴스 파괴 로직 활용
        DontDestroyOnLoad(gameObject);
        InitializeUGS();
    }

    private async void InitializeUGS()
    {
        await UnityServices.InitializeAsync();
        if (!AuthenticationService.Instance.IsSignedIn)
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            if (AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
            {
                Debug.Log("UGS 로그인 완료");
            }
        }
    }
}