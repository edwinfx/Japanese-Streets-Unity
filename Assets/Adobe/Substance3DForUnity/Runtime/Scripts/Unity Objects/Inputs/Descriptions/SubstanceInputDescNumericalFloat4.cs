using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Adobe.Substance.Input.Description
{
    /// <summary>
    /// Numeric input description for input of type float4.
    /// </summary>
    [System.Serializable]
    public class SubstanceInputDescNumericalFloat4 : ISubstanceInputDescNumerical
    {
        [SerializeField]
        private Vector4 _DefaultValue;

        /// <summary>
        /// Default input value
        /// </summary>
        public Vector4 DefaultValue
        {
            get => _DefaultValue;
            set => _DefaultValue = value;
        }

        [SerializeField]
        private Vector4 _MinValue;

        /// <summary>
        /// Minimum value (UI hint only)
        /// </summary>
        public Vector4 MinValue
        {
            get => _MinValue;
            set => _MinValue = value;
        }

        [SerializeField]
        private Vector4 _MaxValue;

        /// <summary>
        /// Maximum value (UI hint only) (Only relevant if widget is Input_Slider)
        /// </summary>
        public Vector4 MaxValue
        {
            get => _MaxValue;
            set => _MaxValue = value;
        }

        [SerializeField]
        private float _SliderStep;

        /// <summary>
        /// Slider step size (UI hint only) (Only relevant if widget is Input_Slider)
        /// </summary>
        public float SliderStep
        {
            get => _SliderStep;
            set => _SliderStep = value;
        }

        [SerializeField]
        private bool _SliderClamp;

        /// <summary>
        /// Should the slider clamp the value? (UI hint only) (Only relevant if widget is Input_Slider)
        /// </summary>
        public bool SliderClamp
        {
            get => _SliderClamp;
            set => _SliderClamp = value;
        }

        [SerializeField]
        private int _EnumValueCount;

        /// <summary>
        /// Number of enum option for this value.
        /// </summary>
        public int EnumValueCount
        {
            get => _EnumValueCount;
            set => _EnumValueCount = value;
        }

        [SerializeField]
        private SubstanceFloat4EnumOption[] _EnumValues;

        /// <summary>
        /// Array of enum values for this property. Only relevant if widget is SBSARIO_WIDGET_COMBOBOX (UI hint only).
        /// </summary>
        public SubstanceFloat4EnumOption[] EnumValues
        {
            get => _EnumValues;
            set => _EnumValues = value;
        }
    }

    [System.Serializable]
    public class SubstanceFloat4EnumOption
    {
        public Vector4 Value;

        public string Label;
    }
}