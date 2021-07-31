using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RanterTools.Scenes
{
    [System.Serializable]
    public class SceneRef
    {
        /// <summary>
        /// Reference to scene asset.
        /// </summary>
        [SerializeField]
        private Object sceneAsset;
        /// <summary>
        /// Scene name
        /// </summary>
        [SerializeField]
        private string name = "";
        /// <summary>
        /// Scene name.
        /// </summary>
        /// <value>Scene name.</value>
        public string Name
        {
            get { return name; }
        }
        
        /// <summary>
        /// Makes it work with the existing Unity methods (LoadLevel/LoadScene)
        /// </summary>
        /// <param name="scene"></param>
        public static implicit operator string(SceneRef scene)
        {
            return scene.Name;
        }
        
        /// <summary>
        /// Standart constructor.
        /// </summary>
        /// <param name="name">Scene name.</param>
        public SceneRef(string name)
        {
            this.name = name;
        }
    }
}