using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RanterTools.Base
{
    public interface IScoreDealer
    {
        #region Parameters
        /// <summary>
        /// Use this score or ignore.
        /// </summary>
        /// <value> Use this score or ignore.</value>
        bool DealScore { get; set; }
        /// <summary>
        /// Amount of score that must be apply.
        /// </summary>
        /// <value>Amount of score that must be apply.</value>
        float Score { get; }
        #endregion Parameters
    }

}