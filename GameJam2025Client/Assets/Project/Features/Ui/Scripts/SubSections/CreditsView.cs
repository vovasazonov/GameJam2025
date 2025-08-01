using System.Collections.Generic;
using System.Linq;
using System.Text;
using Project.Features.Credit.Scripts;
using TMPro;
using UnityEngine;

namespace Project.Features.Ui.Scripts.SubSections
{
    public class CreditsView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _developersCreditsText;
        [SerializeField] private TMP_Text _assetsCreditsText;

        private void Start()
        {
            var creditsData = CreditManager.Instance.GetCreditsText();
            
            var stringBuilder = new StringBuilder();
            foreach (var developer in creditsData.developers)
            {
                stringBuilder.AppendLine(developer);
            }
            _developersCreditsText.text = stringBuilder.ToString();
            stringBuilder.Clear();
            
            var randomizedAssets = creditsData.asset_credits
                .OrderBy(x => UnityEngine.Random.value);
            foreach (var assetCredit in randomizedAssets)
            {
                stringBuilder.AppendLine(assetCredit);
            }
            _assetsCreditsText.text = stringBuilder.ToString();
            stringBuilder.Clear();
        }
    }
}