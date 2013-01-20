﻿<ResourceDictionary
    xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
    xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
    xmlns:system="clr-namespace:System;assembly=mscorlib"
    xmlns:fed="clr-namespace:Microsoft.WindowsAzure.Samples.Phone.Identity.AccessControl;assembly=Phone.Identity.AccessControl">

    <!-- ACS resources -->
    <fed:SimpleWebTokenStore x:Key="swtStore" />
    <system:String x:Key="acsNamespace">youracsnamespace</system:String>
    <system:String x:Key="realm">yourrealm</system:String>

</ResourceDictionary>