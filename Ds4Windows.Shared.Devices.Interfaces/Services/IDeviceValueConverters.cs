﻿namespace DS4Windows.Shared.Devices.Services
{
    public interface IDeviceValueConverters
    {
        int DeadZoneDoubleToInt(double val);
        double DeadZoneIntToDouble(int inVal);
        double RotationConvertFrom(double val);
        double RotationConvertTo(double val);
    }
}