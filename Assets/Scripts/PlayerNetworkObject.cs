using Fusion;
using UnityEngine;

public class PlayerNetworkObject : NetworkBehaviour
{
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void Rpc_RequestSelectCharacter(CharacterDataEnum characterId)
    {
        if (MatchingManager.Instance != null)
        {
            MatchingManager.Instance.Rpc_SelectCharacter(characterId, Object.InputAuthority);
        }
    }
    
    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void Rpc_RequestSelectUser(string nickname)
    {
        if (MatchingManager.Instance != null)
        {
            MatchingManager.Instance.Rpc_SelectUser(nickname, Object.InputAuthority);
        }
    }
}
