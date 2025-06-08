
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
    [SerializeField] protected Image SkillQ;

    [SerializeField] protected Image SkillW;
    [SerializeField] protected Image SkillE;
    [SerializeField] protected Image SkillR;
    [SerializeField] protected Image SkillPassive;
    [SerializeField] protected Image CharacterProfile;
    
    [SerializeField] protected TMP_Text HpBar;
    [SerializeField] protected TMP_Text ManaBar;

    // 연결 해제 버튼
    [SerializeField] protected CharacterInGameData[] _characterInGameData;

    private string MaxHp;

    partial void AwakeUser();
    partial void InitUser();
    partial void ShowUser();
    partial void HideUser();
    public void UpdateUI(CharacterDataEnum character)
    {
        int i = (int)character;
        SkillQ.sprite = _characterInGameData[i].SkillQ;
        SkillW.sprite = _characterInGameData[i].SkillW;
        SkillE.sprite = _characterInGameData[i].SkillE;
        SkillR.sprite = _characterInGameData[i].SkillR;
        SkillPassive.sprite = _characterInGameData[i].SkillPassive;
        CharacterProfile.sprite = _characterInGameData[i].CharacterProfile;
        HpBar.text = $"{_characterInGameData[i].HpBar} / {_characterInGameData[i].HpBar}";
        ManaBar.text = $"{_characterInGameData[i].ManaBar} / {_characterInGameData[i].ManaBar}";
    }

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
        
    }

    // 화면이 닫힐 때 호출
    // 플레이어 목록 갱신을 위한 코루틴 중단
    public override void Hide()
    {
        base.Hide();
        HideUser();
    }


    // 연결 끊기 버튼이 눌렸을 때 실행됨
    // 연결을 끊고 메인 메뉴 화면 으로 전환
    protected virtual async void OnDisconnectPressed()
    {
        await Connection.DisconnectAsync(ConnectFailReason.UserRequest);
        Controller.Show<FusionMenuUIMain>();
    }
}