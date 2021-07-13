using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RanterTools.Localization
{
    /// <summary>
    /// Component for localized text.
    /// </summary>
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class LocalizedText : MonoBehaviour
    {
        #region State
        TextMeshProUGUI tmproUGUIText;

        TextMeshProUGUI TMProUGUIText
        {
            get { return tmproUGUIText ?? GetComponent<TextMeshProUGUI>(); }
            set { tmproUGUIText = value; }
        }
        #endregion

        #region Parameters

        [SerializeField]
        string key;
        #endregion Parameters

        #region Unity
        /// <summary>
        /// Awake is called when the script instance is being loaded.
        /// </summary>
        void Awake()
        {
            if (LocalizationManager.Instance != null)
                TMProUGUIText.text = LocalizationManager.GetLocalizedValue(key);
        }
        #endregion Unity
    }
}