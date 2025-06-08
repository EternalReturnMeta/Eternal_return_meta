using Fusion;
using UnityEngine;

public class PlayerNetworkObject : NetworkBehaviour
{
    [Networked] public string Nickname { get; set; }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void Rpc_RequestSelectCharacter(CharacterDataEnum characterId)
    {
        if (MatchingManager.Instance != null)
        {
            MatchingManager.Instance.Rpc_SelectCharacter(characterId, Object.InputAuthority);
        }
    }
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void Rpc_SetNickname(string nickname)
    {
        Nickname = nickname;
    }
}
