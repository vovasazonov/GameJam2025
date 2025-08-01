using System.Collections.Generic;
using Project.Core.Scripts;
using UnityEngine;

namespace Project.Features.Credit.Scripts
{
    public class CreditManager : SingletonBehaviour<CreditManager>
    {
        [SerializeField] private TextAsset _json;
        
        public CreditData GetCreditsText()
        {
            var data = JsonUtility.FromJson<CreditData>(_json.text);
            return data;
        }
    }

    public class CreditData
    {
        public List<string> developers;
        public List<string> asset_credits;
    }
}