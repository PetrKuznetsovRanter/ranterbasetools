using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RanterTools.Base
{

    public struct ReactiveNumber<T> where T : struct
    {

        public delegate void ValueChangeDelegate(T value);
        public event ValueChangeDelegate OnValueChanged;
        T value;
        public T Value
        {
            get { return value; }
            set
            {
                this.value = value;
                if (OnValueChanged != null) OnValueChanged(value);
            }
        }

        public static implicit operator ReactiveNumber<T>(T x)
        {
            return new ReactiveNumber<T> { Value = x };
        }
    }

}
