
using System.Collections;
using System.Text;
using Fusion.Menu;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// 게임 플레이 중 표시되는 실시간 세션 정보 및 플레이어 관리 UI를 구현한 클래스입니다.
public partial class UIGamePlay : FusionMenuUIScreen
{
    // 플레이어 이름 목록 표시용 텍스트
    [SerializeField] protected TMP_Text _playersText;

    // 연결 해제 버튼
    [SerializeField] protected Button _disconnectButton;
    public float UpdateUsernameRateInSeconds = 2;
    protected Coroutine _updateUsernameCoroutine;

    partial void AwakeUser();
    partial void InitUser();
    partial void ShowUser();
    partial void HideUser();

    public override void Awake()
    {
        base.Awake();
        AwakeUser();
    }
    // 화면이 열릴 때 자동 호출
    // 세션 코드가 유효하면 텍스트에 표시하고 UI 활성화
    // 플레이어 리스트 갱신 시작 (UpdateUsernamesCoroutine() 실행)
    public override void Show()
    {
        base.Show();
        ShowUser();
        
        UpdateUsernames();

        if (UpdateUsernameRateInSeconds > 0)
        {
            _updateUsernameCoroutine = StartCoroutine((UpdateUsernamesCoroutine()));
        }
    }

    // 화면이 닫힐 때 호출
    // 플레이어 목록 갱신을 위한 코루틴 중단
    public override void Hide()
    {
        base.Hide();
        HideUser();

        if (_updateUsernameCoroutine != null)
        {
            StopCoroutine(_updateUsernameCoroutine);
            _updateUsernameCoroutine = null;
        }
    }

    // 연결 끊기 버튼이 눌렸을 때 실행됨
    // 연결을 끊고 메인 메뉴 화면 으로 전환
    protected virtual async void OnDisconnectPressed()
    {
        await Connection.DisconnectAsync(ConnectFailReason.UserRequest);
        Controller.Show<FusionMenuUIMain>();
    }
    
    // UpdateUsernameRateInSeconds 주기로 플레이어 목록 갱신
    protected virtual IEnumerator UpdateUsernamesCoroutine()
    {
        while (UpdateUsernameRateInSeconds > 0)
        {
            yield return new WaitForSeconds(UpdateUsernameRateInSeconds);
            UpdateUsernames();
        }
    }
    // 현재 접속 중인 플레이어 이름을 표시
    // 총 접속자 수와 최대 인원 수를 함께 표시
    // 플레이어가 없으면 해당 UI 전체 비활성화
    protected virtual void UpdateUsernames()
    {
        
    }

}