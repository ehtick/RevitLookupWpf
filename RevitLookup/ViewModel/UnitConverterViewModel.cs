﻿using GalaSoft.MvvmLight;
using RevitLookupWpf.PropertySys;
using RevitLookupWpf.PropertySys.BaseProperty;
using RevitLookupWpf.PropertySys.BaseProperty.ReferenceType;
using RevitLookupWpf.PropertySys.BaseProperty.ValueType;
using RevitLookupWpf.Unit;

namespace RevitLookupWpf.ViewModel
{
    public class UnitConverterViewModel : ObservableObject
    {
        private UnitItem _result;

        public UnitItem Result { get => _result; set =>  Set(ref _result , value); }

        public void Update(PropertyBase property)
        {
            if (!property.NeedUnitConvert)
            {
                Result = null;
            }

            if (property is DoubleProperty doubleProperty)
            {
                switch (property.Name)
                {
                    case "SurfaceArea":
                    case "Area":
                        Result = UnitItem.CreateByValue(doubleProperty.Value, UnitType.Area);
                        break;
                    case "Volume":
                        Result = UnitItem.CreateByValue(doubleProperty.Value, UnitType.Volume);
                        break;
                    default:
                        Result = UnitItem.CreateByValue(doubleProperty.Value, UnitType.Normal);
                        break;
                }

            }
            else if (property is StringProperty)
            {

            }else if (property is XYZProperty xYZProperty)
            {
                Result = UnitItem.CreateByXYZ(xYZProperty.Value);
            }
        }
    }
}
