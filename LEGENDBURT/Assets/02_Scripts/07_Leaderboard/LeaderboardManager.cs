using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using Unity.Services.Leaderboards.Models;
using UnityEngine;

public class LeaderboardManager : MonoSingleton<LeaderboardManager>
{
    private readonly Dictionary<StageType, string> ids = new()
    {
        { StageType.Stage1, "Stage_1_Leaderboard" },
        { StageType.Stage2, "Stage_2_Leaderboard" },
        { StageType.Stage3, "Stage_3_Leaderboard" }
    };

    public async Task<List<LeaderboardEntry>> GetTop20(StageType stage)
    {
        await EnsureSignedInAsync();

        var result = await LeaderboardsService.Instance
            .GetScoresAsync(ids[stage], new GetScoresOptions { Limit = 20 });
        return result.Results;
    }

    public async Task SubmitTime(StageType stage, float clearTime)
    {
        await EnsureSignedInAsync();

        string id = ids[stage];
        try
        {
            var current = await LeaderboardsService.Instance.GetPlayerScoreAsync(id);
            if (clearTime < current.Score)
                await LeaderboardsService.Instance.AddPlayerScoreAsync(id, clearTime);
        }
        catch
        {
            await LeaderboardsService.Instance.AddPlayerScoreAsync(id, clearTime);
        }
    }

    private async Task EnsureSignedInAsync()
    {
        await UnityServices.InitializeAsync();

        if (!AuthenticationService.Instance.IsSignedIn)
            await AuthenticationService.Instance.SignInAnonymouslyAsync();

        #if UNITY_EDITOR
                // 에디터에서는 씬 전환 시 LeaderboardsService.s_Instance가 리셋되는 이슈가 있어
                // Core를 재초기화해서 강제로 재등록 시도
                int retry = 0;
                while (!IsLeaderboardsReady() && retry < 20)
                {
                    await UnityServices.InitializeAsync();
                    await Task.Delay(100);
                    retry++;
                }
        #endif
    }

    private bool IsLeaderboardsReady()
    {
        try { _ = LeaderboardsService.Instance; return true; }
        catch { return false; }
    }

    private bool CheckLeaderboardsReady()
    {
        try
        {
            _ = LeaderboardsService.Instance;
            return true;
        }
        catch
        {
            return false;
        }
    }

}