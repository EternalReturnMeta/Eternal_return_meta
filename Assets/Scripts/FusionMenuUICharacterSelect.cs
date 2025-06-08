using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Fusion.Menu
{
    public partial class FusionMenuUICharacterSelect : FusionMenuUIScreen
    {
        private float duration = 20f;
        private Coroutine _countCoroutine;
        [SerializeField] private  TMP_Text _countText;
        [SerializeField] private Image countGauge;
        [SerializeField] private CharacterData[] playerData;
        [SerializeField] private Image[] playerImages;
        [SerializeField] private TMP_Text[] playerNames;
        [SerializeField] private Image FullImage;
        [SerializeField] private GameObject Deco;
        [SerializeField] private TMP_Text _characterName;
        partial void AwakeUser();
        partial void InitUser();
        partial void ShowUser();
        partial void HideUser();

        public override void Awake()
        {
            base.Awake();
            AwakeUser();
        }

        public override void Init()
        {
            base.Init();
            InitUser();
        }

        public override void Show()
        {
            base.Show();
            _countCoroutine = StartCoroutine(CountCoroutine(duration));
            ShowUser();
        }

        public override void Hide()
        {
            base.Hide();
            HideUser();
        }
        public void Update()
        {
            if (MatchingManager.Instance == null)
            {
                return;
            }
            var sortedPlayers = MatchingManager.Instance.SelectedCharacters
                .OrderBy(pair => pair.Key.PlayerId)
                .ToList();

            // 모든 이미지 초기화
            foreach (var img in playerImages)
            {
                img.color = new Color(1, 1, 1, 0f);
                img.sprite = null;
            }
            Debug.Log($"{sortedPlayers.Count} SortedPlayer입니다.");
            for (int i = 0; i < sortedPlayers.Count && i < playerImages.Length; i++)
            {
                int charId = (int)sortedPlayers[i].Value;
                if (charId == 2)
                    continue;
                if (charId < playerData.Length)
                {
                    playerImages[i].color = new Color(1f, 1f, 1f, 1f);
                    playerImages[i].sprite = playerData[charId].vsImage;
                }
            }
            
            var sortedUser = MatchingManager.Instance.SelectedUser
                .OrderBy(pair => pair.Key.PlayerId)
                .ToList();
            Debug.Log($"{sortedUser.Count} SortedUser입니다.");
            for (int i = 0; i < sortedUser.Count && i < playerImages.Length; i++)
            {
                string name = sortedUser[i].Value;
                Debug.Log($"{name} : name 입니다.");
                playerNames[i].text = name;
            }
        }
        
        public void UpdateMyCharacterImage(CharacterDataEnum characterId)
        {
            int charIdx = (int)characterId;
            if (charIdx < playerData.Length)
            {
                Deco.SetActive(true);
                FullImage.sprite = playerData[charIdx].fullImage;
                _characterName.text = playerData[charIdx].CharacterName;
            }
        }
        private IEnumerator CountCoroutine(float duration)
        {
            float elapsed = 0f;
            countGauge.fillAmount = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = Mathf.Clamp01(elapsed / duration);

                // 게이지 fillAmount 증가
                countGauge.fillAmount = t;

                // 남은 시간 표시 (초 단위, 소수점 1자리)
                float remain = Mathf.Max(0, duration - elapsed);
                _countText.text = Mathf.CeilToInt(remain).ToString();

                yield return null;
            }

            // 최종 값 보정
            countGauge.fillAmount = 1f;
            _countText.text = "0";
            _countCoroutine = null;
        }
    }
}