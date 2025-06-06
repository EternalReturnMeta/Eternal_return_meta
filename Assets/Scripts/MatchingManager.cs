using System.Linq;
using Fusion;
using Fusion.Menu;
using UnityEngine;


public class MatchingManager : NetworkBehaviour
{
    public static MatchingManager Instance { get; private set; }
    [Networked] public bool IsMatchingComplete { get; set; }
    [Networked] public TickTimer LoadingTimer { get; set; }
    [Networked] public TickTimer CharacterSelectTimer { get; set; }
    [Networked] public bool IsCharacterSelectActive { get; set; }
    [Networked, Capacity(2)]
    public NetworkDictionary<PlayerRef, CharacterDataEnum> SelectedCharacters => default;


    private int MaxPlayerCount { get; set; } = 2;
    public const float LoadingDuration = 5f;
    public const float CharacterSelectDuration = 60f;
    public MenuUIController Controller { get; set; }
    
    public override void Spawned()
    {
        Instance = this;
        if (Controller == null)
            Controller = FindAnyObjectByType<MenuUIController>();
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void Rpc_SelectCharacter(CharacterDataEnum characterId, PlayerRef playerRef)
    {
        if (!SelectedCharacters.ContainsKey(playerRef))
        {
            SelectedCharacters.Add(playerRef, characterId);
        }
        else
        {
            SelectedCharacters.Set(playerRef, characterId);
        }
    }
    
    // MaxPlayer를 가져옴
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_UpdatePlayerCount(int ready, int max)
    {
        if (!HasStateAuthority)
            return;
        Controller.Get<MatchingModal>().UpdatePlayerCount(ready, max);
    }

    public override void FixedUpdateNetwork()
    {
        
        if (Controller == null)
        {
            Debug.Log("Controller is null");
        }
        if (!Object.HasStateAuthority) return;
        
        
        int currentPlayers = Runner.ActivePlayers.Count();
        RPC_UpdatePlayerCount(currentPlayers, MaxPlayerCount);
        if (!IsMatchingComplete && Runner.ActivePlayers.Count() == MaxPlayerCount)
        {
            IsMatchingComplete = true;
            LoadingTimer = TickTimer.CreateFromSeconds(Runner, LoadingDuration);
            RPC_ShowLoading();
        }
        if (IsMatchingComplete && LoadingTimer.Expired(Runner) && !IsCharacterSelectActive)
        {
            CharacterSelectTimer = TickTimer.CreateFromSeconds(Runner, CharacterSelectDuration);
            IsCharacterSelectActive = true;
            RPC_GoToCharacterSelect();
        }

        // 캐릭터 선택 완료(모두 선택 or 시간 만료) → 인게임 이동
        if (IsCharacterSelectActive &&
            (CharacterSelectTimer.Expired(Runner) || SelectedCharacters.Count == MaxPlayerCount))
        {
            // RPC_GoToGame();
        }
        
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ShowLoading()
    {
        Controller.Show<FusionMenuUILoading>();
        
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_GoToCharacterSelect()
    {
        Controller.Show<FusionMenuUICharacterSelect>();
    }
}
